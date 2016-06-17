using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Sign
{
    public class SignModel
    {
        public string UNID { get; set; }
        public string OpenId { get; set; }
        public System.DateTime SignDate { get; set; }

        public int SignNum { get; set; }
    }
}
