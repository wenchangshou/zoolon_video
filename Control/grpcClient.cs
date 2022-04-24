using Base;
using Control;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common
{
    internal class GrpcClient :RpcCall.RpcCallBase, Icontrol
    {
        private Server _server;
        public delegate ExecuteResult ExecuteHandler(string payload);
        public event ExecuteHandler Execute;
        public delegate getResult GetHandler();
        public event GetHandler Get;
        public GrpcClient()
        {

        }
        public GrpcClient(string address,int port)
        {
            _server = new Server
            {
                Services = { Control.RpcCall.BindService(this) },
                Ports = { new ServerPort(address, port, ServerCredentials.Insecure) }
            };
            _server.Start();
        }
        
        public event Icontrol.RecvMsg OnRecvMsg;

        public void Close()
        {
            this._server.ShutdownAsync();
        }

        public string Send(string body)
        {
            throw new NotImplementedException();
        }

        public byte[] Send(byte[] body)
        {
            throw new NotImplementedException();
        }

        public override Task<RpcResponse> Call(RpcRequest request, ServerCallContext context)
        {
            string body = request.Body;
            ExecuteResult payload = new();
            
            if (OnRecvMsg != null)
            {
                var reply = OnRecvMsg(body);
                if (reply != null && reply.Reply && reply.Msg != null)
                {
                    payload.Msg = reply.Msg;
                    payload.Code = reply.Code;
                }
            }
            return Task.FromResult(new RpcResponse { Code = payload.Code, Msg = payload.Msg });

        }
    }
}
