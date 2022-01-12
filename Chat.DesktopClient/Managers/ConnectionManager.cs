
using Chat.DesktopClient.Views;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Chat.DesktopClient.Services;
namespace Chat.DesktopClient.Managers
{
    class ConnectionManager
    {
        private readonly string _api;

        public ClientWebSocket Client { get; private set; }

        public ConnectionManager(string api)
        {
            _api = api;
        }

        public async Task StartConnection()
        {
            Client = new ClientWebSocket();
            await Client.ConnectAsync(new Uri($"ws://localhost:5000/{_api}"), CancellationToken.None);

            while (1 == 1)
            {

                var receive = ReciveAsync(Client);
                await Task.WhenAll(receive);
            }

        }

        private async Task ReciveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                GetService _GetService = new GetService();
                _GetService.GetMsg(Encoding.UTF8.GetString(buffer, 0, result.Count));
                //((MainWindow)System.Windows.Application.Current.MainWindow).ListMsg.Items.Add(Encoding.UTF8.GetString(buffer, 0, result.Count));

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }

            }
        }
    }
}
