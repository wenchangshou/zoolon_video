using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace zoolon_container.player
{
    class WebCmdArgumentStruct
    {
        public string? Script { get; set; }
    }
    class WebCmdStruct
    {
        public string ?Action { get; set; }
        public string? Script { get; set; }
        public WebCmdArgumentStruct? Arguments { get; set; }
        
    }
    internal class WebPlayer : iplayer
    {
        customWebPlayer _container;
        private string _source;
        
        public WebPlayer(string source)
        {
            _container = new customWebPlayer();
            _source = source;
            this.Open(_source);
        }
        public bool Close()
        {
            _container.Dispose();
            return true;
        }
        public async Task<string> call(WebCmdStruct cmd)
        {
            string js="";
            if (cmd.Script != "")
            {
                js = cmd.Script;
            }
            if (cmd.Arguments != null && cmd.Arguments.Script != "")
            {
                js=cmd.Arguments.Script;
            }
            var result =await _container.ExecuteJs(js);
            return result;
        }
        public replyMessage Control(string body)
        {
            replyMessage reply=new replyMessage();
            try
            {
                WebCmdStruct _cmd = JsonConvert.DeserializeObject<WebCmdStruct>(body);
                if (_cmd != null)
                {
                    if(_cmd.Action== "call interface")
                    {
                         call(_cmd);
                    }
                }
                return reply;
            }
            catch (System.Exception ex)
            {
                reply.reply = true;
                
            }
            return reply;

        }

        public replyMessage Control(Hashtable args)
        {
            
            throw new NotImplementedException();

        }

        public bool Exit()
        {
            throw new NotImplementedException();
        }

        public ContentControl GetComponents()
        {
            return _container;
        }

        public PlayerType getType()
        {
            return PlayerType.Web;
        }

        public bool Open(string sourceDir)
        {
            _container.Source = sourceDir;
            return true;
        }
    }
}
