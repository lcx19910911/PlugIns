using  EnumPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录提交
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param> 
        /// <returns></returns>
        public JsonResult Submit(string account, string password)
        {         
            var user = WebService.Login(account, password);
            if (user != null)
            {
                Client.LoginUser = new Core.Code.LoginUser(user.UNID, user.Account, user.Name, user.Mobile, WebService.Get_UserMenuLimit((long)user.RoleFlag),(TargetCode)user.TargetCode,user.TargetID);
                return JResult(true);
            }
            else
            {
                return JResult(false);
            }
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            Client.LoginUser = null;
            return View("Index");
        }
    }
}