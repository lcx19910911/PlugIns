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
    /// 商品控制器
    /// </summary>
    public class GoodsController : MallBaseController
    {

        public IMallGoodsService IMallGoodsService;
        public IUserService IUserService;

        public GoodsController(IMallGoodsService _IMallGoodsService, IUserService _IUserService)
        {
            this.IMallGoodsService = _IMallGoodsService;
            this.IUserService = _IUserService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
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
        /// <param name="categoryId">分类id - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string name,string categoryId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IMallGoodsService.Get_MallGoodsPageList(pageIndex, pageSize, name, categoryId, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult Add(Goods model,string DetailsImage,string DetailsSort)
        {
            var result = IMallGoodsService.Add_MallGoods(model, DetailsImage, DetailsSort);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult Update(Goods model, string DetailsImage, string DetailsSort)
        {
            var result = IMallGoodsService.Update_MallGoods(model, DetailsImage, DetailsSort);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult Find(string unid)
        {
            var result = IMallGoodsService.Find_MallGoods(unid);
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
            var model = IMallGoodsService.Delete_MallGoods(unids);
            return JResult(model);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult AllGoods(string categoryId)
        {
            Repository.User user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return OAuthExpired();

            var goodsList = IMallGoodsService.Get_GoodsListByCategoryId(categoryId, person.UNID);

            return View(goodsList);
        }

        /// <summary>
        /// 商品详细页
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Details(string unid)
        {
            Repository.User user = CacheHelper.Get<Repository.User>("user");
            var person = CacheHelper.Get<Person>("person");
            if (user == null || person == null)
                return OAuthExpired();

            ViewData["userScore"]=IUserService.Find_PersonUserScore(person.UNID, user.OpenId);
            var goods = IMallGoodsService.Find_MallGoods(unid);
            if(goods==null)
                return OAuthExpired();
            return View(goods);
        }
    }
}