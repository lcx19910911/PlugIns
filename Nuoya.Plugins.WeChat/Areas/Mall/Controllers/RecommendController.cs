using Nuoya.Plugins.WeChat.Filters;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;
using Nuoya.Plugins.WeChat.Controllers;
using Core.Web;

namespace Nuoya.Plugins.WeChat.Areas.Mall.Controllers
{
    /// <summary>
    /// 推荐控制器
    /// </summary>
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
        /// <returns></returns
        [LoginFilter]
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
        /// <param name="recommendCode">分类 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, int recommendCode)
        {
            var pagelist = IMallRecommendService.Get_RecommendPageList(pageIndex, pageSize, name, recommendCode);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
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
        [LoginFilter]
        public ActionResult Delete(string unids)
        {
            var model = IMallRecommendService.Delete_Recommend(unids);
            return JResult(model);
        }


        /// <summary>
        /// 获取推荐分类
        /// </summary>
        /// <returns></returns>
        public PartialViewResult HomeCategory()
        {
            var person = CacheHelper.Get<Person>("person");
            var categoryList = IMallRecommendService.Get_RecommendCategory(person.UNID);
            return PartialView(categoryList);
        }

        /// <summary>
        /// 获取推荐商品
        /// </summary>
        /// <returns></returns>
        public PartialViewResult HomeGoods()
        {
            var person = CacheHelper.Get<Person>("person");
            var goodsList = IMallRecommendService.Get_RecommendGoods(person.UNID);
            return PartialView(goodsList);
        }
    }
}