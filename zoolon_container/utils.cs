using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoolon_container
{
    public class utils
    {
        [DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps", SetLastError = true)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        enum DeviceCap
        {
            VERTRES = 10,
            PHYSICALWIDTH = 110,
            SCALINGFACTORX = 114,
            DESKTOPVERTRES = 117,

            // http://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
        }
        public static String GetFileSuffix(string name)
        {
            return System.IO.Path.GetExtension(name);
        }

        internal static PlayerType GetPlayerType(string source)
        {
            if (source.StartsWith("http")||source.StartsWith("www"))
            {
                return PlayerType.Web;
            }
            string suffix=GetFileSuffix(source).ToLower();
            if (suffix == ".mp4")
            {
                return PlayerType.Video;
            }
            if (suffix == ".jpg")
            {
                return PlayerType.Image;
            }
            if (suffix == ".ppt" || suffix == ".pptx")
            {
                return PlayerType.PPT;
            }
            return PlayerType.Unknown;

        }

        public static double GetScreenScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            var physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            var screenScalingFactor =
                (double)physicalScreenHeight / Screen.PrimaryScreen.Bounds.Height;
            //SystemParameters.PrimaryScreenHeight;

            return screenScalingFactor;
        }


    }
}
