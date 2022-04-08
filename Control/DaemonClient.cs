using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using Websocket.Client.Models;

namespace Base
{
    public class RegisterMsg
    {
        public string messageType { get; set; } = "RegisterToDaemon";
        public string SocketName { get; set; }
        public string proto { get; set; } = "binary";
    }
    public class DaemonClient : icontrol
    {
        const int MsgIDLength = 16;

        public String instanceName;
        public String uri;
        WebsocketClient client;

        public event icontrol.RecvMsg OnRecvMsg;

        public DaemonClient(String uri, String name)
        {
            this.instanceName = name;
            this.uri = uri;
            
        }

        private void Run()
        {
            var exitEvent = new ManualResetEvent(false);
            var url = new Uri(this.uri);
            client = new WebsocketClient(url);
            client.ReconnectionHappened.Subscribe(Reconnect);
            client.MessageReceived.Subscribe(MessageProcess);
            client.Start();
            exitEvent.WaitOne();
        }
        public void Start()
        {
            Thread thr1 = new Thread(Run);
            thr1.Start();
        }

        private void MessageProcess(ResponseMessage message)
        {
            byte[] id = new byte[16];
            if (message.MessageType == WebSocketMessageType.Binary)
            {

                var msg = new message(message.Binary);
                if (OnRecvMsg != null)
                {
                    var reply = OnRecvMsg(msg.body);
                    if (reply != null && reply.Reply && reply.Msg != null)
                    {

                        msg.body = reply.Msg;
                        msg.topic = "/zebus";
                        var b = msg.Encode();
                        client.Send(b);
                    }
                }
                return;
            }
             if(message.MessageType == WebSocketMessageType.Text)
            {
                if (OnRecvMsg != null)
                {
                    var reply = OnRecvMsg(message.Text);
                    if (reply != null && reply.Reply && reply.Msg != null)
                    {
                    }
                }
            }


        }

        public String genateRegisterMsg()
        {
            var msg = new RegisterMsg { SocketName = this.instanceName };
            string str = JsonConvert.SerializeObject(msg);
            return str;
        }
        private void Reconnect(ReconnectionInfo obj)
        {
            Console.WriteLine("reconnect:" + obj);
            string msg = genateRegisterMsg();
            client.Send(msg);

        }

        public void close()
        {
            throw new NotImplementedException();
        }

        public string send(string body)
        {
            throw new NotImplementedException();
        }

        public byte[] send(byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}
