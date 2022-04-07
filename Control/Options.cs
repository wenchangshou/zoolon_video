using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

public class Options
{
    [Option( "protocol", Required =false,Default ="rpc")]
    public string Protocol { get; set; }
    [Option('x',  Required = false, Default = 0, HelpText = "set winform x position")]
    public int X { get; set; }
    [Option('y', Required = false, Default = 0, HelpText = "set winform y position")]
    public int Y { get; set; }
    [Option('w', "width", Required = false, Default = 1920, HelpText = "set winform width")]
    public int Width { get; set; }
    [Option('h', "height", Required = false, Default = 1080)]
    public int Height { get; set; }
    [Option('p', "port", Required = false, Default = 0)]
    public int Port { get; set; }
    [Option('s', "source", Required = false, Default = "d:/1.mp4")]
    public string Source { get; set; }
}

