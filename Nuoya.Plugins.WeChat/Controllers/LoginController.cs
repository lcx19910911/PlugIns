using Core.AuthAPI;
using EnumPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class LoginController : BaseController
    {

        public IPersonService IPersonService;

        public LoginController(IPersonService _IPersonService)
        {
            this.IPersonService = _IPersonService;
        }

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
            var person = IPersonService.Login(account, password);
            if (person != null)
            {
                this.LoginUser = new Core.Model.LoginUser(person);
                return JResult(true);
            }
            else
            {
                var result = AuthAPI4Fun.Login(account, password);
                if (result != null && result.code == 100)
                {
                    person = IPersonService.Manager_Person(result.data, account, password);
                    this.LoginUser = new Core.Model.LoginUser(person);
                    return JResult(true);
                }
                else
                    return JResult(false);
            }
        }


        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Quit()
        {
            this.LoginUser = null;
            return View("Index");
        }
    }
}