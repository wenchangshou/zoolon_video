using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoolon_videoplayer
{
    public class getResult
    {
        public getResult()
        {
            this.Code = 0;
            this.Msg = "success";
            this.Payload = "";
        }
        public getResult(int code, string msg,string payload)
        {
            Code = code;
            Msg = msg;
            payload = payload;

        }
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Payload { get; set; }
    }
}
