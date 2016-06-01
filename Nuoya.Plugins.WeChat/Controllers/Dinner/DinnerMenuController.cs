using Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class DinnerMenuController : BaseController
    {
        // GET: Customer
        public ActionResult Index(string openId,string shopId)
        {
            ViewBag.ExistsOrder = Client.Request.Cookies["had"] == null ? false : (string.IsNullOrEmpty(Client.Request.Cookies["had"].Value) ? false:true);

            CacheHelper.Get<string>("dinner-openId", CacheTimeOption.TwoHour, () => {
                return openId;
            });

            shopId=CacheHelper.Get<string>("dinner-shopId", CacheTimeOption.TwoHour, () => {
                return shopId;
            });

            
            var item = WebService.Get_ItemByShopId(shopId);
            return View(item);
        }

        public ActionResult GetDish(string dc_id)
        {
            return JResult(WebService.Get_DishListByCategoryId(dc_id));
        }

        #region 菜品详细页  和 已点菜单查看页
        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Haves()
        {
            var model=WebService.Get_CurrentOrder();
            return View(model);
        }
        #endregion

        #region 点餐确认页
        public ActionResult OrderList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderSubmit(string info)
        {
            return JResult(WebService.Add_DinnerOrder(info));
        }
        #endregion

    }
}