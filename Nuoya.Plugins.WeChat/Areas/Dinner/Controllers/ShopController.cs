using Nuoya.Plugins.WeChat.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Nuoya.Plugins.WeChat.Controllers;
using EnumPro;

namespace Nuoya.Plugins.WeChat.Areas.Dinner.Controllers
{
    /// <summary>
    /// 店铺控制器
    /// </summary>
    [LoginFilter]
    public class ShopController : BaseController
    {

        public IDinnerShopService IDinnerShopService;

        public ShopController(IDinnerShopService _IDinnerShopService)
        {
            this.IDinnerShopService = _IDinnerShopService;
        }
        

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (this.LoginUser.IsChildren)
                return Redirect("/home/index");
            else
                return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">门店名 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
           
            var pagelist = IDinnerShopService.Get_DinnerShopPageList(pageIndex, pageSize, name, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(Domain.DinnerShop.Update model)
        {
            var result = IDinnerShopService.Add_DinnerShop(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(Domain.DinnerShop.Update model, string unid)
        {
            var result = IDinnerShopService.Update_DinnerShop(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = IDinnerShopService.Find_DinnerShop(unid);
            return JResult(result);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Disable(string unids)
        {
            var result = IDinnerShopService.Disable_DinnerShop(unids);
            return JResult(result);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Enabled(string unids)
        {
            var result = IDinnerShopService.Enable_DinnerShop(unids);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = IDinnerShopService.Delete_DinnerShop(unids);
            return JResult(model);
        }
    }
}