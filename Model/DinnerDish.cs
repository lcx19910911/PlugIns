namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 菜品
    /// </summary>
    [Table("DinnerDish")]
    public partial class DinnerDish
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 菜品名
        /// </summary>
        [Display(Name = "菜品名")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        [Display(Name = "店铺id")]
        public string ShopId { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        [Display(Name = "分类id")]
        [Required]
        public string CategoryId { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "价格")]
        [Range(typeof(decimal), "0.00", "9999.99", ErrorMessage = "产品价格格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-4]+\.[0-9]{1,2}))$", ErrorMessage = "产品价格不正确！")]
        public decimal Price { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [Display(Name = "标签")]
        [MaxLength(100)]
        public string Label { get; set; }
        /// <summary>
        /// 菜品状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [MaxLength(500)]
        public string Description { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Display(Name = "图标")]
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
