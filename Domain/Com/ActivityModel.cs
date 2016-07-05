using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Com
{
    public class ActivityModel
    {
        public int ComContentID { get; set; }

        public int FK_ApplyID { get; set; }

        //活动类型名
        public string ApplyName { get; set; }


        public int FK_ID { get; set; }

        /// <summary>
        /// 主标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片地址  120
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public string SubTitle { get; set; }

        public string Remark { get; set; }

        public DateTime CreatedDT { get; set; }

        public int OnWall { get; set; }

        //路径
        public string page_url { get; set; }
    }
}
