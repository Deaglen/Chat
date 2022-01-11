using Chat.DesktopClient2.Core;
using Chat.DesktopClient2.DB;
using Chat.DesktopClient2.Logging;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace Chat.DesktopClient2
{

    public partial class MainWindow : Window
    {

        bool isOnline = false;
        ClientWebSocket CLIENT;
        string _nickname;

        public MainWindow()
        {
            InitializeComponent();
            tbNick.IsEnabled = true;
            MsgBox.IsEnabled = false;
            btSendMsg.IsEnabled = false;

        }

        public async Task StartConnection()
        {

            var client = new ClientWebSocket();
            CLIENT = client;
            await client.ConnectAsync(new Uri($"ws://localhost:5000/message"), CancellationToken.None);

            Log log = new Log();
            log.Trace("New connection");

            while (true)
            {

                var receive = ReciveAsync(client);
                await Task.WhenAll(receive);
            }
        }

        public async Task StopConnection()
        {
            await CLIENT.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);

        }

        private async Task ReciveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];

            while (true)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                MsgList.Items.Add(Encoding.UTF8.GetString(buffer, 0, result.Count));



                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }


        private void SendMsg_Click(object sender, RoutedEventArgs e)
        {

            DataRepository dataMsg = new DataRepository();
            Data Data1 = new Data()
            {
                Content = MsgBox.Text,
                Nick = _nickname,
                Time = DateTime.Now
            };
            dataMsg.Save(Data1);


            SendMsg(MsgBox.Text);


        }


        private void btConnect_Click(object sender, RoutedEventArgs e)
        {
            if (isOnline)
            {
                Disconnect();
            }
            else
            {
                if (tbNick.Text == "")
                {
                    _nickname = "ANANIMUS";
                    tbNick.Text = "ANANIMUS";
                }
                else
                {
                    _nickname = tbNick.Text;
                }
                Connect();


            }
        }

        public void Disconnect()
        {
            isOnline = false;
            tbNick.IsEnabled = true;
            MsgBox.IsEnabled = false;
            btSendMsg.IsEnabled = false;
            btConnect.Content = "Connect";
            SendMsg(tbNick.Text + " is disconnected");

            DataRepository dataRep = new DataRepository();
            Data Data1 = new Data()
            {
                Content = "Disconnected",
                Nick = _nickname,
                Time = DateTime.Now
            };
            dataRep.Save(Data1);


            _ = StopConnection();




        }
        public void Connect()
        {
            isOnline = true;
            tbNick.IsEnabled = false;
            MsgBox.IsEnabled = true;
            btSendMsg.IsEnabled = true;
            btConnect.Content = "Disconnect";

            MsgList.Items.Clear();
            _ = StartConnection();
            //Thread.Sleep(2000);


            DataRepository dataRep = new DataRepository();


            var Msgs = dataRep.GetDatas();

            foreach (Data u in Msgs)
            {

                MsgList.Items.Add(u.Time + " " + u.Nick + " : " + u.Content);
            }



            Data Data1 = new Data()
            {
                Content = "Connected",
                Nick = _nickname,
                Time = DateTime.Now
            };
            dataRep.Save(Data1);

            SendMsg(tbNick.Text + " is connected");




            MsgList.ScrollIntoView(MsgList.Items[MsgList.Items.Count - 1]);
        }


        public void SendMsg(string message)
        {
            var client = CLIENT;
            if (Regex.IsMatch(message, @"\p{IsCyrillic}") || Regex.IsMatch(_nickname, @"\p{IsCyrillic}"))
            {
                MsgBox.Text = "No Russian";
                Log log = new Log();
                log.Warn("RussianMessageWarning:Chat.DesktopClient2:SendMsg:" + _nickname + "::" + message);
            }
            else
            {
                Message messageObject = new Message
                {
                    Text = message,
                    Name = _nickname,
                    Time = DateTime.Now // тут время локальное а на сервере время принятия для сервера, там на пару секунд позже обычно
                };

                var jsonMessage = JsonConvert.SerializeObject(messageObject);
                var bytes = Encoding.UTF8.GetBytes(jsonMessage);
                client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Disconnect();
        }

    }
}

