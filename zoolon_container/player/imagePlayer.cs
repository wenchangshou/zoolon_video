using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using zoolon_container.player;

namespace zoolon_container.player
{
    internal class ImagePlayer : iplayer
    {
        //ContentControl component;
        customImage component;
        public ImagePlayer(string source)
        {
            component = new customImage();
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

        public ContentControl GetComponents()
        {
            return component;
        }

        public bool Open(string sourceDir)
        {
            component.Source = sourceDir;
            return true;
        }
    }
}
