using Grpc.Core;
using LibVLCSharp.Shared;
using Control;
using Newtonsoft.Json;

namespace Zoolon_videoplayer
{
    public partial class Form1 : Form
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;
        public Server server;
        private int x;
        private int y;
        private int width;
        private int height;
        private string source;
        private controlImpl control;
        public void initVlc()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }
            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);
            videoView1.MediaPlayer = _mp;
        }
        public Form1()
        {
            InitializeComponent();
            initVlc();
        }
        public Form1(Options options)
        {
            InitializeComponent();

            initControl(options);
            initVlc();

            this.source = options.source;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(options.x, options.y);
            this.Size = new Size(options.width, options.height);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            Load += Form1_Load;
            FormClosed += Form1_FormClosed;

        }
        private void initControl(Options options)
        {
            if (options.port == 0)
            {
                return;
            }
            control = new controlImpl(_mp);
            control.execute += Control_execute;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(control) },
                Ports = { new ServerPort("localhost", options.port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        private string Control_execute(string payload)
        {
            Dictionary<string, string> cmd = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);
            string action = cmd["action"];

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
            return "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var media = new Media(_libVLC, new Uri(this.source));
            _mp.Play(media);
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