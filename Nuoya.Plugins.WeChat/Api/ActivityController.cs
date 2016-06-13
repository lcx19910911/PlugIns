using Core.Model;
using Domain.API;
using IService;
using Nuoya.Plugins.WeChat.Filters;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Nuoya.Plugins.WeChat.Api
{
    /// <summary>
    /// 活动接口
    /// </summary>
    [LoginFilter]
    public class ActivityController : ApiBaseController
    {

        public ScratchCardService IScratchCardService=new ScratchCardService();
        public DinnerShopService IDinnerShopService = new DinnerShopService();

        //public ActivityController(IScratchCardService _IScratchCardService, IDinnerShopService _IDinnerShopService)
        //{
        //    this.IScratchCardService = _IScratchCardService;
        //    this.IDinnerShopService = _IDinnerShopService;
        //}

        /// <summary>
        /// 获取用户所有的刮刮卡
        /// </summary>
        /// <returns></returns>
        [ActionName("GetScratchCardList")]
        public async Task<WebResult<List<ScratchCardResult>>> GetScratchCardList()
        {
            return await Task.Run(() =>
            {
                return Result(IScratchCardService.Get_AllScratchCardList());
            });               
        }

        /// <summary>
        /// 获取用户的店铺
        /// </summary>
        /// <returns></returns>
        [ActionName("GetDinnerShopList")]
        public WebResult<List<ApiDinnerShopModel>> GetDinnerShopList()
        {
            return Result(IDinnerShopService.Get_DinnerShopList());
        }
    }
}
