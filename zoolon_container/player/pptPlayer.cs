using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ppt = Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.Threading;

namespace zoolon_container.player
{
    class pptPlayer : iplayer
    {
        System.Windows.Forms.Panel panel1;
        public ppt.Presentation ObjPrs { get; private set; }

        public ppt.SlideShowView OSlideShowView { get; private set; }

        public ppt.Application ObjApp { get; private set; }
        SynchronizationContext _syncContext = null;

        public pptPlayer(string source)
        {
            _syncContext = SynchronizationContext.Current;
            //防止连续打开多个PPT程序.
            if (ObjApp != null) { return; }
            ObjApp = new ppt.Application();
            var te = OpenPpt(source.ToString());
            
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

            }
            catch (Exception e)
            {
            }

        }
        public ppt.Presentation OpenPpt(string url)
        {
            var objPresSet = ObjApp.Presentations;
            var objPrs = objPresSet.Open(url, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
            return objPrs;
        }
        public bool Close()
        {
            throw new NotImplementedException();
        }

        public replyMessage Control(string body)
        {
            throw new NotImplementedException();
        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public ContentControl GetComponents()
        {
            StackPanel panel = new StackPanel();
        }

        public bool Open(string sourceDir)
        {
            throw new NotImplementedException();
        }
    }
}
