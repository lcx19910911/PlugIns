using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Prize
{
    public class Update
    {       
        /// <summary>
        /// 一等奖名
        /// </summary>
        public string OnePrize { get; set; }
        /// <summary>
        /// 一等奖个数
        /// </summary>
        public int OnePrizeCount { get; set; }
        /// <summary>
        /// 二等奖名
        /// </summary>
        public string TwoPrize { get; set; }
        /// <summary>
        /// 二等奖个数
        /// </summary>
        public int TwoPrizeCount { get; set; }
        /// <summary>
        /// 三等奖名
        /// </summary>
        public string ThreePrize { get; set; }
        /// <summary>
        /// 三等奖个数
        /// </summary>
        public int ThreePrizeCount { get; set; }

        /// <summary>
        /// 预计参与人数
        /// </summary>
        public int ExpectedPeopleCount { get; set; }
        /// <summary>
        /// 每人每天次数限制
        /// </summary>
        public int DayLimt { get; set; }
        /// <summary>
        /// 没人总次数限制
        /// </summary>
        public int AllCountLimt { get; set; }
        /// <summary>
        /// 是否显示奖品数量
        /// </summary>
        public bool IsShowCount { get; set; } = true;
    }
}
