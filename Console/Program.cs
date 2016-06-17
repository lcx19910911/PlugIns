using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Core.Extensions;
using Domain;
using System.Web;
using MPUtil.UserMng;
using Service;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {

            var dsd = DateTime.Today;

            WXUser entity = new WXUser()
            {
                openid = "1112dsdsdsdsdsd",
                nickname = "哈哈啊查询查询",
                city = "福州",
                country = "中国",
                headimgurl = "http://wx.qlogo.cn/mmopen/PYEBjepVt4IW6ibsc3I8NVLN7CSIEzlsngoKK1Rka4nfUh6ia961qEUWSBTFH5OaRaf5qxpicz2bPnictCQ0fV8Sh5sLlLoVhibF9/0",
                province = "福建",
                sex = "1"
            };
            string dd = "?info=" + HttpUtility.UrlEncode(entity.ToJson());
            string ss = "";
        }
    }
}
