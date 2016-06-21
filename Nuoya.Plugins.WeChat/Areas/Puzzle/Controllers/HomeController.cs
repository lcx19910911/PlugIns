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

namespace Nuoya.Plugins.WeChat.Areas.Puzzle.Controllers
{
    /// <summary>
    /// 拼图
    /// </summary>
    public class HomeController : PuzzleBaseController
    {
        public IUserSignService IUserSignService;
        public IUserService IUserService;

        public HomeController(IUserSignService _IUserSignService, IUserService _IUserService)
        {
            this.IUserSignService = _IUserSignService;
            this.IUserService = _IUserService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult Sign()
        {
            var result = IUserSignService.User_Sign();
            return JResult(result);
        }
    }
}