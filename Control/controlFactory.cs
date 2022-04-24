using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public enum ProtocolType
    {
        Daemon,
        Grpc
    }
     public class ControlFactory
    {
        public static Icontrol Make(ProtocolType protocol,Options option)
        {
            if (protocol == ProtocolType.Daemon)
            {
                string uri = $"ws://{option.WebsocketIP}:{option.WebsocketPort}";
               var client= new DaemonClient(uri,option.WebsocketInstanceName);
                client.Start();
                return client;
            }
            return null;
        }
    }
}
