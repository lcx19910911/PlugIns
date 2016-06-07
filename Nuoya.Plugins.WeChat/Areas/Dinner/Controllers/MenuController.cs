using Core.Extensions;
using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using IService;
using Nuoya.Plugins.WeChat.Controllers;
using Nuoya.Plugins.WeChat.Filters;

namespace Nuoya.Plugins.WeChat.Areas.Dinner.Controllers
{
    public class MenuController : BaseController
    {


        public IDinnerCategoryService IDinnerCategoryService;
        public IDinnerDishService IDinnerDishService;
        public IDinnerOrderService IDinnerOrderService;
        public IUserService IUserService;

        public MenuController(IDinnerCategoryService _IDinnerCategoryService, IDinnerDishService _IDinnerDishService, IDinnerOrderService _IDinnerOrderService, IUserService _IUserService)
        {
            this.IDinnerCategoryService = _IDinnerCategoryService;
            this.IDinnerDishService = _IDinnerDishService;
            this.IDinnerOrderService = _IDinnerOrderService;
            this.IUserService = _IUserService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shopId"></param>
        /// <returns></returns>
        [OAuthFilter]
        public ActionResult Index(string shopId)
        {
            //判断是否已有订单
            ViewBag.ExistsOrder = this.Request.Cookies["had"] == null ? false : (string.IsNullOrEmpty(this.Request.Cookies["had"].Value) ? false : true);
            //店铺id
            shopId = CacheHelper.Get<string>("dinner-shopId", CacheTimeOption.TwoHour, () =>
            {
                return shopId;
            });

            if (string.IsNullOrEmpty(shopId))
            {
                return View("/base/Error");
            }
            else
            {
                var item = IDinnerCategoryService.Get_ItemByShopId(shopId);
                return View(item);
            }
        }

        /// <summary>
        /// 根据分类获取菜品
        /// </summary>
        /// <param name="dc_id"></param>
        /// <returns></returns>
        public ActionResult GetDish(string dc_id)
        {
            return JResult(IDinnerDishService.Get_DishListByCategoryId(dc_id));
        }

        #region 菜品详细页  和 已点菜单查看页
        /// <summary>
        /// 菜品详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            return View();
        }
        /// <summary>
        /// 已点菜单（两小时内的菜单）
        /// </summary>
        /// <returns></returns>
        public ActionResult Haves()
        {
            var model = IDinnerOrderService.Get_CurrentOrder();
            return View(model);
        }
        #endregion

        #region 点餐确认页
        /// <summary>
        /// 点餐列表
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderList()
        {
            return View();
        }

        /// <summary>
        /// 确认订单提交给后台
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderSubmit(string info)
        {
            return JResult(IDinnerOrderService.Add_DinnerOrder(info));
        }
        #endregion

    }
}