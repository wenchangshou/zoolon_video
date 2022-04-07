using CommandLine;
namespace Zoolon_videoplayer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                  .WithParsed<Options>(o =>

                  {
                      Application.ThreadException += Application_ThreadException;
                      AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                      Application.EnableVisualStyles();
                      ApplicationConfiguration.Initialize();
                      Application.Run(new zoolon_videoPlayer(o));
                  })
                      .WithNotParsed(HandleParseError); 

        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception? ex = e.ExceptionObject as Exception;
            MessageBox.Show(string.Format("捕获到未处理异常：{0}\n异常信息：{1}\n异常堆栈：{2}\r\nCLR即将退出：{3}", ex?.GetType(), ex?.Message, ex?.StackTrace, e.IsTerminating));
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