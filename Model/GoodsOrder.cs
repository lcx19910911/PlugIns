namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("GoodsOrder")]

    public partial class GoodsOrder
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 商品id
        /// </summary>
        [Display(Name = "商品id")]
        public string GoodsId { get; set; }
        /// <summary>
        /// 微信Openid
        /// </summary>
        [Display(Name = "微信Openid")]
        public string OpenId { get; set; }
        /// <summary>
        /// 人员id
        /// </summary>
        [Display(Name = "人员id")]
        public string PersonId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Count { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        [Display(Name = "总价")]
        [Range(typeof(decimal), "0.00", "9999999999.99", ErrorMessage = "原价格式不正确")]
        [RegularExpression(@"^(([0-9]+)|([0-10]+\.[0-9]{1,2}))$", ErrorMessage = "原价价格不正确！")]
        public decimal AllPrice { get; set; }
        /// <summary>
        /// 积分总数
        /// </summary>
        [Display(Name = "积分总数")]
        public int ScoreNum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
    }
}
