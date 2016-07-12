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
    /// 微信用户中心
    /// </summary>
    public class HomeController : UserBaseController
    {
        public IUserService IUserService;

        public IUserSignService IUserSignService;
        public IPersonService IPersonService;

        public HomeController(IUserService _IUserService, IUserSignService _IUserSignService, IPersonService _IPersonService)
        {
            this.IUserService = _IUserService;
            this.IUserSignService = _IUserSignService;
            this.IPersonService = _IPersonService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string info, string comId)
        {
            //接收微信用户数据
            var userInfoCache = CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (person == null && !string.IsNullOrEmpty(comId))
            {
                var personInfo = IPersonService.Get_ByComId(comId.GetInt());
                CookieHelper.CreatePeople(personInfo);
            }
            UserCenterModel model = new UserCenterModel();
            if (!string.IsNullOrEmpty(info) && userInfoCache == null)
            {
                WXUser entity = info.DeserializeJson<WXUser>();
                if (entity != null)
                {
                    model.User = new Repository.User()
                    {
                        NickName = entity.nickname,
                        HeadImgUrl = entity.headimgurl
                    };
                    //更新数据
                    IUserService.Update_User(entity);
                    CookieHelper.CreateWxUser(entity);
                }
            }

            model.User = new Repository.User()
            {
                HeadImgUrl = userInfoCache.headimgurl,
                NickName = userInfoCache.nickname
            };
            model.Score = IUserService.Find_PersonUserScore(person.UNID, userInfoCache.openid);
            var signModel = IUserSignService.Get_LastSign(userInfoCache.openid, person.UNID);
            model.SignNum = signModel == null ? 0 : (signModel.SignDate == DateTime.Now.Date || signModel.SignDate == DateTime.Now.AddDays(-1).Date ? signModel.SignNum : 0);

            return View(model);
        }
    }
}