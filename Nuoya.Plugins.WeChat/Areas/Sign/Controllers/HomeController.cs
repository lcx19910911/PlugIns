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
            var user =CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (user==null|| person==null)
                return OAuthExpired();
            UserCenterModel model = new UserCenterModel();
            model.User = new Model.User()
            {
                HeadImgUrl=user.headimgurl,
                NickName=user.nickname
            };
            model.Score = IUserService.Find_PersonUserScore(person.UNID,user.openid);
            var signModel = IUserSignService.Get_LastSign(user.openid, person.UNID);
            if (signModel != null && signModel.SignDate == DateTime.Now.Date)
                model.TodayHadSign = true;
            model.SignNum = signModel == null ? 0 : (model.TodayHadSign?signModel.SignNum:0);
            model.SignDic = IUserSignService.Get_LastelyTenDaySign(user.openid,person.UNID);
            return View(model);
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