namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户参与记录
    /// </summary>
    [Table("UserJoinCounter")]
    public partial class UserJoinCounter
    {
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
        /// 微信openid
        /// </summary>
        [Display(Name = "微信openid")]
        public string OpenID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 是否中奖
        /// </summary>
        [Display(Name = "是否中奖")]
        public int IsPrize { get; set; }
        /// <summary>
        /// 奖品等级
        /// </summary>
        [Display(Name = "奖品等级")]
        public int PrizeGrade { get; set; }
        /// <summary>
        /// sn中奖码
        /// </summary>
        [Display(Name = "sn中奖码")]
        public string SN { get; set; }
        /// <summary>
        /// 是否兑换
        /// </summary>
        [Display(Name = "是否兑换")]
        public int IsCach { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public Nullable<System.DateTime> CachTime { get; set; }
    }
}
