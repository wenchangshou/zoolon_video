using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoolon_container
{
    internal class replyMessage
    {
        public bool reply=false;
        public string content="";
    }
    internal interface iplayer
    {
        public replyMessage control(string body);
        public bool close();
        public bool exit();
        public bool open(string sourceDir);
    }
}
