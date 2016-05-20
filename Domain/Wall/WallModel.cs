using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Wall
{
    public class WallModel
    {
        public string UNID { get; set; }
        public string Name { get; set; }
        public string Sponsor { get; set; }

        public System.DateTime OngoingTime { get; set; }
        public System.DateTime OverTime { get; set; }

        public System.DateTime CreatedTime { get; set; }

        public System.DateTime UpdatedTime { get; set; }
        public string State
        {
            get
            {
                if (OngoingTime > DateTime.Now)
                    return "未开始";
                else if (this.OngoingTime <= DateTime.Now && DateTime.Now < this.OverTime)
                    return "正在进行中";
                else if (this.OverTime <= DateTime.Now)
                    return "已结束";
                else
                    return "未知";
            }
        }
    }
}
