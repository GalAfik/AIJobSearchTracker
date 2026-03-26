using JobSearchTracker.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// AI job scraping service using Anthropic's Claude API.
    /// </summary>
    public class ClaudeJobScrapingService : IAiJobScrapingService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://api.anthropic.com/v1/messages";

        public string ProviderName => "Claude (Anthropic)";

        public ClaudeJobScrapingService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        }

        public async Task<bool> ValidateApiKeyAsync()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                return false;

            try
            {
                var testRequest = new
                {
                    model = "claude-3-haiku-20240307",
                    max_tokens = 10,
                    messages = new[]
                    {
                        new { role = "user", content = "Hi" }
                    }
                };

                var json = JsonSerializer.Serialize(testRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiUrl, content);

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

                var request = new
                {
                    model = "claude-3-5-sonnet-20241022",
                    max_tokens = 2048,
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    }
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(ApiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                    // Check for insufficient credits
                    if (response.StatusCode == System.Net.HttpStatusCode.PaymentRequired || 
                        errorContent.Contains("insufficient") || 
                        errorContent.Contains("quota"))
                    {
                        return new AiJobScrapingResult
                        {
                            Success = false,
                            ErrorMessage = "Insufficient API credits. Please check your Anthropic account.",
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
                return ParseClaudeResponse(responseJson);
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

        private AiJobScrapingResult ParseClaudeResponse(string responseJson)
        {
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;

                if (!root.TryGetProperty("content", out var contentArray))
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "Invalid response format from Claude API."
                    };
                }

                var firstContent = contentArray[0];
                if (!firstContent.TryGetProperty("text", out var textElement))
                {
                    return new AiJobScrapingResult
                    {
                        Success = false,
                        ErrorMessage = "No text content in Claude response."
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
