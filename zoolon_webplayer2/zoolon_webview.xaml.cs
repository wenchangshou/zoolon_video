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

namespace zoolon_webplayer2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class zoolon_webView : Window
    {
        private controlImpl control;
        private Server server;
        public zoolon_webView()
        {
            InitializeComponent();
       
        }

        public zoolon_webView(Options obj)
        {
            InitializeComponent();
            Application.Current.MainWindow.Width = obj.width;
            Application.Current.MainWindow.Height = obj.height;
            Application.Current.MainWindow.Left = obj.x;
            Application.Current.MainWindow.Top = obj.y;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
            this.Source = obj.source;
            initControl(obj);
        }

        private void initControl(Options options)
        {
            if (options.port == 0)
            {
                return;
            }
            control = new controlImpl();
            control.execute += Control_execute;
            control.get += getHandler;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(control) },
                Ports = { new ServerPort("localhost", options.port, ServerCredentials.Insecure) }
            };
            server.Start();
        }

        private getResult getHandler()
        {
            Dictionary<string, Object> m = new Dictionary<string, object>();
            getResult result = new getResult();

            m["source"] = this.source;
       
            result.Code = 0;
            result.Payload = JsonConvert.SerializeObject(m);
            return result;
        }

        private ExecuteResult Control_execute(string payload)
        {
             Dictionary<string, object> cmd;
            Dictionary<string, object> arguments=new Dictionary<string, object>();
            try
            {
                cmd= JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                if (cmd == null)
                {
                    return new ExecuteResult(400,"payload 解析数据为空");
                }

            }
            catch (System.Exception ex)
            {
                return new ExecuteResult(400, "解析json失败:"+ex.Message);
            }
            string? action = "";
            if ((!cmd.ContainsKey("action")) && (!cmd.ContainsKey("Action")))
            {
                return new ExecuteResult(400, "action 必须填写");
            }
            action = cmd.ContainsKey("action") ? cmd["action"].ToString() : cmd["Action"].ToString();
            if (cmd.ContainsKey("arguments"))
            {
        
                arguments= JsonConvert.DeserializeObject<Dictionary<string, object>>(cmd["arguments"].ToString());
            }
            int code = 0;
            string msg = "success";
            return new ExecuteResult(code, msg);
        }
        private string source;
        public string Source {
            get {
                return source;
            }
            private set
            {
                source = value;
                webview.Source = new Uri(value);
                
            }
        }
    }
}
