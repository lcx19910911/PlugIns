using Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DinnerDish
{
    public class List
    {

        public string UNID { get; set; }
        public string Name { get; set; }
        [DataAutoMapper("DinnerCategoryId", typeof(Repository.DinnerCategory), "UNID", "Name")]
        public string CategoryName { get; set; }
        public int Sort { get; set; }
        public decimal Price { get; set; }
        public string Label { get; set; }
        public System.DateTime CreatedTime { get; set; }
    }
}
