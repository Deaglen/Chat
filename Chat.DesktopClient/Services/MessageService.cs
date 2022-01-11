using System;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using Core;
using Chat.DesktopClient.Managers;
using Newtonsoft.Json;

using Chat.DesktopClient.ViewModels;
using System.Text.RegularExpressions;

namespace Chat.DesktopClient.Services
{
    class MessageService
    {
        private const string API = "message";

        private readonly ConnectionManager _connectionManager;

        public MessageService()
        {
            _connectionManager = new ConnectionManager(API);
            _ = _connectionManager.StartConnection();
        }

        public void SendMessage(string message)
        {
            if (Regex.IsMatch(message, @"\p{IsCyrillic}"))
            {

            }
            else
            {
                Message messageObject = new Message
                {
                    Text = message
                };

                var jsonMessage = JsonConvert.SerializeObject(messageObject);
                var bytes = Encoding.UTF8.GetBytes(jsonMessage);
                _connectionManager.Client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    public class GetService
    {
        public void GetMsg(string message)
        {

            MainWindowViewModel_ForGet _MainWindowViewModel_ForGet = new MainWindowViewModel_ForGet();
            _MainWindowViewModel_ForGet.GetMsg(message);

        }
    }

}
