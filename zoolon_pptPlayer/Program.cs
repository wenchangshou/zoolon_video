using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zoolon_pptPlayer
{
    static class Program
    {
      
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //    Application.EnableVisualStyles();
            //     Application.SetCompatibleTextRenderingDefault(false);
            Parser.Default.ParseArguments<Options>(args).WithParsed(Run).WithNotParsed(HandleParseError);

        }

        private static void Run(Options obj)
        {
            try
            {
                Form1 form1;
                form1 = new Form1();
                form1.SetSource(obj.source);
                form1.SetWindowInfo(obj.x, obj.y, obj.width, obj.height);
                Application.ThreadException += Application_ThreadException;
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Application.EnableVisualStyles();
                //   Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(form1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("捕获到未处理的异常：{0}\n异常信息：{1}\n异常堆栈：{2}", ex.GetType(), ex.Message, ex.StackTrace));

            }

        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            MessageBox.Show(string.Format("捕获到未处理异常：{0}\n异常信息：{1}\n异常堆栈：{2}\r\nCLR即将退出：{3}", ex.GetType(), ex.Message, ex.StackTrace, e.IsTerminating));
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            MessageBox.Show(string.Format("捕获到未处理异常：{0}\n异常信息：{1}\n异常堆栈：{2}", ex.GetType(), ex.Message, ex.StackTrace));
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine(errs.ToString());
        }
    }
}
