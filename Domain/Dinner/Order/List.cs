using Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dinner.Order
{
    public class List
    {
        public string UNID { get; set; }
        public string OrderNum { get; set; }
        [DataAutoMapper("OpenId", typeof(Repository.User), "OpenID", "NickName")]
        public string NickName { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }
        public string Details { get; set; }
        public int State { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
