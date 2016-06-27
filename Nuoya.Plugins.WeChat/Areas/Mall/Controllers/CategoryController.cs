using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Nuoya.Plugins.WeChat.Controllers;

namespace Nuoya.Plugins.WeChat.Areas.Mall.Controllers
{
    /// <summary>
    /// 分类控制器
    /// </summary>
    [LoginFilter]
    public class CategoryController : MallBaseController
    {
        
        public ICategoryService ICategoryService;

        public CategoryController(ICategoryService _ICategoryService)
        {
            this.ICategoryService = _ICategoryService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name)
        {
            var pagelist = ICategoryService.Get_DinnerCategoryPageList(pageIndex, pageSize, name);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(Category model)
        {
            var result = ICategoryService.Add_DinnerCategory(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(Category model, string unid)
        {
            var result = ICategoryService.Update_DinnerCategory(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = ICategoryService.Find_DinnerCategory(unid);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = ICategoryService.Delete_DinnerCategory(unids);
            return JResult(model);
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelectItem(string cid)
        {
            var listSelectItem = ICategoryService.Get_DinnerCategorySelectItem(cid);
            return JResult(listSelectItem);
        }
    }
}