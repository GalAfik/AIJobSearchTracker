using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for email-related operations.
    /// </summary>
    public class EmailService
    {
        /// <summary>
        /// Opens the default email client with a new message.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        public void ComposeEmail(string toEmail, string subject = "", string body = "")
        {
            try
            {
                var encodedSubject = HttpUtility.UrlEncode(subject);
                var encodedBody = HttpUtility.UrlEncode(body);
                var mailtoUrl = $"mailto:{toEmail}?subject={encodedSubject}&body={encodedBody}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = mailtoUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open email client.", ex);
            }
        }

        /// <summary>
        /// Opens Gmail in the default browser with a new message.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        public void ComposeGmail(string toEmail, string subject = "", string body = "")
        {
            try
            {
                var encodedSubject = HttpUtility.UrlEncode(subject);
                var encodedBody = HttpUtility.UrlEncode(body);
                var gmailUrl = $"https://mail.google.com/mail/?view=cm&fs=1&to={toEmail}&su={encodedSubject}&body={encodedBody}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = gmailUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open Gmail.", ex);
            }
        }

        /// <summary>
        /// Opens Outlook Web in the default browser with a new message.
        /// </summary>
        /// <param name="toEmail">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email.</param>
        public void ComposeOutlookWeb(string toEmail, string subject = "", string body = "")
        {
            try
            {
                var encodedSubject = HttpUtility.UrlEncode(subject);
                var encodedBody = HttpUtility.UrlEncode(body);
                var outlookUrl = $"https://outlook.office.com/mail/deeplink/compose?to={toEmail}&subject={encodedSubject}&body={encodedBody}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = outlookUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open Outlook Web.", ex);
            }
        }
    }
}
