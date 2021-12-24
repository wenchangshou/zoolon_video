using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Control;
using Grpc.Core;
using LibVLCSharp.Shared;
namespace Zoolon_videoplayer
{
    public class controlImpl : RpcCall.RpcCallBase
    {
        public controlImpl(MediaPlayer mediaPlayer)
        {

        }

        public override Task<RpcResponse> Call(RpcRequest request, ServerCallContext context)
        {
            string body = request.Body;

            return base.Call(request, context);
        }

        public override Task<SimpleResponse> Close(EmptyMessage request, ServerCallContext context)
        {
            return base.Close(request, context);
        }
    }
}
