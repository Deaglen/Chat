using Chat.Server.SocketsManager;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;
using Core;
using Newtonsoft.Json;
using Chat.DB;
using Chat.Server.Logging;
namespace Chat.Server.Handlers
{
    public class MessageHandler : SocketHandler
    {
        private readonly ConcurrentDictionary<User, WebSocket> _connections = new();

        public MessageHandler(ConnectionManager connectionManager) : base(connectionManager)
        {

        }

        public string _name;
        public bool noName = false;
        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = $"User{_connections.Count}"
            };
            _name = user.Name;
            _connections.TryAdd(user, socket);

            Log log = new Log();
            log.Trace("Server: New connection " + user.Name.ToString());
            //  await SendMessageToAll(DateTime.Now.ToString() + " " + user.Name.ToString() + " connected");
        }

        public override async Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Message messageObject = JsonConvert.DeserializeObject<Message>(message);
            // await SendMessageToAll(messageObject.Text);

            string name = _name;
            DataRepository dataRep = new DataRepository();




            Data msg = new Data()
            {
                Content = messageObject.Text,
                Nick = messageObject.Name,
                Time = DateTime.Now // время для сервера
            };

            if (messageObject.Name == string.Empty)
            {
                // msg.Nick = _name;
                name = _name;
                noName = true;
                messageObject.Name = _name;
            }
            else
            {
                //_name = msg.Nick;
                name = msg.Nick;
            }

            DateTime Time;
            if (noName == true) // это для десктоп1, так как он не основной, то делаем так
            {
                name = _name;
                Time = DateTime.Now;
                noName = false;
            }
            else
            {
                Time = msg.Time;
            }
            dataRep.Save(msg);
            var Msgs = dataRep.GetDatas();

            if (name == string.Empty)
                name = _name;
            // _name = " 123";
            await SendMessageToAll(Time + " : " + name + " : " + messageObject.Text);
            await SendMessageToAll(" !: server info: " + " idName: " + _name + " Nick: " + Msgs[Msgs.Count - 1].Nick + " " + "content:" + " " + Msgs[Msgs.Count - 1].Content + " " + " serverTime: " + Msgs[Msgs.Count - 1].Time);
            Log log = new Log();
            log.Trace(" !: server info: " + " idName: " + _name + " Nick: " + Msgs[Msgs.Count - 1].Nick + " " + "content:" + " " + Msgs[Msgs.Count - 1].Content + " " + " serverTime: " + Msgs[Msgs.Count - 1].Time);
            // await SendMessageToAll(messageObject.Text);
        }

    }
}
