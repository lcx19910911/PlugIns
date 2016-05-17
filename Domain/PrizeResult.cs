using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// 活动结果
    /// </summary>
    public class PrizeResult
    {
        /// <summary>
        /// SN码
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 是否中奖
        /// </summary>
        public bool IsPrize { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        public bool IsError { get; set; } = true;
    }
}
