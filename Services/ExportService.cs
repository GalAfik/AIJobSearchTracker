using ClosedXML.Excel;
using JobSearchTracker.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for exporting and importing job search data.
    /// </summary>
    public class ExportService
    {
        /// <summary>
        /// Exports a job search project to an Excel file.
        /// </summary>
        /// <param name="project">The project to export.</param>
        /// <param name="filePath">The file path where the Excel file should be saved.</param>
        public void ExportToExcel(JobSearchProject project, string filePath)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            using var workbook = new XLWorkbook();

            // Create Jobs worksheet
            CreateJobsWorksheet(workbook, project);

            // Create Interviews worksheet
            CreateInterviewsWorksheet(workbook, project);

            // Create Contacts worksheet
            CreateContactsWorksheet(workbook, project);

            // Create Summary worksheet
            CreateSummaryWorksheet(workbook, project);

            workbook.SaveAs(filePath);
        }

        /// <summary>
        /// Exports an unemployment report containing jobs applied to within the last 7 days.
        /// </summary>
        /// <param name="jobs">The list of jobs to include in the report (pre-filtered).</param>
        /// <param name="filePath">The file path where the Excel file should be saved.</param>
        public void ExportUnemploymentReport(List<Job> jobs, string filePath)
        {
            if (jobs == null)
                throw new ArgumentNullException(nameof(jobs));

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Unemployment Report");

            // Headers
            worksheet.Cell(1, 1).Value = "Company Name";
            worksheet.Cell(1, 2).Value = "Job Title";
            worksheet.Cell(1, 3).Value = "Date Applied";
            worksheet.Cell(1, 4).Value = "Website";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Data rows
            int row = 2;
            foreach (var job in jobs)
            {
                worksheet.Cell(row, 1).Value = job.CompanyName;
                worksheet.Cell(row, 2).Value = job.JobTitle;
                worksheet.Cell(row, 3).Value = job.DateApplied?.ToString("MM/dd/yyyy") ?? "";
                worksheet.Cell(row, 4).Value = job.JobUrl ?? "";
                row++;
            }

            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(filePath);
        }

        private void CreateJobsWorksheet(XLWorkbook workbook, JobSearchProject project)
        {
            var worksheet = workbook.Worksheets.Add("Jobs");

            // Headers
            worksheet.Cell(1, 1).Value = "Company Name";
            worksheet.Cell(1, 2).Value = "Job Title";
            worksheet.Cell(1, 3).Value = "Location";
            worksheet.Cell(1, 4).Value = "Status";
            worksheet.Cell(1, 5).Value = "Date Posted";
            worksheet.Cell(1, 6).Value = "Date Applied";
            worksheet.Cell(1, 7).Value = "Platform";
            worksheet.Cell(1, 8).Value = "Salary Range";
            worksheet.Cell(1, 9).Value = "Job URL";
            worksheet.Cell(1, 10).Value = "# of Interviews";
            worksheet.Cell(1, 11).Value = "# of Contacts";
            worksheet.Cell(1, 12).Value = "Date Rejected";
            worksheet.Cell(1, 13).Value = "Date Offered";
            worksheet.Cell(1, 14).Value = "Notes";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 14);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            // Data
            int row = 2;
            foreach (var job in project.Jobs.OrderByDescending(j => j.DateAdded))
            {
                worksheet.Cell(row, 1).Value = job.CompanyName;
                worksheet.Cell(row, 2).Value = job.JobTitle;
                worksheet.Cell(row, 3).Value = job.Location;
                worksheet.Cell(row, 4).Value = job.Status.ToString();
                worksheet.Cell(row, 5).Value = job.DatePosted?.ToString("MM/dd/yyyy") ?? "";
                worksheet.Cell(row, 6).Value = job.DateApplied?.ToString("MM/dd/yyyy") ?? "";
                worksheet.Cell(row, 7).Value = job.ApplicationPlatform.ToString();
                worksheet.Cell(row, 8).Value = job.SalaryRange;
                worksheet.Cell(row, 9).Value = job.JobUrl;
                worksheet.Cell(row, 10).Value = job.Interviews.Count;
                worksheet.Cell(row, 11).Value = job.Contacts.Count;
                worksheet.Cell(row, 12).Value = job.DateRejected?.ToString("MM/dd/yyyy") ?? "";
                worksheet.Cell(row, 13).Value = job.DateOffered?.ToString("MM/dd/yyyy") ?? "";
                worksheet.Cell(row, 14).Value = job.Notes;

                // Gray out rejected jobs
                if (job.Status == JobStatus.Rejected)
                {
                    worksheet.Range(row, 1, row, 14).Style.Fill.BackgroundColor = XLColor.LightGray;
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();
        }

        private void CreateInterviewsWorksheet(XLWorkbook workbook, JobSearchProject project)
        {
            var worksheet = workbook.Worksheets.Add("Interviews");

            // Headers
            worksheet.Cell(1, 1).Value = "Company Name";
            worksheet.Cell(1, 2).Value = "Job Title";
            worksheet.Cell(1, 3).Value = "Round";
            worksheet.Cell(1, 4).Value = "Interview Type";
            worksheet.Cell(1, 5).Value = "Date & Time";
            worksheet.Cell(1, 6).Value = "Interviewer Name";
            worksheet.Cell(1, 7).Value = "Interviewer Email";
            worksheet.Cell(1, 8).Value = "Outcome";
            worksheet.Cell(1, 9).Value = "Notes";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            // Data
            int row = 2;
            foreach (var job in project.Jobs)
            {
                foreach (var interview in job.Interviews.OrderBy(i => i.InterviewDateTime))
                {
                    worksheet.Cell(row, 1).Value = job.CompanyName;
                    worksheet.Cell(row, 2).Value = job.JobTitle;
                    worksheet.Cell(row, 3).Value = interview.Round;
                    worksheet.Cell(row, 4).Value = interview.InterviewType;
                    worksheet.Cell(row, 5).Value = interview.InterviewDateTime?.ToString("MM/dd/yyyy HH:mm") ?? "";
                    worksheet.Cell(row, 6).Value = interview.InterviewerName;
                    worksheet.Cell(row, 7).Value = interview.InterviewerEmail;
                    worksheet.Cell(row, 8).Value = interview.Outcome;
                    worksheet.Cell(row, 9).Value = interview.Notes;
                    row++;
                }
            }

            worksheet.Columns().AdjustToContents();
        }

        private void CreateContactsWorksheet(XLWorkbook workbook, JobSearchProject project)
        {
            var worksheet = workbook.Worksheets.Add("Contacts");

            // Headers
            worksheet.Cell(1, 1).Value = "Company Name";
            worksheet.Cell(1, 2).Value = "Contact Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Position";
            worksheet.Cell(1, 5).Value = "Notes";

            // Style headers
            var headerRange = worksheet.Range(1, 1, 1, 5);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

            // Data
            int row = 2;
            foreach (var job in project.Jobs)
            {
                foreach (var contact in job.Contacts)
                {
                    worksheet.Cell(row, 1).Value = job.CompanyName;
                    worksheet.Cell(row, 2).Value = contact.Name;
                    worksheet.Cell(row, 3).Value = contact.Email;
                    worksheet.Cell(row, 4).Value = contact.Position;
                    worksheet.Cell(row, 5).Value = contact.Notes;
                    row++;
                }
            }

            worksheet.Columns().AdjustToContents();
        }

        private void CreateSummaryWorksheet(XLWorkbook workbook, JobSearchProject project)
        {
            var worksheet = workbook.Worksheets.Add("Summary");

            worksheet.Cell(1, 1).Value = "Project Summary";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 16;

            int row = 3;
            worksheet.Cell(row++, 1).Value = "Project Name:";
            worksheet.Cell(row - 1, 2).Value = project.Name;

            worksheet.Cell(row++, 1).Value = "Description:";
            worksheet.Cell(row - 1, 2).Value = project.Description;

            worksheet.Cell(row++, 1).Value = "Date Created:";
            worksheet.Cell(row - 1, 2).Value = project.DateCreated.ToString("MM/dd/yyyy");

            row++;

            // Statistics
            worksheet.Cell(row++, 1).Value = "Statistics";
            worksheet.Cell(row - 1, 1).Style.Font.Bold = true;

            worksheet.Cell(row, 1).Value = "Total Jobs:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count;

            worksheet.Cell(row, 1).Value = "Not Applied:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.NotApplied);

            worksheet.Cell(row, 1).Value = "Applied:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Applied);

            worksheet.Cell(row, 1).Value = "Interviewed:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Interviewed);

            worksheet.Cell(row, 1).Value = "Offered:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Offered);

            worksheet.Cell(row, 1).Value = "Accepted:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Accepted);

            worksheet.Cell(row, 1).Value = "Rejected:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Rejected);

            worksheet.Cell(row, 1).Value = "Withdrawn:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Count(j => j.Status == JobStatus.Withdrawn);

            row++;
            worksheet.Cell(row, 1).Value = "Total Interviews:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Sum(j => j.Interviews.Count);

            worksheet.Cell(row, 1).Value = "Total Contacts:";
            worksheet.Cell(row++, 2).Value = project.Jobs.Sum(j => j.Contacts.Count);

            worksheet.Columns().AdjustToContents();
        }
    }
}
