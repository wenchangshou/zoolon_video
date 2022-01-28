using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommandLine;
namespace zoolon_webplayer2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            Parser.Default.ParseArguments<Options>(e.Args).WithParsed(Run)
                .WithNotParsed(HandleParseError);
        }
        private void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine(errs.ToString());

        }

        private void Run(Options obj)
        {
            try
            {
                zoolon_webView mainWindow = new zoolon_webView(obj);

                mainWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("捕获到未处理的异常：{0}\n异常信息：{1}\n异常堆栈：{2}", ex.GetType(), ex.Message, ex.StackTrace));
            }
        }
    }
}
