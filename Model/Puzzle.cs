namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 拼图
    /// </summary>
    [Table("Puzzle")]
    public partial class Puzzle
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 拼图名
        /// </summary>
        [Display(Name = "拼图名")]
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public long Flag { get; set; }
        /// <summary>
        /// 难度  3*3
        /// </summary>
        [Display(Name = "难度")]
        public int DifficultyType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
        /// <summary>
        /// 是否绑定积分
        /// </summary>
        [Display(Name = "是否绑定积分")]
        public int IsBindScore { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        [Display(Name = "积分")]
        [Range(1, 100000)]
        public int Score { get; set; }

        /// <summary>
        /// 拼图完毕提示
        /// </summary>
        [Display(Name = "拼图完毕提示")]
        [MaxLength(64)]
        public string BindTitle { get; set; }

        /// <summary>
        /// 绑定活动名称
        /// </summary>
        [Display(Name = "绑定活动名称")]
        [MaxLength(500)]
        public string BindName { get; set; }
        /// <summary>
        /// 绑定logo
        /// </summary>
        [Display(Name = "绑定logo")]
        [MaxLength(500)]
        public string BindLogoUrl { get; set; }
        /// <summary>
        /// 平台绑定地址
        /// </summary>
        [Display(Name = "平台绑定地址")]
        [MaxLength(500)]
        public string BindUrl { get; set; }

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
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        [MaxLength(500)]
        public string Image { get; set; }

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
