using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoolon_videoplayer
{
    public class setVolumeCmdParams
    {
        public int volume { get; set; }
    }
    public class setVolumeCmd
    {
        public string action { get; set; }
        public setVolumeCmdParams arguments { get; set; }
    }
}
