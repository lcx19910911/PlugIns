using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DinnerShop
{
    public class List
    {

        public string UNID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public int Sort { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }

        public string HoldTime { get; set; }

        public System.DateTime CreatedTime { get; set; }
        public System.DateTime UpdatedTime { get; set; }

        public string Flag { get; set; }
    }
}
