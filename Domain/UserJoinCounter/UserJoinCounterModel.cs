using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserJoinCounter
{
    public class UserJoinCounterModel
    {
        public string UNID { get; set; }

        public string Name { get; set; }

        public string TargetCode { get; set; }

        public string OpenID { get; set; }

        public System.DateTime CreatedTime { get; set; }

        public string SN { get; set; }

        public string PrizeResult { get; set; }

        public string IsCach { get; set; }

        public DateTime? CashTime { get; set; }
    }
}
