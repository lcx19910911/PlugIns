using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Domain.ScratchCard
{
    /// <summary>
    /// 增加刮刮卡活动
    /// </summary>
    public class Update: Domain.Prize.Update
    {   

        /// <summary>
        /// 活动名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 回复关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public System.DateTime OngoingTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public System.DateTime OverTime { get; set; }
        /// <summary>
        /// 重复抽奖提醒
        /// </summary>
        public string RepeatNotice { get; set; }
        /// <summary>
        /// 预热标题
        /// </summary>
        public string PreheatingTitle { get; set; }
        /// <summary>
        /// 预热图片
        /// </summary>
        public string PreheatingImage { get; set; }
        /// <summary>
        /// 预热描述
        /// </summary>
        public string PreheatingDescribe { get; set; }
        /// <summary>
        /// 进行时标题
        /// </summary>
        public string OngoingTitle { get; set; }
        /// <summary>
        /// 进行时图片
        /// </summary>
        public string OngoingImage { get; set; }
        /// <summary>
        /// 进行时描述
        /// </summary>
        public string OngoingDescribe { get; set; }
        /// <summary>
        /// 结束标题
        /// </summary>
        public string OverTitle { get; set; }
        /// <summary>
        /// 结束图片
        /// </summary>
        public string OverImage { get; set; }
        /// <summary>
        /// 结束描述
        /// </summary>
        public string OverDescribe { get; set; }
  
    }
}
