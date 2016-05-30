using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.DinnerShop
{
    public class Update
    {
        public string UNID { get; set; }

        public string Name { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public int Sort { get; set; }
        public string ContactPhone { get; set; }
        public string Address { get; set; }
        public string Introduction { get; set; }
        public string StartShoptime { get; set; }
        public string EndShoptime { get; set; }
        public string Image { get; set; }
        public string Mark { get; set; }

        public long RoleFlag { get; set; }
    }
}
