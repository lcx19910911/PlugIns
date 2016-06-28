using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mall.Goods
{
    public class GoodsModel
    {
        public Repository.Goods Goods { get; set; }

        public List<Repository.GoodsDetails> GoodsDetails { get; set; }
    }
}
