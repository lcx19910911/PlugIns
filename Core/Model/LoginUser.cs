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
        public LoginUser(string AppId, string AppSecret)
        {
            this.AppId = AppId;
            this.AppSecret = AppSecret;

        }
        /// <summary>
        /// 公众号appid
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }

    }
}
