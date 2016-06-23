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
            var user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null|| person==null)
                return Error();

            var model = IPuzzleService.Get_NextPuzzle(unid, user.OpenId,person.UNID) ;
            ViewData["LastOne"] = false;
            if (model == null)
                return Error();
            else if (model.UNID.Equals(unid))
                ViewData["LastOne"] = true;

            return View(model);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult Complete()
        {
            var result = IPuzzleService.Complete();
            return JResult(result);
        }
    }
}