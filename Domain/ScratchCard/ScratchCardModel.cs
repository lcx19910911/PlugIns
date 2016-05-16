using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ScratchCard
{
    public class ScratchCardModel
    {
        public string UNID { get; set; }
        public string Name { get; set; }
        public string KeyWord { get; set; }
        public Nullable<System.DateTime> OngoingTime { get; set; }
        public Nullable<System.DateTime> OverTime { get; set; }
      
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }

        public string State { get {
                if (OngoingTime < DateTime.Now)
                    return "未开始";
                else if (OverTime > DateTime.Now)
                    return "正在进行中";
                else
                    return "已结束";
            } }
    }
}
