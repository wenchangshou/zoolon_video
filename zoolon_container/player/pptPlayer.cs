using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;

namespace zoolon_container.player
{
    class pptPlayer : iplayer
    {
        private customPptPlayer player;
        string _source;
        public pptPlayer(string source)
        {
         
            _source = source;
            player = new customPptPlayer(source);
   
            

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
            return player;
        }
   
        public bool Open(string sourceDir)
        {
            throw new NotImplementedException();
        }
    }
}
