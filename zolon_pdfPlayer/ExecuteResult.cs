using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ExecuteResult
{
    public ExecuteResult()
    {
        Code = 0;
        Msg = "success";
    }
    public ExecuteResult(int code, string msg)
    {
        this.Code = code;
        this.Msg = msg;
    }

    public int Code { get; set; }
    public string Msg { get; set; }
}
