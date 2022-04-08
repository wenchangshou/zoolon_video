using CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace zoolon_container
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
            string str = errs.ToString();
            Console.WriteLine(str);
        }

        private void Run(Options obj)
        {
            try
            {
                MainWindow mainWindow = new MainWindow(obj);

                mainWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("捕获到未处理的异常：{0}\n异常信息：{1}\n异常堆栈：{2}", ex.GetType(), ex.Message, ex.StackTrace));
            }
        }
    }
}
