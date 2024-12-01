using System.Net.Http;
using System.Text;
using System.Text.Json;

public class ExpoNotificationService
{
    private readonly HttpClient _httpClient;

    public ExpoNotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendNotificationAsync(string expoPushToken, string title, string body, object data)
    {
        var message = new
        {
            to = expoPushToken,
            sound = "default",
            title,
            body,
            data
        };

        var jsonMessage = JsonSerializer.Serialize(message);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://api.expo.dev/v2/push/send"),
            Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to send Expo notification: {response.StatusCode}, {responseBody}");
        }
    }
}
