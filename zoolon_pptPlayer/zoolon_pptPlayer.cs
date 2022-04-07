using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Grpc.Core;
using ppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Base;

namespace zoolon_pptPlayer
{

    public partial class zoolon_pptPlayer : Form
    {
        public ppt.Presentation ObjPrs { get; private set; }

        public ppt.SlideShowView OSlideShowView { get; private set; }

        public ppt.Application ObjApp { get; private set; }
        private int x;
        private int y;
        private int width;
        private int height;
        private string _source;
        private controlImpl control;
        public Server server;
        SynchronizationContext _syncContext = null;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr childIntPtr, IntPtr parentIntPtr);
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        public zoolon_pptPlayer(Options options)
        {
            InitializeComponent();
            initControl(options);
            _syncContext = SynchronizationContext.Current;

            //防止连续打开多个PPT程序.
            if (ObjApp != null) { return; }
            ObjApp = new ppt.Application();
        }

        private void initControl(Options options)
        {
            if (options.Port == 0)
            {
                return;
            }
            control = new controlImpl();
            control.execute += Control_execute;
            control.get += getHandler;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(control) },
                Ports = { new ServerPort("localhost", options.Port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        private getResult getHandler()
        {
            getResult result=new getResult();
            Dictionary<string,object> payload=new Dictionary<string,object>();
            if (OSlideShowView == null)
            {
                return result;
            }
            try
            {
                payload["state"] = OSlideShowView.State;
                payload["page"] = OSlideShowView.Slide.SlideIndex;
                result.Payload = JsonConvert.SerializeObject(payload);
            }
            catch (Exception)
            {

            }
           

            return result;
        }

        private ExecuteResult Control_execute(string payload)
        {
            ExecuteResult result=new ExecuteResult(0,"success");
            Dictionary<string, object> cmd;
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            try
            {
                cmd = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                if (cmd == null)
                {
                    return new ExecuteResult(400, "payload 解析数据为空");
                }
            }
            catch (System.Exception ex)
            {
                return new ExecuteResult(400, "解析json失败:" + ex.Message);
            }
            string? action = "";
            if ((!cmd.ContainsKey("action")) && (!cmd.ContainsKey("Action")))
            {
                return new ExecuteResult(400, "action 必须填写");
            }
            action = cmd.ContainsKey("action") ? cmd["action"].ToString() : cmd["Action"].ToString();
            if (cmd.ContainsKey("arguments"))
            {
                arguments = JsonConvert.DeserializeObject<Dictionary<string, object>>(cmd["arguments"].ToString());
            }
            if (action == "first")
            {
                OSlideShowView.First();
            }else if (action == "last")
            {
                OSlideShowView.Last();
            }else if (action == "pre")
            {
                OSlideShowView.Previous();
            }else if(action == "next")
            {
                OSlideShowView.Next();
            }else if (action == "goPage")
            {
                if (arguments == null)
                {
                    result.Code = 400;
                    result.Msg = "goPage的参数不能为空";
                }
                if (arguments.ContainsKey("page"))
                {
                    int page=Int32.Parse(arguments["page"].ToString());
                    OSlideShowView.GotoSlide(page);
                }
            }else if (action == "change")
            {
                if (!arguments.ContainsKey("source"))
                {
                    return new ExecuteResult(400, "action change source not exists");
                }
                _source = arguments["source"].ToString();
                _syncContext.Post(LoadSource, _source);
              
            }
            return result;
        }

        public ppt.Presentation OpenPpt(string url)
        {
            var objPresSet = ObjApp.Presentations;
            var objPrs = objPresSet.Open(url, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
            return objPrs;
        }
        public void SetForm(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;

            this.Size = new Size(width, height);
            this.Location = new Point(x, y);
            this.ShowInTaskbar = false;
        }

    

        public void SetWindowInfo(int x, int y, int width, int height)
        {

            SetForm(x, y, width, height);
        }

        /// <summary>
        /// 播放ppt
        /// </summary>
        /// <param name="objPrs"></param>
        public void PlayPpt(ppt.Presentation objPrs)
        {
            ObjPrs = objPrs;
            //进入播放模式
            var objSlides = objPrs.Slides;
            var objSss = objPrs.SlideShowSettings;
            objSss.LoopUntilStopped = MsoTriState.msoCTrue;
            objSss.StartingSlide = 1;
            objSss.EndingSlide = objSlides.Count;
            objSss.ShowType = ppt.PpSlideShowType.ppShowTypeKiosk;
            objSss.ShowPresenterView = MsoTriState.msoCTrue;
            var sw = objSss.Run();

            OSlideShowView = objPrs.SlideShowWindow.View;
            var wn = (IntPtr)sw.HWND;

            //嵌入窗体
            if (panel1 == null)
            {
                return;
            }
            var parentHwnd = panel1.Handle;
            try
            {
                SetParent(wn, parentHwnd);
            }
            catch (Exception e)
            {
            }

        }
        public void LoadSource(object source)
        {
            var te = OpenPpt(source.ToString());
            PlayPpt(te);
        }
        public void SetSource(string source)
        {
            this._source = source;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Width = this.width;
            panel1.Height = this.height;

            LoadSource(this._source);

        }

     
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                ObjPrs.Close();
                ObjApp.Quit();
            }
            catch { }
        }
    }
    
}
