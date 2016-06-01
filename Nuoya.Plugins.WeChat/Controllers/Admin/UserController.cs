using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extension;
using Repository;
using Nuoya.Plugins.WeChat.Filters;

namespace Nuoya.Plugins.WeChat.Controllers
{

    public class UserController : BaseController
    {

        [LoginFilter]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="groupName">分组名称 - 搜索项</param>
        /// <param name="keyValue">键值 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_UserPageList(pageIndex, pageSize, name, openId, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }
        /// <summary>
        /// 授权保存code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">原来请求地址</param>
        /// <returns></returns>
        //public ActionResult DoAuth(string code, string state)
        //{
        //    CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () => {
        //        return WebService.Update_User(code)? code:"";
        //        //return "12345678";
        //    });
        //    return Redirect(state);
        //}

    }
}