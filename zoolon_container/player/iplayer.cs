using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace zoolon_container
{
    public enum PlayerType
    {
        Video,
        Image,
        Web,
        PPT,
        Unknown
    }
    public class replyMessage
    {
        public bool reply=false;
        public string content="";
    }
    public interface iplayer
    {
        public replyMessage Control(string body);
        public bool Close();
        public bool Exit();
        public bool Open(string sourceDir);
        public ContentControl GetComponents();
    }
    public class Player : iplayer
    {
        public bool Close()
        {
            throw new NotImplementedException();
        }

        public replyMessage Control(string body)
        {
            throw new NotImplementedException();
        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public ContentControl GetComponents()
        {
            throw new NotImplementedException();
        }

        public bool Open(string sourceDir)
        {
            throw new NotImplementedException();
        }
        
    }
}
