namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 点餐订单明细
    /// </summary>
    [Table("OrderDetails")]
    public partial class OrderDetails
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 订单id
        /// </summary>
        [Display(Name = "订单id")]
        public string OrderId { get; set; }
        /// <summary>
        /// 菜品id
        /// </summary>
        [Display(Name = "订单id")]
        public string DishId { get; set; }
        /// <summary>
        /// 菜品名
        /// </summary>
        [Display(Name = "订单id")]
        public string DishName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "价格")]
        [Range(typeof(decimal), "0.00", "9999.99", ErrorMessage = "价格格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-4]+\.[0-9]{1,2}))$", ErrorMessage = "价格价格不正确！")]
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Number { get; set; }
    }
}
