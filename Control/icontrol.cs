using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    internal interface icontrol
    {
        public delegate ExecuteResult RecvMsg(string body);
        public event RecvMsg OnRecvMsg;
        public void close();
        public string send(string body);
        public byte[] send(byte[] body);

    }
}
