using JobSearchTracker.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// AI job scraping service using Google's Gemini API.
    /// </summary>
    public class GeminiJobScrapingService : IAiJobScrapingService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

        public string ProviderName => "Gemini (Google)";

        public GeminiJobScrapingService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient();
        }

        public async Task<bool> ValidateApiKeyAsync()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return false;

            try
            {
                var url = $"{ApiBaseUrl}?key={_apiKey}";
                var testRequest = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[] { new { text = "Hi" } }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(testRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<AiJobScrapingResult> ScrapeJobFromUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return new AiJobScrapingResult
                {
                    Success = false,
                    ErrorMessage = "URL is required."
                };
            }

            try
            {
                var prompt = BuildScrapingPrompt(url);
                var apiUrl = $"{ApiBaseUrl}?key={_apiKey}";

                var request = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[] { new { text = prompt } }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.3,
                        maxOutputTokens = 2048
                    }
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                    // Check for insufficient credits or quota
                    if (response.StatusCode == (System.Net.HttpStatusCode)429 ||
                        errorContent.Contains("quota") || 
                        errorContent.Contains("RESOURCE_EXHAUSTED"))
                    {
                        return new AiJobScrapingResult
                        {
                            Success = false,
                            ErrorMessage = "API quota exceeded. Please check your Google Cloud account.",
                            InsufficientCredits = true
                        };
                    }

                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = $"API request failed: {response.StatusCode}"
                    };
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                return ParseGeminiResponse(responseJson);
            }
            catch (HttpRequestException ex)
            {
                return new AiJobScrapingResult
                {
                    Success = false,
                    ErrorMessage = $"Network error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new AiJobScrapingResult
                {
                    Success = false,
                    ErrorMessage = $"Unexpected error: {ex.Message}"
                };
            }
        }

        private string BuildScrapingPrompt(string url)
        {
            return $@"Please visit the following job posting URL and extract the job details: {url}

Extract the following information and return it as a JSON object with these exact field names:
{{
  ""companyName"": ""string"",
  ""jobTitle"": ""string"",
  ""location"": ""string"",
  ""salaryRange"": ""string (or empty if not available)"",
  ""datePosted"": ""MM/dd/yyyy (or empty if not available)"",
  ""applicationPlatform"": ""string (LinkedIn, Indeed, CompanyWebsite, etc.)"",
  ""description"": ""string"",
  ""jobUrl"": ""{url}""
}}

Important:
- For applicationPlatform, determine from the URL (e.g., if URL contains 'linkedin.com', use 'LinkedInEasyApply')
- Extract as much information as possible
- If a field cannot be determined, use an empty string
- Return ONLY the JSON object, no other text";
        }

        private AiJobScrapingResult ParseGeminiResponse(string responseJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;

                if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid response format from Gemini API."
                    };
                }

                var firstCandidate = candidates[0];
                if (!firstCandidate.TryGetProperty("content", out var content) ||
                    !content.TryGetProperty("parts", out var parts) ||
                    parts.GetArrayLength() == 0)
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "No content in Gemini response."
                    };
                }

                var firstPart = parts[0];
                if (!firstPart.TryGetProperty("text", out var textElement))
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "No text in Gemini response."
                    };
                }

                var extractedText = textElement.GetString() ?? "";
                
                // Extract JSON from potential markdown code blocks
                var jsonStart = extractedText.IndexOf('{');
                var jsonEnd = extractedText.LastIndexOf('}');
                
                if (jsonStart == -1 || jsonEnd == -1)
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "Could not find JSON in AI response."
                    };
                }

                var jobJson = extractedText.Substring(jsonStart, jsonEnd - jsonStart + 1);
                var jobData = JsonDocument.Parse(jobJson).RootElement;

                var job = new Job
                {
                    CompanyName = jobData.GetProperty("companyName").GetString() ?? "",
                    JobTitle = jobData.GetProperty("jobTitle").GetString() ?? "",
                    Location = jobData.GetProperty("location").GetString() ?? "",
                    SalaryRange = jobData.GetProperty("salaryRange").GetString() ?? "",
                    JobUrl = jobData.GetProperty("jobUrl").GetString() ?? "",
                    Description = jobData.GetProperty("description").GetString() ?? "",
                    DateAdded = DateTime.Now
                };

                // Parse date posted if available
                if (jobData.TryGetProperty("datePosted", out var datePostedElement))
                {
                    var dateString = datePostedElement.GetString();
                    if (!string.IsNullOrWhiteSpace(dateString) && DateTime.TryParse(dateString, out var datePosted))
                    {
                        job.DatePosted = datePosted;
                    }
                }

                // Parse application platform
                if (jobData.TryGetProperty("applicationPlatform", out var platformElement))
                {
                    var platformString = platformElement.GetString() ?? "";
                    job.ApplicationPlatform = ParseApplicationPlatform(platformString);
                }

                return new AiJobScrapingResult
                {
                    Success = true,
                    Job = job
                };
            }
            catch (Exception ex)
            {
                return new AiJobScrapingResult
                {
                    Success = false,
                    ErrorMessage = $"Failed to parse AI response: {ex.Message}"
                };
            }
        }

        private ApplicationPlatform ParseApplicationPlatform(string platform)
        {
            return platform.ToLower() switch
            {
                var p when p.Contains("linkedin") => ApplicationPlatform.LinkedInEasyApply,
                var p when p.Contains("indeed") => ApplicationPlatform.Indeed,
                var p when p.Contains("glassdoor") => ApplicationPlatform.Glassdoor,
                var p when p.Contains("ziprecruiter") => ApplicationPlatform.ZipRecruiter,
                var p when p.Contains("handshake") => ApplicationPlatform.Handshake,
                var p when p.Contains("company") || p.Contains("website") => ApplicationPlatform.CompanyWebsite,
                var p when p.Contains("referral") => ApplicationPlatform.Referral,
                _ => ApplicationPlatform.Other
            };
        }
    }
}
