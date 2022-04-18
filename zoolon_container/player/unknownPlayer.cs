using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace zoolon_container.player
{
    internal class UnknownPlayer : iplayer
    {
        customUnknownPlayer player = new customUnknownPlayer();

        public bool Close()
        {
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
            return player;
        }

        public PlayerType getType()
        {
            return PlayerType.Unknown;
        }

        public bool Open(string sourceDir)
        {
            throw new NotImplementedException();
        }
    }
}
