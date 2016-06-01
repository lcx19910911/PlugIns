using Core.Extensions;
using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class DinnerMenuController : BaseController
    {
        // GET: Customer
        public ActionResult Index(string info, string shopId)
        {
            //接收微信用户数据
            if (!string.IsNullOrEmpty(info))
            {
                User model = info.DeserializeJson<User>();
                if (model != null)
                {
                    //更新数据
                    WebService.Update_User(model);
                    CacheHelper.Get<string>("dinner-openId", CacheTimeOption.TwoHour, () =>
                    {
                        return model.OpenId;
                    });
                }
            }
            //判断是否已有订单
            ViewBag.ExistsOrder = Client.Request.Cookies["had"] == null ? false : (string.IsNullOrEmpty(Client.Request.Cookies["had"].Value) ? false : true);
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
                var item = WebService.Get_ItemByShopId(shopId);
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
            return JResult(WebService.Get_DishListByCategoryId(dc_id));
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
            var model=WebService.Get_CurrentOrder();
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
            return JResult(WebService.Add_DinnerOrder(info));
        }
        #endregion

    }
}