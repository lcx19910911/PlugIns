using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using Nuoya.Plugins.WeChat.Filters;
using IService;
using Core.Web;
using MPUtil;
using MPUtil.UserMng;

namespace Nuoya.Plugins.WeChat.Controllers
{

    public class UserController : BaseController
    {
        public IUserService IUserService;

        public UserController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }

        [LoginFilter]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="groupName">分组名称 - 搜索项</param>
        /// <param name="keyValue">键值 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IUserService.Get_UserPageList(pageIndex, pageSize, name, openId, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 授权保存code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">原来请求地址</param>
        /// <returns></returns>
        public ActionResult DoAuth(string code, string state)
        {
            Core.Util.LogHelper.WriteInfo(code);
            Core.Util.LogHelper.WriteInfo(state);
            CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () =>
            {
                //获取access-token
                var access_tokenHash=BaseFunctions.GetAccessToken(code, Params.AppId, Params.AppSecret);
                if (access_tokenHash != null&&access_tokenHash["access_token"]!=null)
                {
                    string access_token = access_tokenHash["access_token"].ToString();
                    Core.Util.LogHelper.WriteInfo(access_token);
                    string openId = access_tokenHash["openid"].ToString();
                    //获取用户信息
                    Core.Util.LogHelper.WriteInfo(openId);
                    var userInfo = UserFunction.GetInfo(access_token, openId);
                    if (userInfo != null && userInfo["user"] != null)
                    {
                        //保存下来
                        WXUser user = (WXUser)userInfo["user"];
                        IUserService.Update_User(user);
                        return openId;
                    }
                    else
                        return "";
                }
                return  "";
            });
            return Redirect(state);
        }

    }
}