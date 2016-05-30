using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class DinnerShopController : BaseController
    {
        // GET: DinnerShop
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
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_DinnerShopPageList(pageIndex, pageSize, name, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(Domain.DinnerShop.Update model)
        {
            var result = WebService.Add_DinnerShop(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(Domain.DinnerShop.Update model, string unid)
        {
            var result = WebService.Update_DinnerShop(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = WebService.Find_DinnerShop(unid);
            return JResult(result);
        }

        /// <summary>
        /// 禁用
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Disable(string unids)
        {
            var result = WebService.Disable_DinnerShop(unids);
            return JResult(result);
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Enabled(string unids)
        {
            var result = WebService.Enable_DinnerShop(unids);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = WebService.Delete_DinnerShop(unids);
            return JResult(model);
        }
    }
}