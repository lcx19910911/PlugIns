using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Domain.Dinner
{
    public class OrderModel
    {
        public DinnerOrder Order { get; set; }
        public List<OrderDetails> Details { get; set; }

        public string Remark { get; set; }
    }
}
