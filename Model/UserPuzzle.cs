namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户拼图记录
    /// </summary>
    [Table("UserPuzzle")]
    public partial class UserPuzzle
    {
        [Key]
        public string UNID { get; set; }
        /// <summary>
        /// 拼图id
        /// </summary>
        public string PuzzleId { get; set; }
        /// <summary>
        /// 拼图时间
        /// </summary>
        public System.DateTime PuzzleDate { get; set; }
        /// <summary>
        /// 微信openid
        /// </summary>
        public string OpenId { get; set; }
    }
}
