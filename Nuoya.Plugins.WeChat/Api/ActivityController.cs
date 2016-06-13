using Core.Model;
using Domain.API;
using IService;
using Nuoya.Plugins.WeChat.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nuoya.Plugins.WeChat.Api
{
    /// <summary>
    /// 活动接口
    /// </summary>
    //[LoginFilter]
    public class ActivityController : ApiBaseController
    {

        public IScratchCardService IScratchCardService;
        public IDinnerShopService IDinnerShopService;

        public ActivityController(IScratchCardService _IScratchCardService, IDinnerShopService _IDinnerShopService)
        {
            this.IScratchCardService = _IScratchCardService;
            this.IDinnerShopService = _IDinnerShopService;
        }

        /// <summary>
        /// 获取用户所有的刮刮卡
        /// </summary>
        /// <returns></returns>
        public WebResult<List<ScratchCardResult>> GetScratchCardList()
        {
            return  Result(IScratchCardService.Get_AllScratchCardList());
        }

        /// <summary>
        /// 获取用户的店铺
        /// </summary>
        /// <returns></returns>
        public WebResult<List<ApiDinnerShopModel>> GetDinnerShopList()
        {
            return Result(IDinnerShopService.Get_DinnerShopList());
        }
    }
}
