using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;

namespace Control
{
    internal class DaemonClient
    {

        private Uri _uri;
        private String _name;
        public DaemonClient(String server,int port,String name)
        {
            String uri=String.Format("{0}:{1}", server, port);
            _uri=new Uri(uri);
            _name = name;
        }
        public int Start()
        {
            using (var client =new WebsocketClient(_uri))
            {
                client.Name = "client";
                client.ReconnectionHappened.Subscribe(type =>
                {
                    
                });
                client.DisconnectionHappened.Subscribe(type =>
                {

                });
                client.MessageReceived.Subscribe(msg =>
                {

                });

                client.Start();

                Task.Run(() => StartSendingPing(client));


            }

            return 1;
        }
        private static async Task StartSendingPing(WebsocketClient client)
        {
            while (true)
            {
                await Task.Delay(1000);
                
            }
        }

    }
}
