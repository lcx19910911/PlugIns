namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 奖品设置
    /// </summary>
    [Table("Prize")]
    public partial class Prize
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 活动类型
        /// </summary>
        [Display(Name = "活动类型")]
        public int TargetCode { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [Display(Name = "活动id")]
        public string TargetID { get; set; }
        /// <summary>
        /// 一等奖奖品
        /// </summary>
        [Display(Name = "一等奖奖品")]
        [MaxLength(50)]
        public string OnePrize { get; set; }
        /// <summary>
        /// 一等奖个数
        /// </summary>
        [Display(Name = "一等奖个数")]
        [Range(1,5000)]
        public int OnePrizeCount { get; set; }
        /// <summary>
        /// 一等奖图片
        /// </summary>
        [Display(Name = "一等奖图片")]
        [MaxLength(500)]
        public string OnePrizeImage { get; set; }
        /// <summary>
        /// 二等奖奖品
        /// </summary>
        [Display(Name = "二等奖奖品")]
        [MaxLength(50)]
        public string TwoPrize { get; set; }
        /// <summary>
        /// 二等奖个数
        /// </summary>
        [Display(Name = "二等奖个数")]
        [Range(1, 5000)]
        public int TwoPrizeCount { get; set; }
        /// <summary>
        /// 二等奖图片
        /// </summary>
        [Display(Name = "二等奖图片")]
        [MaxLength(500)]
        public string TwoPrizeImage { get; set; }
        /// <summary>
        /// 三等奖奖品
        /// </summary>
        [Display(Name = "三等奖奖品")]
        [MaxLength(50)]
        public string ThreePrize { get; set; }
        /// <summary>
        /// 三等奖个数
        /// </summary>
        [Display(Name = "三等奖个数")]
        [Range(1, 5000)]
        public int ThreePrizeCount { get; set; }
        /// <summary>
        /// 三等奖图片
        /// </summary>
        [Display(Name = "三等奖图片")]
        [MaxLength(500)]
        public string ThreePrizeImage { get; set; }
        /// <summary>
        /// 奖品数
        /// </summary>
        [Display(Name = "奖品数")]
        public int AllCount { get; set; }
        /// <summary>
        /// 预计参与人数
        /// </summary>
        [Display(Name = "预计参与人数")]
        public int ExpectedPeopleCount { get; set; }
        /// <summary>
        /// 已中奖数
        /// </summary>
        [Display(Name = "已中奖数")]
        public int hadPrizeCount { get; set; }
        /// <summary>
        /// 每天每人次数限制
        /// </summary>
        [Display(Name = "每天每人次数限制")]
        public int DayLimt { get; set; }
        /// <summary>
        /// 每人总次数限制
        /// </summary>
        [Display(Name = "每人总次数限制")]
        [Range(1, 100000)]
        public int AllCountLimt { get; set; }
        /// <summary>
        /// 是否显示奖品数
        /// </summary>
        [Display(Name = "是否显示奖品数")]
        public int IsShowCount { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Display(Name = "更新时间")]
        [Timestamp]
        public byte[] TimeStamp { get; set; }
    }
}
