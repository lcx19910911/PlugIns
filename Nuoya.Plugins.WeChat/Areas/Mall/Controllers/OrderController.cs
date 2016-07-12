using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Nuoya.Plugins.WeChat.Controllers;
using Core.Web;
using Service;

namespace Nuoya.Plugins.WeChat.Areas.Mall.Controllers
{
    /// <summary>
    /// 订单控制器
    /// </summary>
    public class OrderController : MallBaseController
    {
        
        public IMallOrderService IMallOrderService;

        public OrderController(IMallOrderService _IMallOrderService)
        {
            this.IMallOrderService = _IMallOrderService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">商品名 - 搜索项</param>
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize,string unid, string name, string nickName, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IMallOrderService.Get_OrderPageList(pageIndex, pageSize, unid, name, nickName, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(GoodsOrder model)
        {
            var result = IMallOrderService.Add_Order(model);
            return JResult(result);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var user = CookieHelper.GetCurrentWxUser();
            var person = CookieHelper.GetCurrentPeople();
            if (user == null || person == null)
                return OAuthExpired();

            var orderList = IMallOrderService.Get_AllOrderList(user.openid,person.UNID);
            return View(orderList);
        }
    }
}