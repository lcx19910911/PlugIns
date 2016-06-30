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
    /// <summary>
    /// 微信用户控制器
    /// </summary>
    [LoginFilter]
    public class WXUserController : BaseController
    {
        public IUserService IUserService;

        public WXUserController(IUserService _IUserService)
        {
            this.IUserService = _IUserService;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">昵称</param>
        /// <param name="openId"></param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string openId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IUserService.Get_UserPageList(pageIndex, pageSize, name, openId, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }
    }
}