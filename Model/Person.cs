namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户
    /// </summary>
    [Table("Person")]
    public partial class Person
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号")]
        [MaxLength(12)]
        [MinLength(6)]
        public string Account { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        public string Name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [MaxLength(100)]
        [MinLength(6)]
        public string Password { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [Display(Name = "状态")]
        public long Flag { get; set; }
        /// <summary>
        /// 平台id
        /// </summary>
        [Display(Name = "平台id")]
        public string ComId { get; set; }

        /// <summary>
        /// 是否是子级
        /// </summary>
        [Display(Name = "是否是子级")]
        public int IsChildren { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        [Display(Name = "店铺id")]
        public string ShopId { get; set; }

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
