using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace zoolon_container.player
{
    /// <summary>
    /// customWebPlayer.xaml 的交互逻辑
    /// </summary>
    public partial class customWebPlayer : UserControl
    {
        private WebView2 _webView;
        private delegate Task<string> executeJsDelegate(string js);

        public customWebPlayer()
        {
            InitializeComponent();
            _webView = new WebView2
            {
                CreationProperties = new CoreWebView2CreationProperties
                {
                }
            };
            Content = _webView;
        }
        public async Task<string> Call(string js)
        {
            var result=await _webView.ExecuteScriptAsync(js);
            return result;   
        }
        [BindableAttribute(true)]
        public string Source
        {
            get { return (string)GetValue(TitlesProperty); }
            set { SetValue(TitlesProperty, value); this._webView.Source = new Uri(value); }
        }
        public async Task<string>  executeJs(string js)
        {
            return await _webView.ExecuteScriptAsync(js); ;
        }
        public async Task<string> ExecuteJs(string js)
        {
            string result="";
            try
            {
                 this._webView.Dispatcher.Invoke(new executeJsDelegate(executeJs), js);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("err:" + ex.ToString());
            }
            return result;
        }

        public void Dispose()
        {
            _webView?.Dispose();
        }
        public static readonly DependencyProperty TitlesProperty =
    DependencyProperty.Register("Source", typeof(string), typeof(customImage), new PropertyMetadata("custom http source"));
    }
}
