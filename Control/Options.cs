using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;

public class Options
{
    [Option( "protocol", Required =false,Default ="grpc")]
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
    [Option("register_websocket", Required = false, Default = false)]
    public bool RegisterWebsocket { get; set; }
    [Option("websocket_ip",Required =false,Default ="127.0.0.1")]
    public string WebsocketIP { get; set; }
    [Option("websocket_instance_name",Required =false,Default ="app")]
    public string WebsocketInstanceName { get; set; }

    [Option("websocket_port",Required =false,Default =5678)]
    public int WebsocketPort { get; set; }
}

