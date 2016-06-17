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

namespace Nuoya.Plugins.WeChat.Areas.Sign.Controllers
{
    /// <summary>
    /// 签到
    /// </summary>
    public class HomeController : SignBaseController
    {
        public IScratchCardService IScratchCardService;
        public IUserService IUserService;

        public HomeController(IScratchCardService _IScratchCardService, IUserService _IUserService)
        {
            this.IScratchCardService = _IScratchCardService;
            this.IUserService = _IUserService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
       // [LoginFilter]
        public ActionResult Index()
        {
            //if (this.LoginUser.IsChildren)
            //    return Redirect("/home/index");
            //else
                return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string title, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IScratchCardService.Get_ScratchCardPageList(pageIndex, pageSize, title, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }


        /// <summary>
        /// 明细(info)
        /// </summary>
        /// <param name="unid"></param
        /// <param name="info">用户信息</param>
        /// <returns></returns>
        public ActionResult Details(string unid,string info)
        {
            //接收微信用户数据
            if (!string.IsNullOrEmpty(info))
            {
                WXUser model = info.DeserializeJson<WXUser>();
                if (model != null)
                {
                    //更新数据
                    IUserService.Update_User(model);
                    CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () =>
                    {
                        return model.openid;
                    });
                }
            }
            var item = IScratchCardService.Show_ScratchCard(unid);
            return View(item);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Do(string unid)
        {
            var result = IScratchCardService.Do_ScratchCard(unid);
            return JResult(result);
        }
    }
}