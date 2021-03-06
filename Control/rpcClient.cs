using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Control;
using Grpc.Core;

namespace Base
{
    public class grpcImpl : RpcCall.RpcCallBase
    {
        public grpcImpl()
        {
        }
        public delegate ExecuteResult executeHandler(string payload);
        public event executeHandler execute;
        public delegate getResult getHandler();
        public event getHandler get;
        public override Task<RpcResponse> Call(RpcRequest request, ServerCallContext context)
        {
            string body = request.Body;

            ExecuteResult payload = new ExecuteResult();
            if (execute != null)
            {
                payload = execute(body);
            }

            return Task.FromResult(new RpcResponse { Code = payload.Code, Msg = payload.Msg });
        }

        public override Task<SimpleResponse> Close(EmptyMessage request, ServerCallContext context)
        {
            return base.Close(request, context);
        }

    }
    internal class rpcClient : Icontrol
    {
        private Server server;
        private grpcImpl contrl;
   
        public rpcClient(string address,int port)
        {
            contrl = new grpcImpl();
            contrl.execute += Control_execute;
            contrl.get += getHandler;
            server = new Server
            {
                Services = { Control.RpcCall.BindService(contrl) },
                Ports = {new ServerPort(address,port,ServerCredentials.Insecure)}
            };
            server.Start();

        }

        private getResult getHandler()
        {
            throw new NotImplementedException();
        }

        private ExecuteResult Control_execute(string payload)
        {
            if(OnRecvMsg != null)
            {
                return OnRecvMsg(payload);
            }
            return new ExecuteResult();
        }

        public event Icontrol.RecvMsg OnRecvMsg;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public string Send(string body)
        {
            
            throw new NotImplementedException();
        }

        public byte[] Send(byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}
