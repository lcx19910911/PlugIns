using Core;
using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MPUtil;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 授权保存code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">原来请求地址</param>
        /// <returns></returns>
        public ActionResult DoAuth(string code, string state)
        {
            CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () => {
                return WebService.Update_User(code)? code:"";
                //return "12345678";
            });
            return Redirect(state);
        }
    }
}