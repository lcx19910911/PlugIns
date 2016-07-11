namespace Repository
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户签到记录
    /// </summary>
    [Table("UserSign")]
    public partial class UserSign
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 签到时间
        /// </summary>
        public System.DateTime SignDate { get; set; }
        /// <summary>
        /// 连续签到天数
        /// </summary>
        public int SignNum { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string PersonId { get; set; }
    }
}
