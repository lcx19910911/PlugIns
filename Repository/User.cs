namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 微信用户
    /// </summary>
    [Table("User")]
    public partial class User
    {
        [Key]
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        public string HeadImgUrl { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家")]
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        [Display(Name = "省份")]
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        public string City { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public int Sex { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Display(Name = "手机号码")]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        public System.DateTime CreatedTime { get; set; }
    }
}
