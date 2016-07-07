using Core.AuthAPI;
using EnumPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Core.Helper;
using Core;
using Nuoya.Plugins.WeChat.Filters;
using Core.Model;
using System.Threading.Tasks;

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
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View();
            });
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
        /// 登录提交
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param> 
        /// <returns></returns>
        [LoginFilter]
        public ActionResult ChildrenLogin(string unid)
        {
            var person = IPersonService.Get_ByShopId(unid);
            if (person != null)
            {
                this.LoginUser = new Core.Model.LoginUser(person);
                return Redirect("/Dinner/Category/index");
            }
            else
                return Redirect("/home/index");
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