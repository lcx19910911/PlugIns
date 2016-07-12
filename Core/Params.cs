using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;

namespace Core
{
    public class Params
    {
        /// <summary>
        /// 公众号appid
        /// </summary>
        public static readonly string AppId = ConfigurationManager.AppSettings["AppId"];

        /// <summary>
        /// 公众号密钥
        /// </summary>
        public static readonly string AppSecret = ConfigurationManager.AppSettings["AppSecret"];

        /// <summary>
        /// 域名
        /// </summary>
        public static readonly string DomianName = ConfigurationManager.AppSettings["DomianName"];

        /// <summary>
        /// 时间戳有效时间c
        /// </summary>
        public const int TimspanExpiredMinutes = 60;
        /// <summary>
        /// token失效时间
        /// </summary>
        public const int ExpiredDays = 7;
        /// <summary>
        /// 跟平台通信密钥
        /// </summary>
        public static readonly string SecretKey = ConfigurationManager.AppSettings["Com_SharedKey"];
        /// <summary>
        /// 平台请求登陆接口
        /// </summary>
        public static readonly string LoginUrl =string.Format("{0}/{1}", ConfigurationManager.AppSettings["Com_Site"], "api/CompanyAccount/Login");

        /// <summary>
        /// 平台地址
        /// </summary>
        public static readonly string ComUrl = ConfigurationManager.AppSettings["Com_Site"];

        /// <summary>
        /// 每天签到积分
        /// </summary>
        public static readonly int SignScore = 10;

        /// <summary>
        /// 连续十天签到积分
        /// </summary>
        public static readonly int TendayScore = 30;

        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "wechat_user";

        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string PeopleCookieName = "web_people";
    }
}
