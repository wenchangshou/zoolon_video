using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace zoolon_container
{
    internal class ImagePlayer : Image, iplayer
    {
        public ImagePlayer(string source)
        {
            this.Open(source);
        }
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

        public bool Open(string sourceDir)
        {
            this.Source=new BitmapImage(new Uri(sourceDir));
            return true;
        }
    }
}
