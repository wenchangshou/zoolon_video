using Base;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace zoolon_image
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private controlImpl control;
        private int idx;
        private string[] _source;
        private Server server;

        public MainWindow(Options option)
        {

            InitializeComponent();
            this.Source = option.Source;
            Application.Current.MainWindow.Width = option.Width;
            Application.Current.MainWindow.Height = option.Height;
            Application.Current.MainWindow.Left = option.X;
            Application.Current.MainWindow.Top = option.Y;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            initControl(option);
        }
        private void initControl(Options options)
        {
            if (options.Port == 0)
            {
                return;
            }
            control = new controlImpl();
            control.Execute += Control_execute;
            control.Get += getHandler;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(control) },
                Ports = { new ServerPort("localhost", options.Port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        private getResult getHandler()
        {
            throw new NotImplementedException();
        }

        private ExecuteResult Control_execute(string payload)
        {
            ExecuteResult result = new ExecuteResult(0, "success");
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
                first();
            }else if (action == "last")
            {
                last();
            }else if (action == "next")
            {
                next();

            }else if (action == "change")
            {
                if (!arguments.ContainsKey("action"))
                {
                    return new ExecuteResult(400, "action change source not exists");
                }
                _source = arguments["source"].ToString().Split(",");
                this.first();
            }
            return result;
        }
        private  void first()
        {
            idx = 0;
            load();
        }
        private void last()
        {
            idx = _source.Length - 1;
            load();
        }
        private void next()
        {
            idx++;
            if (idx>= _source.Length)
            {
                idx = 0;
            }
            load();
        }
        private void prev()
        {
            idx--;
            if (idx <= 0)
            {
                idx=_source.Length - 1;
            }
            load();
        }
        public string Source
        {

            set
            {
                var arr = value.Split(",");
                _source = arr;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void load()
        {
            image.Source = new BitmapImage(new Uri(_source[idx]));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_source.Length == 0)
            {
                return;
            }
            idx = 0;
            load();

        }
    }
}
