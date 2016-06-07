using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Core.Extensions;
using Domain;
using System.Web;


namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            User entity = new User()
            {
                CreatedTime = DateTime.Now,
                OpenId = "1112dsdsdsdsdsd",
                NickName = "哈哈啊查询查询",
                City = "福州",
                Country = "中国",
                HeadImgUrl = "/Upload/ScratchCard/4b426174f4134a868920e957be691f7b.jpg",
                Province = "福建",
                Sex = 1
            };
            string dd = "?info=" + HttpUtility.UrlEncode(entity.ToJson()) + "&shopid=9f3726004c63406aa4cfa7ae73a3f53a";
            string ss = "";
        }
    }
}
