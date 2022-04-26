using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zoolon_container.player
{
    class factory
    {
        public static iplayer CreatePlayer(PlayerType pType ,string source, double width, double height)
        {
            iplayer player;
            switch (pType)
            {
                case PlayerType.PPT:
                    player = new PptPlayer(source);
                    break;
                case PlayerType.Video:
                    player = new VideoPlayer(source);
                    break;
                case PlayerType.Web:
                    player = new WebPlayer(source);
                    break;
                case PlayerType.Image:
                    player=new ImagePlayer(source);
                    break;
                default:
                    player = new UnknownPlayer();
                    break;
            }

            player.GetComponents().Width = width;
            player.GetComponents().Height = height;
            return player;

        }
    }
}
