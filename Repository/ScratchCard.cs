namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 刮刮卡
    /// </summary>
    [Table("ScratchCard")]
    public partial class ScratchCard
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "刮刮卡名称")]
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "重复抽奖提示")]
        [Required]
        [MaxLength(64)]
        public string RepeatNotice { get; set; }

        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "预热标题")]
        [Required]
        [MaxLength(64)]
        public string PreheatingTitle { get; set; }

        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "预热图片")]
        [Required]
        [MaxLength(500)]
        public string PreheatingImage { get; set; }
        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "预热说明")]
        [Required]
        [MaxLength(2000)]
        public string PreheatingDescribe { get; set; }
        /// <summary>
        /// 刮刮卡名称
        /// </summary>
        [Display(Name = "进行时标题")]
        [Required]
        [MaxLength(64)]
        public string OngoingTitle { get; set; }
        /// <summary>
        /// 进行时图片
        /// </summary>
        [Display(Name = "进行时图片")]
        [Required]
        [MaxLength(500)]
        public string OngoingImage { get; set; }
        /// <summary>
        /// 进行时说明
        /// </summary>
        [Display(Name = "进行时说明")]
        [Required]
        [MaxLength(2000)]
        public string OngoingDescribe { get; set; }
        /// <summary>
        /// 结束标题
        /// </summary>
        [Display(Name = "结束标题")]
        [Required]
        [MaxLength(64)]
        public string OverTitle { get; set; }
        /// <summary>
        /// 结束图片
        /// </summary>
        [Display(Name = "结束图片")]
        [Required]
        [MaxLength(500)]
        public string OverImage { get; set; }
        /// <summary>
        /// 结束说明
        /// </summary>
        [Display(Name = "结束说明")]
        [Required]
        [MaxLength(2000)]
        public string OverDescribe { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public long Flag { get; set; }


        /// <summary>
        /// 活动开始时间
        /// </summary>
        [Display(Name = "活动开始时间")]
        [Required]
        public System.DateTime OngoingTime { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        [Display(Name = "活动结束时间")]
        [Required]
        public System.DateTime OverTime { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        public System.DateTime UpdatedTime { get; set; }
    }
}
