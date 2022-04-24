using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoolon_container
{
    
    class MessageProcess 
    {
        Icontrol _control;
        iplayer _player;
        public MessageProcess(Options options,iplayer player)
        {
            if (options.Protocol == "daemon" && options.RegisterWebsocket)
            {
                _control = ControlFactory.Make(ProtocolType.Daemon, options);
            }
            if (_control != null)
            {
                _control.OnRecvMsg += _control_OnRecvMsg;
            }
            _player = player;
        }

        private ExecuteResult _control_OnRecvMsg(string body)
        {
            throw new NotImplementedException();
        }
    }
}
