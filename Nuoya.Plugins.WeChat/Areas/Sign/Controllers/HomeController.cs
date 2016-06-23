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
            var person = CacheHelper.Get<Person>("person");
            if (user==null|| person==null)
                return Error();
            UserCenterModel model = new UserCenterModel();
            model.User = user;
            model.Score = IUserService.Find_PersonUserScore(person.UNID,user.OpenId);
            var signModel = IUserSignService.Get_LastSign(user.OpenId, person.UNID);
            if (signModel != null && signModel.SignDate == DateTime.Now.Date)
                model.TodayHadSign = true;
            model.SignNum = signModel == null ? 0 : (model.TodayHadSign?signModel.SignNum:0);
            model.SignDic = IUserSignService.Get_LastelyTenDaySign(user.OpenId,person.UNID);
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