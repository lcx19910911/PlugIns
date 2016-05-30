using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumPro
{
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,
        /// <summary>
        /// 服务器错误
        /// </summary>
        [Description("服务器错误")]
        ServerError = 1,
        /// <summary>
        /// 自定义错误
        /// </summary>
        [Description("自定义错误")]
        CustomError = 2,

        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        [Description("用户名或密码错误")]
        UsernameOrPasswordWrong = 100

    }
}
