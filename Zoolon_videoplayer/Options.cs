using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
namespace Zoolon_videoplayer
{
    public class Options
    {
        [Option('x',"xx",Required =false,Default =0,HelpText ="set winform x position")]
        public int x { get; set; }
        [Option('y',"yy",Required =false,Default =0,HelpText ="set winform y position")]
        public int y { get; set; }
        [Option('w',"width",Required =false,Default =1920,HelpText ="set winform width")]
        public int width { get; set; }
        [Option('h',"height",Required =false,Default =1080)]
        public int height { get; set; }
        [Option('p',"port",Required =false,Default =0)]
        public int port { get; set; }
        [Option('s', "source", Required = false, Default = "")]
        public string? source { get; set; }
    }
}
