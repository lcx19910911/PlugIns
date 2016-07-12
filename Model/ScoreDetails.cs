namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 积分明细
    /// </summary>
    [Table("ScoreDetails")]
    public partial class ScoreDetails
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        [Display(Name = "微信openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 变化值
        /// </summary>
        [Display(Name = "变化值")]
        public int Value { get; set; }
        /// <summary>
        /// 是否增加积分
        /// </summary>
        [Display(Name = "是否增加积分")]
        public int IsAdd { get; set; }
        /// <summary>
        /// 操纵类型
        /// </summary>
        [Display(Name = "操纵类型")]
        public int Type { get; set; }
        /// <summary>
        /// 活动UNID
        /// </summary>
        [Display(Name = "活动UNID")]
        public string TargetId { get; set; }
    }
}
