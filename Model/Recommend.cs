namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 商城推荐
    /// </summary>
    [Table("Recommend")]
    public partial class Recommend
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        [MaxLength(64)]
        public string Title { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; }
        /// <summary>
        /// 活动id
        /// </summary>
        [Display(Name = "活动id")]
        [Required]
        public string TargetID { get; set; }
        /// <summary>
        /// 活动类型
        /// </summary>
        [Display(Name = "活动类型")]
        [Required]
        public int TargetCode { get; set; }
        /// <summary>
        /// 推荐类型
        /// </summary>
        [Display(Name = "推荐类型")]
        [Required]
        public int RecommendCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
    }
}
