using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    /// <summary>
    /// 刮刮卡
    /// </summary>
    public class ApiScratchCardModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }
        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public System.DateTime OngoingTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public System.DateTime OverTime { get; set; }
        /// <summary>
        /// 重复抽奖提示
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
        /// 预热说明
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
        /// 进行时说明
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
        /// 结束说明
        /// </summary>
        public string OverDescribe { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreatedTime { get; set; }
    }
}
