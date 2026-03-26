using System;
using System.Diagnostics;
using System.Web;

namespace JobSearchTracker.Services
{
    /// <summary>
    /// Service responsible for providing directions to job locations.
    /// </summary>
    public class DirectionsService
    {
        /// <summary>
        /// Opens Google Maps with directions from home to destination.
        /// </summary>
        /// <param name="fromAddress">The starting address (home).</param>
        /// <param name="toAddress">The destination address (job location).</param>
        public void GetDirections(string fromAddress, string toAddress)
        {
            if (string.IsNullOrWhiteSpace(toAddress))
            {
                throw new ArgumentException("Destination address is required.", nameof(toAddress));
            }

            var origin = string.IsNullOrWhiteSpace(fromAddress) ? "" : HttpUtility.UrlEncode(fromAddress);
            var destination = HttpUtility.UrlEncode(toAddress);

            string mapsUrl;
            if (string.IsNullOrWhiteSpace(fromAddress))
            {
                // If no home address, just show the destination
                mapsUrl = $"https://www.google.com/maps/search/?api=1&query={destination}";
            }
            else
            {
                // Show directions from home to destination
                mapsUrl = $"https://www.google.com/maps/dir/?api=1&origin={origin}&destination={destination}";
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = mapsUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to open Google Maps.", ex);
            }
        }
    }
}
