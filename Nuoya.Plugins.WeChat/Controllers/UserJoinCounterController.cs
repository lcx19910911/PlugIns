using EnumPro;
using Nuoya.Plugins.WeChat.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IService;

namespace Nuoya.Plugins.WeChat.Controllers
{
    /// <summary>
    /// 用户参与记录控制器
    /// </summary>
    [LoginFilter]
    public class UserJoinCounterController : BaseController
    {

        
        public IUserJoinCounterService IUserJoinCounterService;

        public UserJoinCounterController(IUserJoinCounterService _IUserJoinCounterService)
        {
            this.IUserJoinCounterService = _IUserJoinCounterService;
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
        /// <param name="name">活动名 - 搜索项</param>
        /// <param name="openId">微信openid - 搜索项</param>
        /// <param name="targetCode">活动类型 - 搜索项</param>
        /// <param name="targetId">活动id - 搜索项</param>
        /// <param name="SN">sn吗 - 搜索项</param>
        /// <param name="prizeType">奖品等级 - 搜索项</param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string openId,int targetCode, string targetId,string sn,int prizeType, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IUserJoinCounterService.Get_UserJoinCounterPageList(pageIndex, pageSize, name, openId, targetCode, targetId, sn, prizeType, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 兑奖
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public JsonResult Cash(string unid)
        {
            var effect = IUserJoinCounterService.Cash(unid);
            return JResult(effect);
        }
    }
}