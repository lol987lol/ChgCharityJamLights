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

        public async Task Send(int channel, byte value)
        {
            using (ClientWebSocket ws = new ClientWebSocket())
            {
                Uri serverUri = new Uri(_options.BaseUrl);
                await ws.ConnectAsync(serverUri, CancellationToken.None);

                await SendWhite(ws, 0);

                await Task.Delay(100);

                await FadeIn(ws);

                await Task.Delay(500);
                await FadeOut(ws);


                await Task.Delay(2000);

                await SendColor(ws, 117, 86, 240);

                await Task.Delay(4000);

                await SendChannel(ws, 1, 0);

                await Task.Delay(2000);

                await SendWhite(ws, 0);

            }
        }

        private static async Task FadeIn(ClientWebSocket ws)
        {
            for (int i = 0; i < 255; i++)
            {
                await SendWhite(ws, (byte)i);

                i += 10;
                await Task.Delay(150);
            }
        }

        private static async Task SendWhite(ClientWebSocket ws, byte i)
        {
            await SendColor(ws, i, i, i);
        }

        private static async Task SendColor(ClientWebSocket ws, byte r, byte g, byte b)
        {
            var tasks = new List<Task>();
            tasks.Add(SendChannel(ws, 1, r));
            tasks.Add(SendChannel(ws, 2, g));
            tasks.Add(SendChannel(ws, 3, b));
            await Task.WhenAll(tasks);
        }

        private static async Task FadeOut(ClientWebSocket ws)
        {
            for (int i = 255; i >= 0; i--)
            {
                await SendWhite(ws, (byte)i);
                i -= 10;
                await Task.Delay(150);
            }
        }

        private static async Task SendChannel(ClientWebSocket client, int channel, byte value)
        {
            var bytesToSend = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes($"CH|{channel}|{value}"));
            await client.SendAsync(bytesToSend, WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}
