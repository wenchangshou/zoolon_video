using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace zoolon_container.player
{
    internal class WebPlayer : iplayer
    {
        customWebPlayer _container;
        private string _source;
        
        public WebPlayer(string source)
        {
            _container = new customWebPlayer();
            _source = source;
            this.Open(_source);
        }
        public bool Close()
        {
            _container.Dispose();
            return true;
        }
        public replyMessage Control(string body)
        {
            throw new NotImplementedException();
        }

        public replyMessage Control(Hashtable args)
        {
            
            throw new NotImplementedException();

        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public ContentControl GetComponents()
        {
            return _container;
        }

        public PlayerType getType()
        {
            return PlayerType.Web;
        }

        public bool Open(string sourceDir)
        {
            _container.Source = sourceDir;
            return true;
        }
    }
}
