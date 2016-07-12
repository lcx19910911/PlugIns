namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户积分
    /// </summary>
    [Table("UserScore")]
    public partial class UserScore
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string PersonId { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Score { get; set; }
    }
}
