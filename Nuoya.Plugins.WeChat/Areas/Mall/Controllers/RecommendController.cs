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
    /// 推荐控制器
    /// </summary>
    [LoginFilter]
    public class RecommendController : MallBaseController
    {

        public IMallRecommendService IMallRecommendService;

        public RecommendController(IMallRecommendService _IMallRecommendService)
        {
            this.IMallRecommendService = _IMallRecommendService;          
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
        /// <param name="name">商品名 - 搜索项</param>
        /// <param name="targetCode">分类 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, int targetCode)
        {
            var pagelist = IMallRecommendService.Get_RecommendPageList(pageIndex, pageSize, name, targetCode);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Add(Recommend model)
        {
            var result = IMallRecommendService.Add_Recommend(model);
            return JResult(result);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = IMallRecommendService.Delete_Recommend(unids);
            return JResult(model);
        }
    }
}