using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Runtime.InteropServices;

namespace zoolon_pptPlayer
{

    public partial class Form1 : Form
    {
        public ppt.Presentation ObjPrs { get; private set; }

        public ppt.SlideShowView OSlideShowView { get; private set; }

        public ppt.Application ObjApp { get; private set; }
        private int x;
        private int y;
        private int width;
        private int height;
        private string _source;
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr childIntPtr, IntPtr parentIntPtr);
        [DllImport("kernel32.dll")]
        static extern uint GetLastError();
        public Form1()
        {
            InitializeComponent();

            //防止连续打开多个PPT程序.
            if (ObjApp != null) { return; }
            ObjApp = new ppt.Application();
        }
        public ppt.Presentation OpenPpt(string url)
        {
            var objPresSet = ObjApp.Presentations;
            var objPrs = objPresSet.Open(url, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
            return objPrs;
        }
        public void setForm(int x, int y, int width, int height)
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

        internal void pre()
        {
            OSlideShowView.Previous();
        }

        public void SetWindowInfo(int x, int y, int width, int height)
        {

            setForm(x, y, width, height);
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
                Console.WriteLine("yyy");
            }
            catch (Exception e)
            {
            }

        }
        public void LoadSource(string source)
        {
            var te = OpenPpt(source);
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

        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void next()
        {
            OSlideShowView.Next();
        }
        public void first()
        {
            OSlideShowView.First();
        }
        public void last()
        {
            OSlideShowView.Last();
        }
        public void goPage(int page)
        {
            OSlideShowView.GotoSlide(page);
        }



        private void button2_Click(object sender, EventArgs e)
        {
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
    class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private IntPtr _hwnd;
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get
            {
                return _hwnd;
            }
        }
    }
}
