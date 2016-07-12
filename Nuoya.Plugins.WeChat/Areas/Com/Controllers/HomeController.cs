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
using Domain.Com;
using Core.Model;

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

        /// <summary>
        /// 获取平台的活动
        /// </summary>
        /// <returns></returns>
        public ActionResult GetComActivityList(int type)
        {
            string url = string.Format("{0}{1}?cid={2}", Params.ComUrl, "api/CompanyComContentExt/GetComContent", this.LoginUser.ComId);
            string pageResult=WebHelper.GetPage(url, null, "GET", null, System.Text.Encoding.UTF8);

            if (string.IsNullOrEmpty(pageResult))
                return _505();

            var result = pageResult.DeserializeJson<ComResult<ActivityModel>>();
            if(result == null|| result.code != 100|| result.data==null)
                return _505();
            //排除自身
            result.data.RemoveAll(x => x.FK_ApplyID == type);
            var pageListResult=new PageList<ActivityModel>(result.data, 1, result.data.Count, result.data.Count);
            return JResult(pageListResult);
        }
    }
}