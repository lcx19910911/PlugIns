using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API
{
    /// <summary>
    /// 奖品
    /// </summary>
    public class ApiPrizeModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string UNID { get; set; }
        /// <summary>
        /// 一等奖奖品
        /// </summary>
        public string OnePrize { get; set; }
        /// <summary>
        /// 一等奖个数
        /// </summary>
        public int OnePrizeCount { get; set; }
        /// <summary>
        /// 二等奖奖品
        /// </summary>
        public string TwoPrize { get; set; }
        /// <summary>
        /// 二等奖个数
        /// </summary>
        public int TwoPrizeCount { get; set; }
        /// <summary>
        /// 三等奖奖品
        /// </summary>
        public string ThreePrize { get; set; }
        /// <summary>
        /// 三等奖个数
        /// </summary>
        public int ThreePrizeCount { get; set; }
        /// <summary>
        /// 奖品数
        /// </summary>
        public int AllCount { get; set; }
        /// <summary>
        /// 预计参与人数
        /// </summary>
        public int ExpectedPeopleCount { get; set; }
        /// <summary>
        /// 已中奖数
        /// </summary>
        public int hadPrizeCount { get; set; }
        /// <summary>
        /// 每天每人次数限制
        /// </summary>
        public int DayLimt { get; set; }
        /// <summary>
        /// 每人总次数限制
        /// </summary>
        public int AllCountLimt { get; set; }
        /// <summary>
        /// 是否显示奖品数（1显示）
        /// </summary>
        public int IsShowCount { get; set; }
    }
}
