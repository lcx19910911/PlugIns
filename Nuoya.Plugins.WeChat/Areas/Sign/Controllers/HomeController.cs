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

namespace Nuoya.Plugins.WeChat.Areas.Sign.Controllers
{
    /// <summary>
    /// 签到
    /// </summary>
    public class HomeController : SignBaseController
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
            Repository.User user=CacheHelper.Get<Repository.User>("user");
            if (user==null)
                return Error();
            UserCenterModel model = new UserCenterModel();
            model.User = user;
            var scoreModel = IUserService.Find_User(user.OpenId);
            model.Score = scoreModel == null ? 0 : scoreModel.Score;
            var signModel = IUserSignService.Get_LastSign(user.OpenId);
            model.SignNum = signModel == null ? 0 : signModel.SignNum;
            model.SignDic = IUserSignService.Get_LastelyTenDaySign(user.OpenId);
            return View(model);
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult Sign(string openId)
        {
            var result = IUserSignService.User_Sign(openId);
            return JResult(result);
        }
    }
}