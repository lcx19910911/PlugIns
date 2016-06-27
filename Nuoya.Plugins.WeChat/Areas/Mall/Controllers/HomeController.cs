using Core;
using Core.Extensions;
using Core.Web;
using Nuoya.Plugins.WeChat.Filters;
using Repository;
using IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MPUtil.UserMng;
using Domain.User;

namespace Nuoya.Plugins.WeChat.Areas.Mall.Controllers
{
    /// <summary>
    /// 商城
    /// </summary>
    public class HomeController : MallBaseController
    {
        public ICategoryService ICategoryService;
        public IUserService IUserService;

        public HomeController(ICategoryService _ICategoryService, IUserService _IUserService)
        {
            this.ICategoryService = _ICategoryService;
            this.IUserService = _IUserService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            Repository.User user=CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user==null|| person==null)
                return Error();
            UserCenterModel model = new UserCenterModel();

            return View(model);
        }

    }
}