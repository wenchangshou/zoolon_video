using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base;
using Control;
using Grpc.Core;

public class ControlStruct
{
    public string Action { get; set; }
}
public class controlImpl : RpcCall.RpcCallBase
{
    public controlImpl()
    {
    }
    public delegate ExecuteResult ExecuteHandler(string payload);
    public event ExecuteHandler Execute;
    public delegate getResult GetHandler();
    public event GetHandler Get;
    public override Task<RpcResponse> Call(RpcRequest request, ServerCallContext context)
    {
        string body = request.Body;

        ExecuteResult payload = new();
        if (Execute != null)
        {
            payload = Execute(body);
        }

        return Task.FromResult(new RpcResponse { Code = payload.Code, Msg = payload.Msg });
    }

    public override Task<SimpleResponse> Close(EmptyMessage request, ServerCallContext context)
    {
        return base.Close(request, context);
    }


}
