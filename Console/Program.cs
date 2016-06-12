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

            new PersonService().Add_Person("11", 111);

            WXUser entity = new WXUser()
            {
                openid = "1112dsds31sd",
                nickname = "哈哈啊查询查询",
                city = "福州",
                country = "中国",
                headimgurl = "/Upload/ScratchCard/4b426174f4134a868920e957be691f7b.jpg",
                province = "福建",
                sex = "1"
            };
            string dd = "?info=" + HttpUtility.UrlEncode(entity.ToJson()) + "&unid=9f3726004c63406aa4cfa7ae73a3f53a";
            string ss = "";
        }
    }
}
