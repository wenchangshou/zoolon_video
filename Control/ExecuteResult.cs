using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    public class ExecuteResult
    {
        public ExecuteResult()
        {
            Code = 0;
            Msg = "success";
        }
        public ExecuteResult(bool reply,int code, string msg)
        {
            Code = code;
            Msg = msg;
            Reply = reply;
        }
        public bool Reply { get; set; }

        public int Code { get; set; }
        public string? Msg { get; set; }
        public string encode()
        {
            string jsonString = JsonConvert.SerializeObject(this);
            return jsonString;

        }
    }
}