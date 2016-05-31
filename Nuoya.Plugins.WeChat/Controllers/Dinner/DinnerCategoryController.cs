using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    [LoginFilter]
    public class DinnerCategoryController : BaseController
    {
        // GET: DinnerCategory
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="groupName">分组名称 - 搜索项</param>
        /// <param name="keyValue">键值 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name)
        {
            var pagelist = WebService.Get_DinnerCategoryPageList(pageIndex, pageSize, name);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(DinnerCategory model)
        {
            var result = WebService.Add_DinnerCategory(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(DinnerCategory model, string unid)
        {
            var result = WebService.Update_DinnerCategory(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = WebService.Find_DinnerCategory(unid);
            return JResult(result);
        }
    
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = WebService.Delete_DinnerCategory(unids);
            return JResult(model);
        }


        /// <summary>
        /// 获取菜品分类选择项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelectItem(string cid)
        {
            var listSelectItem = WebService.Get_DinnerCategorySelectItem(cid);
            return JResult(listSelectItem);
        }
    }
}