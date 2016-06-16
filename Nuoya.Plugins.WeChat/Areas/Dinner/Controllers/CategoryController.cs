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
    /// 分类控制器
    /// </summary>
    [LoginFilter]
    public class CategoryController : DinnerBaseController
    {
        
        public IDinnerCategoryService IDinnerCategoryService;

        public CategoryController(IDinnerCategoryService _IDinnerCategoryService)
        {
            this.IDinnerCategoryService = _IDinnerCategoryService;
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
        /// <param name="name">名称 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name)
        {
            var pagelist = IDinnerCategoryService.Get_DinnerCategoryPageList(pageIndex, pageSize, name);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(Category model)
        {
            var result = IDinnerCategoryService.Add_DinnerCategory(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(Category model, string unid)
        {
            var result = IDinnerCategoryService.Update_DinnerCategory(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = IDinnerCategoryService.Find_DinnerCategory(unid);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = IDinnerCategoryService.Delete_DinnerCategory(unids);
            return JResult(model);
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelectItem(string cid)
        {
            var listSelectItem = IDinnerCategoryService.Get_DinnerCategorySelectItem(cid);
            return JResult(listSelectItem);
        }
    }
}