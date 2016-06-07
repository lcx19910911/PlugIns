using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AuthAPI
{
    public class APIResult
    {
        public int code { get; set; }

        public string msg { get; set; }

        public ResultData data { get; set; }
    }


    public class ResultData
    {
        public string access_token { get; set; }

        public string expires_in { get; set; }


        public string username { get; set; }

        public string comaccountid { get; set; }

        public string name { get; set; }

        public string comid { get; set; }
    }
}
