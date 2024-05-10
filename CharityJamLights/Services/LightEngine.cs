using System.Net.WebSockets;
using CharityJamLights.Configuration;

namespace CharityJamLights.Services
{
    public class LightEngine
    {
        private readonly LightsOptions _options;

        public LightEngine(LightsOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task Send()
        {
            using (ClientWebSocket ws = new ClientWebSocket())
            {
                Uri serverUri = new Uri(_options.BaseUrl);
                await ws.ConnectAsync(serverUri, CancellationToken.None);
                Console.WriteLine("Connected!");

                ArraySegment<byte> bytesToSend = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes("Hello, World!"));
                await ws.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine("Message sent!");

                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await ws.ReceiveAsync(bytesReceived, CancellationToken.None);
                Console.WriteLine($"Message received: {System.Text.Encoding.UTF8.GetString(bytesReceived.Array, 0, result.Count)}");

                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                Console.WriteLine("Disconnected!");
            }
        }
    }
}
