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
    /// 签到
    /// </summary>
    public class HomeController : UserBaseController
    {
        public IUserService IUserService;

        public IUserSignService IUserSignService;

        public HomeController(IUserService _IUserService, IUserSignService _IUserSignService)
        {
            this.IUserService = _IUserService;
            this.IUserSignService = _IUserSignService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string info)
        {
            //接收微信用户数据
            if (!string.IsNullOrEmpty(info))
            {
                WXUser entity = info.DeserializeJson<WXUser>();
                if (entity != null)
                {
                    //更新数据
                    IUserService.Update_User(entity);
                    CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () =>
                    {
                        return entity.openid;
                    });

                    UserCenterModel model = new UserCenterModel();
                    model.User = new Repository.User()
                    {
                        NickName = entity.nickname,
                        HeadImgUrl = entity.headimgurl
                    };
                    model.Score = (int)IUserService.Find_User(entity.openid)?.Score;
                    model.SignNum = (int)IUserSignService.Get_LastSign(entity.openid)?.SignNum;

                    return View(model);
                }
            }
            return Error();
        }
    }
}