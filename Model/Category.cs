namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 分类 （商品 菜品分类）
    /// </summary>
    [Table("Category")]
    public partial class Category
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]      
        public string UNID { get; set; }
        /// <summary>
        /// 分类名
        /// </summary>
        [Display(Name = "分类名")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        [Display(Name = "店铺id")]
        public string ShopId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Display(Name = "用户id")]
        public string PersonId { get; set; }
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
        [Display(Name= "修改时间")]
        public System.DateTime UpdatedTime { get; set; }
    }
}
