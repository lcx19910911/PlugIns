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
using Service;

namespace Nuoya.Plugins.WeChat.Areas.Puzzle.Controllers
{
    /// <summary>
    /// 拼图
    /// </summary>
    public class HomeController : PuzzleBaseController
    {
        public IPuzzleService IPuzzleService;

        public HomeController(IPuzzleService _IPuzzleService)
        {
            this.IPuzzleService = _IPuzzleService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string unid)
        {
            var model = new Model.Puzzle();
            ViewData["LastOne"] = false;
            //预览
            if (this.LoginUser != null&&!string.IsNullOrEmpty(unid))
            {
                model = IPuzzleService.Find_Puzzle(unid);
                return View(model);
            }
            var user = CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (user == null|| person==null)
                return OAuthExpired();

            model = IPuzzleService.Get_NextPuzzle(unid, user?.openid,person?.UNID) ;
            if (model == null||model.UNID.Equals(unid))
                ViewData["LastOne"] = true;

            return View(model);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult Complete(string unid)
        {
            var result = IPuzzleService.Complete(unid);
            return JResult(result);
        }
    }
}