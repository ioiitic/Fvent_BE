using System.Text;
using System.Text.Json;
using Google.Apis.Auth.OAuth2;

namespace Fvent.Service.Services.Imp
{
    public class FirebaseService
    {
        private readonly string _projectId;
        private readonly string _serviceAccountPath;

        public FirebaseService(string serviceAccountPath)
        {
            if (!File.Exists(serviceAccountPath))
                throw new FileNotFoundException("Firebase service key file not found.", serviceAccountPath);

            _serviceAccountPath = serviceAccountPath;

            // Extract project ID from the service account key
            var serviceAccountJson = File.ReadAllText(serviceAccountPath);
            var jsonDoc = JsonDocument.Parse(serviceAccountJson);

            _projectId = jsonDoc.RootElement.TryGetProperty("project_id", out var projectId)
                ? projectId.GetString()!
                : throw new Exception("Project ID not found in the service account file.");
        }

        /// <summary>
        /// Generates an OAuth 2.0 access token using the service account key.
        /// </summary>
        /// <returns>Access token string.</returns>
        private async Task<string> GetAccessTokenAsync()
        {
            var credential = GoogleCredential.FromFile(_serviceAccountPath)
                                              .CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            return await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        }

        /// <summary>
        /// Sends a single notification to an FCM token.
        /// </summary>
        public async Task SendNotificationAsync(string token, string title, string body)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("FCM token cannot be null or empty.", nameof(token));

            var url = $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send";

            var message = new
            {
                message = new
                {
                    token = token,
                    notification = new
                    {
                        title = title ?? "Default Title",
                        body = body ?? "Default Body"
                    }
                }
            };

            using var client = new HttpClient();

            try
            {
                // Generate access token
                var accessToken = await GetAccessTokenAsync();
                Console.WriteLine($"Access Token: {accessToken}");

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

                // Send request
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Response: {responseBody}");

                response.EnsureSuccessStatusCode();
                Console.WriteLine("Notification sent successfully!");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }


        /// <summary>
        /// Sends bulk notifications to multiple FCM tokens.
        /// </summary>
        public async Task SendBulkNotificationsAsync(IEnumerable<string> tokens, string title, string body)
        {
            // Ensure the tokens are unique
            var validTokens = tokens.Where(token => !string.IsNullOrEmpty(token)).Distinct().ToList();
            if (!validTokens.Any())
            {
                Console.WriteLine("No valid FCM tokens provided.");
                return;
            }

            var tasks = validTokens.Select(token => SendNotificationAsync(token, title, body));
            await Task.WhenAll(tasks);

            Console.WriteLine("Bulk notifications sent successfully!");
        }

    }
}

