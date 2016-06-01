using EnumPro;
using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    [LoginFilter]
    public class DinnerOrderController : BaseController
    {
        // GET: DinnerOrder
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
        public JsonResult GetPageList(int pageIndex, int pageSize, int state, string orderNum, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_DinnerOrderPageList(pageIndex, pageSize, state, orderNum, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }


        /// <summary>
        /// 确认的订单
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Confirm(string unid)
        {
            var result = WebService.Confirm_DinnerOrder(unid);
            return JResult(result);
        }

        /// <summary>
        /// 订单无效
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Invalid(string unid)
        {
            var model = WebService.Invalid_DinnerOrder(unid);
            return JResult(model);
        }
    }
}

