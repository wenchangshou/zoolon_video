using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Control;
using Grpc.Core;
using Newtonsoft.Json;

    public class ControlStruct
{
    public string Action { get; set; }
}
public class controlImpl : RpcCall.RpcCallBase
{
    public controlImpl()
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

    /**
     * 
     */
    public override Task<SimpleResponse> Ping(EmptyMessage request, ServerCallContext context)
    {

        return Task.FromResult(new SimpleResponse { Code = 0 });

    }

    public override Task<RpcGetResponse> Get(RpcGetRequest request, ServerCallContext context)
    {
        var result = get();
        return Task.FromResult(new RpcGetResponse { Code = result.Code, Msg = result.Msg, Payload = result.Payload });
    }
}
