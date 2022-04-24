using Base;
using LibVLCSharp.Shared;
using LibVLCSharp.WPF;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using zoolon_container.player;

namespace zoolon_container
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        PlayerType _playerType = PlayerType.Unknown;
        iplayer c;
        double scalingRatio = 1.0;
        Options _options;
        MessageProcess _process;
        public MainWindow()
        {
            InitializeComponent();

        }
        public MainWindow(Options options)
        {
            InitializeComponent();
            _options = options;
            scalingRatio = ScreenHelper.GetScalingRatio();
            Application.Current.MainWindow.Width = options.Width / scalingRatio;
            Application.Current.MainWindow.Height = options.Height / scalingRatio;
            Application.Current.MainWindow.Left = options.X / scalingRatio;
            Application.Current.MainWindow.Top = options.Y / scalingRatio;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            canvas1.Width = options.Width / scalingRatio;
            canvas1.Height = options.Height / scalingRatio;
            Open(_options.Source);
           // _process = new MessageProcess(options, c);
            InitControl(options);
        }

        private void onLoaded(object sender, RoutedEventArgs e)
        {
            Open(_options.Source);
        }

        private void InitControl(Options option)
        {
            Icontrol? control =null ;
            if (option.Protocol == "daemon" && option.RegisterWebsocket)
            {
                control=ControlFactory.Make(ProtocolType.Daemon, option);
            }
            if (control != null)
            {
                control.OnRecvMsg += Control_OnRecvMsg;
            }
        }

        private ExecuteResult Control_OnRecvMsg(string body)
        {
            var result = new ExecuteResult();
            result.Reply = true;
            result.Msg = "成功";
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
            if (deserialized.ContainsKey("Service") && deserialized["Service"].ToString() == "registerCall")
            {
                return result;
            }
            if ((!deserialized.ContainsKey("action")) && !(!deserialized.ContainsKey("Action")))
            {
                result.Msg = "Action 必须填写";
                return result;
            }
            string action = deserialized["action"].ToString();
            if (action == "open")
            {
                string _source = deserialized["source"].ToString();
                this.Dispatcher.Invoke(new Action(delegate
                {
                    Open(_source);
                }));
                return result;
            }

            ((iplayer)c).Control(body);

            Console.WriteLine($"接收到消息:{body}");

            return result;
        }

      

        private void Open(string source)
        {
            PlayerType pType = utils.GetPlayerType(source);

            if (c != null && c.getType() == pType)
            {
                c.Open(source);
                return;
            }
            c?.Close();
            canvas1.Children.Clear();
            _playerType = pType;
            c = factory.CreatePlayer(pType, source, this.canvas1.Width, this.canvas1.Height);
            canvas1.Children.Add(c.GetComponents());
        }
    
        public void Dispose()
        {
            if (c != null)
            {
                c.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (c != null)
            {
                c.Close();
            }
            System.Environment.Exit(0);
        }

        //切换视频

    }
}
