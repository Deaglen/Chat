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

        public string _ID_Name;

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = $"User{_connections.Count}"
            };
            _ID_Name = user.Name;
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

            // string name = _ID_Name;
            DataRepository dataRep = new DataRepository();

            if (messageObject.Name == null)
            {
                messageObject.Name = "ANANIMUS";
                messageObject.Time = DateTime.Now;
            }


            Data msg = new Data()
            {
                Content = messageObject.Text,
                Nick = messageObject.Name,
                Time = messageObject.Time,// время для сервера

            };

            dataRep.Save(msg);
            var Msgs = dataRep.GetDatas();


            // _name = " 123";
            await SendMessageToAll(messageObject.Time + " : " + messageObject.Name + " : " + messageObject.Text);
            await SendMessageToAll(" !: server info: " + " Nick: " + Msgs[Msgs.Count - 1].Nick + " " + "content:" + " " + Msgs[Msgs.Count - 1].Content + " " + " serverTime: " + Msgs[Msgs.Count - 1].Time);
            Log log = new Log();
            log.Trace(" !: server info: " + " Nick: " + Msgs[Msgs.Count - 1].Nick + " " + "content:" + " " + Msgs[Msgs.Count - 1].Content + " " + " serverTime: " + Msgs[Msgs.Count - 1].Time);
            // await SendMessageToAll(messageObject.Text);
        }

    }
}
