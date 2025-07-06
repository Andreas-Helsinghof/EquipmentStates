using EquipmentStates.OrdersAPI.Models;
using System.Collections.Concurrent;

namespace EquipmentStates.OrdersAPI.Services
{
    public class EquipmentSSEService
    {
        private static readonly ConcurrentDictionary<Guid, EquipmentSSERegistration> Registrations = new();

        public void RegisterEquipmentSSE(RegisterEquipmentStateRequest request)
        {
            Registrations[request.EquipmentId] = new EquipmentSSERegistration
            {
                EquipmentId = request.EquipmentId,
                SSEurl = request.SSEurl,
                RegisteredAt = DateTime.UtcNow
            };
            _ = SubscribeToSSEAsync(request.SSEurl);
        }

        private async Task SubscribeToSSEAsync(string sseUrl)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                using var request = new HttpRequestMessage(HttpMethod.Get, sseUrl);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/event-stream"));
                using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine($"[SSE] {sseUrl}: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SSE] Error subscribing to {sseUrl}: {ex.Message}");
            }
        }
    }
}
