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

namespace zoolon_container
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        PlayerType _playerType=PlayerType.Unknown;
        ContentControl c;
        ImagePlayer imagePlayer;
        double scalingRatio=1.0;
        DaemonClient? client ;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(Options options)
        {
            InitializeComponent();
            scalingRatio = ScreenHelper.GetScalingRatio();
            Application.Current.MainWindow.Width = options.Width/ scalingRatio;
            Application.Current.MainWindow.Height = options.Height/scalingRatio;
            Application.Current.MainWindow.Left = options.X / scalingRatio;
            Application.Current.MainWindow.Top = options.Y / scalingRatio;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            canvas1.Width = options.Width / scalingRatio;
            canvas1.Height = options.Height / scalingRatio;
            Open(options.Source);
            InitControl(options);
        }
        private void InitControl(Options option)
        {
            string uri = $"ws://{option.WebsocketIP}:{option.WebsocketPort}";
             client = new DaemonClient(uri,option.WebsocketInstanceName);
            client.Start();
            client.OnRecvMsg += Client_OnRecvMsg;
        }

        private ExecuteResult Client_OnRecvMsg(string body)
        {
            var result = new ExecuteResult();
            result.Reply = true;
            result.Msg = "测试消息";
            var deserialized = JsonConvert.DeserializeObject<Dictionary<string,object>>(body);
            if(deserialized.ContainsKey("Service")&&deserialized["Service"].ToString()== "registerCall")
            {
                return result;
            }
            if (!deserialized.ContainsKey("action"))
            {
                result.Msg = "Action 必须填写";
                return result;
            }
            string action=deserialized["action"].ToString();
            if (action == "open")
            {
                string _source = deserialized["source"].ToString();
                this.Dispatcher.Invoke(new Action(delegate
                {
                    Open(_source);

                }));


            }
            Console.WriteLine($"接收到消息:{body}");
            
            return result;
        }

        private void Open(string source)
        {
            PlayerType pType = utils.GetPlayerType(source);
            if (_playerType == pType&& pType != PlayerType.Image)
            {
                ((iplayer)c).Open(source);
            }
            if (c != null && pType == PlayerType.Video)
            {
                ((iplayer)c).Close();
            }
            _playerType = pType;

            canvas1.Children.Clear();

            if (pType == PlayerType.Video){
                c = new VideoPlayer(source);
              
            }
            if (pType == PlayerType.Image)
            {
                imagePlayer = new ImagePlayer(source);
                imagePlayer.Width = this.Width;
                imagePlayer.Height = this.Height;
                canvas1.Children.Add(imagePlayer);
                return;
            }
            c.Width = this.Width;
            c.Height = this.Height;
            canvas1.Children.Add(c);
        }
        public void initControl(Options options)
        {

        }


        //切换视频
   
    }
}
