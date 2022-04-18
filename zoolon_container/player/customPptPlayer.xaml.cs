using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ppt = Microsoft.Office.Interop.PowerPoint;
using System.Windows.Forms;
namespace zoolon_container.player
{
    /// <summary>
    /// customPptPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class customPptPlayer : System.Windows.Controls.UserControl
    {
        private const int WM_SYSCOMMAND = 274;
        private const int SC_MAXIMIZE = 61488;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr IntPtrGetDesktopWindow();

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
                IntPtr hWnd,        // 信息发往的窗口的句柄
                int Msg,            // 消息ID
                int wParam,         // 参数1
                int lParam          //参数2
            );
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        // Delegate to filter which windows to include 
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        public ppt.Presentation ObjPrs { get; private set; }

        public ppt.SlideShowView OSlideShowView { get; private set; }
        public ppt.Application ObjApp { get; private set; }
        SynchronizationContext _syncContext = null;
       
        private string _source;
        WindowsFormsHost _host;
        System.Windows.Forms.Panel _panel;
        public customPptPlayer()
        {
            InitializeComponent();
        }
        public customPptPlayer(string source)
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            _panel = new System.Windows.Forms.Panel();

            _host = new WindowsFormsHost();
            _host.Child = _panel;
            Content = _host;
            //防止连续打开多个PPT程序.
            if (ObjApp != null) { return; }
            ObjApp = new ppt.Application();
            _source = source;

            Loaded += OnLoaded;
           
        }
         ~customPptPlayer()
        {
            Console.WriteLine("~custom ppt player");
        }
        private void _panel_Resize(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var te = OpenPpt(_source.ToString());


            PlayPpt(te);
        }
        private void wbWinForms_DocumentTitleChanged(object sender, EventArgs e)
        {
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

            //IntPtr hwnd = ((HwndSource)PresentationSource.FromVisual(dockPanel)).Handle;


            //var parentHwnd = new WindowInteropHelper(hwnd).Handle;

            try
            {


   
                SetParent(wn, _panel.Handle);
                System.Threading.Thread.Sleep(1000);//加上，100如果效果没有就继续加大

                SendMessage(wn, WM_SYSCOMMAND, SC_MAXIMIZE, 0);

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
            }

        }
        public void Close()
        {
            ObjPrs.Close();

        }
        public  void Change(string source)
        {
            _source = source;
            var te = OpenPpt(source);
            PlayPpt(te);
        }
        public ppt.Presentation OpenPpt(string url)
        {
            var objPresSet = ObjApp.Presentations;
            var objPrs = objPresSet.Open(url, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
            return objPrs;
        }
    }
}
