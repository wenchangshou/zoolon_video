using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public interface Icontrol
    {
        public delegate ExecuteResult RecvMsg(string body);
        public event RecvMsg OnRecvMsg;
        public void Close();
        public string Send(string body);
        public byte[] Send(byte[] body);

    }
}
