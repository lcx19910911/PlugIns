using EnumPro;
using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Nuoya.Plugins.WeChat.Controllers;

namespace Nuoya.Plugins.WeChat.Areas.Dinner.Controllers
{
    /// <summary>
    /// 订单
    /// </summary>
    [LoginFilter]
    public class OrderController : BaseController
    {
        public IDinnerOrderService IDinnerOrderService;

        public OrderController(IDinnerOrderService _IDinnerOrderService)
        {
            this.IDinnerOrderService = _IDinnerOrderService;
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!this.LoginUser.IsChildren)
                return Redirect("/home/index");
            else
                return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="state">状态 - 搜索项</param>
        /// <param name="orderNum">订单号 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, int state, string orderNum, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IDinnerOrderService.Get_DinnerOrderPageList(pageIndex, pageSize, state, orderNum, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }


        /// <summary>
        /// 确认的订单
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Confirm(string unid)
        {
            var result = IDinnerOrderService.Confirm_DinnerOrder(unid);
            return JResult(result);
        }

        /// <summary>
        /// 订单无效
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Invalid(string unid)
        {
            var model = IDinnerOrderService.Invalid_DinnerOrder(unid);
            return JResult(model);
        }
    }
}

