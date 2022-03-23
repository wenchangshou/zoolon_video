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
                      ApplicationConfiguration.Initialize();
                      Application.Run(new Form1(o));
                  })
                      .WithNotParsed(HandleParseError); 

        }
        static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine(errs.ToString());
            //handle errors
        }
    }
}