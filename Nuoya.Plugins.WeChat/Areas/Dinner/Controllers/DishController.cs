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
    /// 菜品控制器
    /// </summary>
    [LoginFilter]
    public class DishController : BaseController
    {

        public IDinnerDishService IDinnerDishService;

        public DishController(IDinnerDishService _IDinnerDishService)
        {
            this.IDinnerDishService = _IDinnerDishService;          
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!this.LoginUser.IsChildren)
                return RedirectToAction("/home/index");
            else
                return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">菜品名 - 搜索项</param>
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name,string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IDinnerDishService.Get_DinnerDishPageList(pageIndex, pageSize, name, categoryId, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(DinnerDish model)
        {
            var result = IDinnerDishService.Add_DinnerDish(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(DinnerDish model, string unid)
        {
            var result = IDinnerDishService.Update_DinnerDish(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = IDinnerDishService.Find_DinnerDish(unid);
            return JResult(result);
        }
    
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = IDinnerDishService.Delete_DinnerDish(unids);
            return JResult(model);
        }
    }
}