namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 点餐
    /// </summary>
    [Table("DinnerOrder")]
    public partial class DinnerOrder
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        public string OrderNum { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        [Display(Name = "店铺id")]
        public string ShopId { get; set; }
        /// <summary>
        /// 微信OpenID号
        /// </summary>
        [Display(Name = "微信OpenID号")]
        public string OpenId { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        [Display(Name = "总价")]
        [Range(typeof(decimal), "0.00", "99999999.99", ErrorMessage = "总价格格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-8]+\.[0-9]{1,2}))$", ErrorMessage = "总价价格不正确！")]
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(50)]
        public string Remark { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        [Display(Name = "明细")]
        [MaxLength(1000)]
        public string Details { get; set; }
        /// <summary>
        /// 菜品状态
        /// </summary>
        public int State { get; set; }
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
