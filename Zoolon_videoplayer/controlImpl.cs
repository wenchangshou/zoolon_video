using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Control;
using Grpc.Core;
using LibVLCSharp.Shared;
using Newtonsoft.Json;
namespace Zoolon_videoplayer
{
    public class ControlStruct
    {
        public string Action { get; set; }
    }
    public class controlImpl : RpcCall.RpcCallBase
    {
        private MediaPlayer player;
        public controlImpl(MediaPlayer mediaPlayer)
        {
            player = mediaPlayer;
        }
        public delegate string executeHandler(string payload);
        public event executeHandler execute;
        public override Task<RpcResponse> Call(RpcRequest request, ServerCallContext context)
        {
            string body = request.Body;
            int code = 0;
            string msg = "success";
            string payload = "";
            if (execute != null)
            {
                execute(body);
            }
          /* Dictionary<string, string> cmd = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
            string action = cmd["action"];
          
            if (action == "play")
            {
                player.Play();
            }else if (action == "stop")
            {
                player.Stop();
            }else if (action == "pause")
            {
                player.Pause();
            }*/


            return Task.FromResult(new RpcResponse { Code = code, Msg = msg, Payload = payload }) ;
        }

        public override Task<SimpleResponse> Close(EmptyMessage request, ServerCallContext context)
        {
            return base.Close(request, context);
        }

        /**
         * 
         */
        public override Task<SimpleResponse> Ping(EmptyMessage request, ServerCallContext context)
        {
            return base.Ping(request, context);
        }

    
    }
}
