using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mall.Order
{
    public class OrderModel
    {
        public string UNID { get; set; }
        public string GoodsId { get; set; }
        public string GoodsName { get; set; }
        public string OpenId { get; set; }
        public string NickName { get; set; }
        public string PersonId { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> AllPrice { get; set; }
        public Nullable<int> ScoreNum { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
