using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace Fvent.Service.Utils;

public class FirebaseService
{
    private static FirebaseApp? _firebaseApp;

    public FirebaseService(string serviceAccountPath)
    {
        if (_firebaseApp == null)
        {
            _firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(serviceAccountPath)
            });
        }
    }

    public async Task SendNotificationAsync(string token, string title, string body)
    {
        if (string.IsNullOrEmpty(token)) throw new ArgumentException("FCM token cannot be null or empty.", nameof(token));

        var message = new Message
        {
            Token = token,
            Notification = new Notification
            {
                Title = title,
                Body = body
            }
        };

        try
        {
            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
        catch (Exception ex)
        {
            // Log error appropriately
            Console.WriteLine($"Failed to send notification: {ex.Message}");
        }
    }

    public async Task SendBulkNotificationsAsync(IEnumerable<string> tokens, string title, string body)
    {
        var validTokens = tokens.Where(token => !string.IsNullOrEmpty(token)).ToList();

        if (!validTokens.Any()) return;

        var message = new MulticastMessage
        {
            Tokens = validTokens,
            Notification = new Notification
            {
                Title = title,
                Body = body
            }
        };

        try
        {
            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            Console.WriteLine($"Successfully sent {response.SuccessCount} notifications, {response.FailureCount} failures.");
        }
        catch (Exception ex)
        {
            // Log error appropriately
            Console.WriteLine($"Failed to send bulk notifications: {ex.Message}");
        }
    }
}
