using Core;
using Core.Extensions;
using Core.Web;
using Nuoya.Plugins.WeChat.Filters;
using Repository;
using IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MPUtil.UserMng;
using Domain.User;

namespace Nuoya.Plugins.WeChat.Areas.Com.Controllers
{
    /// <summary>
    /// 获取平台信息
    /// </summary>
    [LoginFilter]
    public class HomeController : ComBaseController
    {
        public IMallRecommendService IMallRecommendService;
        public IMallGoodsService IMallGoodsService;

        public HomeController(IMallRecommendService _IMallRecommendService, IMallGoodsService _IMallGoodsService)
        {
            this.IMallRecommendService = _IMallRecommendService;
            this.IMallGoodsService = _IMallGoodsService;
        }


        //public ActionResult GetComActivity()
        //{

        //}
    }
}