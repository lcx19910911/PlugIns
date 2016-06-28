using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Mall.Recommend
{
    public class RecommendModel
    {
        public string UNID { get; set; }
        public string Title { get; set; }
        public int Sort { get; set; }
        public string TargetCode { get; set; }
        public string RecommendCode { get; set; }
    }
}
