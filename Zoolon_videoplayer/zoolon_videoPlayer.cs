using Grpc.Core;
using LibVLCSharp.Shared;
using Control;
using Newtonsoft.Json;
using Base;

namespace Zoolon_videoplayer
{
    public partial class zoolon_videoPlayer : Form
    {
        public LibVLC? _libVLC;
        public MediaPlayer _mp;
        public Server? server;
        private int x=0;
        private int y=0;
        private int width=1920;
        private int height=1080;
        private string source="";
        private controlImpl control;
        public void InitVlc()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }
            _libVLC = new LibVLC("--input-repeat=65535");
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;
        }
        public zoolon_videoPlayer()
        {
    
            InitializeComponent();
            if (!DesignMode)
            {
                Core.Initialize();
            }
            _libVLC = new LibVLC("--input-repeat=65535");
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;
        }
        public zoolon_videoPlayer(Options options)
        {
            InitializeComponent();

            InitControl(options);
            if (!DesignMode)
            {
                Core.Initialize();
            }
            _libVLC = new LibVLC("--input-repeat=65535");
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;

            this.source = options.source;
         //   this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(options.x, options.y);
            this.Size = new Size(options.width, options.height);
           // this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            Load += Form1_Load;
            FormClosed += Form1_FormClosed;

        }
        private void InitControl(Options options)
        {
            if (options.port == 0)
            {
                return;
            }
            control = new controlImpl();
            control.Execute += Control_execute;
            control.Get += GetHandler;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(control) },
                Ports = { new ServerPort("localhost", options.port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        private getResult GetHandler()
        {
            Dictionary<string, Object> m = new Dictionary<string, object>();
            getResult result = new getResult();
            if (_mp == null){
                return result;
            }
            var state = _mp.State;
            m["position"] =state==VLCState.Ended?100: _mp.Position*100;
            m["state"] = state.ToString();
            m["volume"] = _mp.Volume;
            m["duration"] = _mp.Time;
            m["time"] = _mp.Length;
            result.Code = 0;
            result.Payload = JsonConvert.SerializeObject(m);
            return result;
        }

        private ExecuteResult Control_execute(string payload)
        {
            Dictionary<string, object> cmd;
            Dictionary<string, object> arguments=new Dictionary<string, object>();
            try
            {
                cmd= JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                if (cmd == null)
                {
                    return new ExecuteResult(400,"payload 解析数据为空");
                }

            }
            catch (System.Exception ex)
            {
                return new ExecuteResult(400, "解析json失败:"+ex.Message);
            }
            string? action = "";
            if ((!cmd.ContainsKey("action")) && (!cmd.ContainsKey("Action")))
            {
                return new ExecuteResult(400, "action 必须填写");
            }
            action = cmd.ContainsKey("action") ? cmd["action"].ToString() : cmd["Action"].ToString();
            if (cmd.ContainsKey("arguments"))
            {
        
                arguments= JsonConvert.DeserializeObject<Dictionary<string, object>>(cmd["arguments"].ToString());
            }
            int code = 0;
            string msg = "success";
           
            if (action == "play")
            {
                _mp.Play();
            }
            else if (action == "stop")
            {
                _mp.Stop();
            }
            else if (action == "pause")
            {
                _mp.Pause();
            }
            else if (action == "setVolume")
            {
                if (!arguments.ContainsKey("volume"))
                {
                    return new ExecuteResult(400, "action setVolume volume not exists");
                }
                _mp.Volume = Int32.Parse(arguments["volume"].ToString());
            }else if(action== "setPosition")
            {
                if (!arguments.ContainsKey("position"))
                {
                    return new ExecuteResult(400, "action setVolume position not exists");
                }
                _mp.Position=Int32.Parse(arguments["position"].ToString());
            }else if (action == "change")
            {
                if (!arguments.ContainsKey("source"))
                {
                    return new ExecuteResult(400, "action change source not exists");
                }
                source = arguments["source"].ToString();
                var media = new Media(_libVLC, new Uri(source));
                _mp.Play(media);
            }
            return new ExecuteResult(code, msg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var media = new Media(_libVLC, new Uri(this.source));
            _mp.Play(media);
            _mp.Volume = 0;
            media.Dispose();
    
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (server != null)
            {
                server.ShutdownAsync().Wait();
            }
            _mp.Stop();
            _mp.Dispose();
            _libVLC.Dispose();
        }
    }
}