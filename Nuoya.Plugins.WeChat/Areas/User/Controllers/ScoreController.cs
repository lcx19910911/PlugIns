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

namespace Nuoya.Plugins.WeChat.Areas.User.Controllers
{
    /// <summary>
    /// 用户积分
    /// </summary>
    public class ScoreController :UserBaseController
    {
        public IUserService IUserService;

        public ScoreController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var user = CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (user == null || person == null)
                return OAuthExpired();
            int score = IUserService.Find_PersonUserScore(person.UNID, user.openid);
            var list = IUserService.Get_UserScore(user.openid, person.UNID, 1);
            if(list == null)
                return OAuthExpired();
            else
                return View( new Tuple<int, Model.User, List<Model.ScoreDetails>>(score, new Model.User() {
                    NickName=user.nickname,
                    HeadImgUrl=user.headimgurl
                }, list));
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult LoadMore(int pageIndex=1)
        {
            var user = CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (user == null || person == null)
                return OAuthExpired();
            var result = IUserService.Get_UserScore(user.openid, person.UNID, pageIndex);
            return JResult(result);
        }
    }
}