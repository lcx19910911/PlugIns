using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Code
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        public LoginUser(string UNID)
        {
            this.UserId = UNID;

        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        ///// <summary>
        ///// 用户手机号 
        ///// </summary>
        //public string Phone { get; set; }

        ///// <summary>
        ///// 用户昵称
        ///// </summary>
        //public string Nickname { get; set; }


    }
}
