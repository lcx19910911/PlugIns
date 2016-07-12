namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 商品
    /// </summary>
    [Table("Goods")]
    public partial class Goods
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        [Display(Name = "商品名")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        [Display(Name = "分类id")]
        public string CategoryId { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        [Display(Name = "库存数量")]
        public int StockNum { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Display(Name = "剩余数量")]
        public int SurplusNum { get; set; }
        /// <summary>
        /// 活动结束时间
        /// </summary>
        [Display(Name = "活动结束时间")]
        [Required]
        public System.DateTime OverTime { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        [Display(Name = "活动开始时间")]
        [Required]
        public System.DateTime OngoingTime { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        [Display(Name = "原价")]
        [Range(typeof(decimal), "0.00", "99999999.99", ErrorMessage = "原价格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-8]+\.[0-9]{1,2}))$", ErrorMessage = "原价价格不正确！")]
        [Required]
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 销售价
        /// </summary>
        [Display(Name = "销售价")]
        [Range(typeof(decimal), "0.00", "99999999.99", ErrorMessage = "销售价格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-8]+\.[0-9]{1,2}))$", ErrorMessage = "销售价价格不正确！")]
        [Required]
        public decimal SellingPrice { get; set; }
        /// <summary>
        /// 需要积分
        /// </summary>
        [Display(Name = "需要积分")]
        public int ScoreNum { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public long Flag { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        [Display(Name = "人员id")]
        public string PersonId { get; set; }

        [Timestamp]
        public byte[] TimeStamp { get; set; }
        /// <summary>
        /// 菜品状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        [Display(Name = "图标")]
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
