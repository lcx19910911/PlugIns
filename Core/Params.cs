using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
    }
}
