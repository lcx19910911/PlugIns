using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    /// <summary>
    /// 微信墙
    /// </summary>
    public class WallController : BaseController
    {
        // GET: Wall
        public ActionResult Index()
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
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_WallPageList(pageIndex, pageSize, name, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }


        public ActionResult Detials()
        {
            return View();
        }
    }
}