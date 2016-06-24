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
            Repository.User user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return Error();
            int score = IUserService.Find_PersonUserScore(person.UNID, user.OpenId);
            var list = IUserService.Get_UserScore(user.OpenId, person.UNID, 1);
            if(list == null)
                return Error();
            else
                return View( new Tuple<int, Repository.User, List<Repository.ScoreDetails>>(score, user, list));
        }

        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public ActionResult LoadMore(int pageIndex=1)
        {
            Repository.User user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return Error();
            var result = IUserService.Get_UserScore(user.OpenId, person.UNID, pageIndex);
            return JResult(result);
        }
    }
}