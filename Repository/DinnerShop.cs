namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 店铺
    /// </summary>
    [Table("DinnerShop")]
    public partial class DinnerShop
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 店铺名
        /// </summary>
        [Display(Name = "店铺名")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        [MaxLength(100)]
        public string Address { get; set; }
        /// <summary>
        /// 自我介绍
        /// </summary>
        [Display(Name = "自我介绍")]
        [MaxLength(1000)]
        public string Introduction { get; set; }
        /// <summary>
        /// 开始营业时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Required]
        public System.DateTime StartShoptime { get; set; }
        /// <summary>
        /// 结束营业时间
        /// </summary>
        [Display(Name = "结束营业时间")]
        [Required]
        public System.DateTime EndShoptime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Mark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public long Flag { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        [MaxLength(500)]
        public string Image { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; }
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
