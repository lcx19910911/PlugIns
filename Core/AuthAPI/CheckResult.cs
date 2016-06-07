using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AuthAPI
{
    public class CheckResult
    {
        public int code { get; set; }
        public string msg { get; set; }
        public TokenInfo tokenInfo { get; set; }
    }
}
