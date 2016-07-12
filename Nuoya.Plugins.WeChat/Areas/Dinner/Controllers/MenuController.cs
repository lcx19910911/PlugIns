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
using MPUtil.UserMng;
using Service;

namespace Nuoya.Plugins.WeChat.Areas.Dinner.Controllers
{
    /// <summary>
    /// 点餐微信展示页面
    /// </summary>
    public class MenuController : DinnerBaseController
    {
        public ICategoryService IDinnerCategoryService;
        public IDinnerDishService IDinnerDishService;
        public IDinnerOrderService IDinnerOrderService;
        public IUserService IUserService;

        public MenuController(ICategoryService _IDinnerCategoryService, IDinnerDishService _IDinnerDishService, IDinnerOrderService _IDinnerOrderService, IUserService _IUserService)
        {
            this.IDinnerCategoryService = _IDinnerCategoryService;
            this.IDinnerDishService = _IDinnerDishService;
            this.IDinnerOrderService = _IDinnerOrderService;
            this.IUserService = _IUserService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="unid">店铺id</param>
        /// <param name="info">微信用户信息json化字符串</param>
        /// <returns>页面缓存1小时</returns>
        //[OAuthFilter]
        public ActionResult Index(string unid,string info)
        {

            var userInfoCache = CookieHelper.GetCurrentWxUser();

            if (!string.IsNullOrEmpty(info) && userInfoCache == null)
            {
                WXUser entity = info.DeserializeJson<WXUser>();
                if (entity != null)
                {                 
                    //更新数据
                    IUserService.Update_User(entity);
                    CookieHelper.CreateWxUser(entity);
                }
            }
            if (userInfoCache == null)
                return OAuthExpired();

            //判断是否已有订单
            ViewBag.ExistsOrder = this.Request.Cookies["had"] == null ? false : (string.IsNullOrEmpty(this.Request.Cookies["had"].Value) ? false : true);

            //店铺id
            if (string.IsNullOrEmpty(unid))
                unid = CookieHelper.GetCurrentShopId();
            else
                CookieHelper.CreateShopId(unid);

            if (string.IsNullOrEmpty(unid))
            {
                return OAuthExpired();
            }
            else
            {
                var item = IDinnerCategoryService.Get_ItemByShopId(unid);
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
        /// <param name="orderInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderSubmit(string orderInfo)
        {
            return JResult(IDinnerOrderService.Add_DinnerOrder(orderInfo));
        }
        #endregion

    }
}