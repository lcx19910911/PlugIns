using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class UserJoinCounterController : BaseController
    {
        // GET: Prize
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">活动名</param>
        /// <param name="openId">微信openid</param>
        /// <param name="code">活动类型</param>
        /// <param name="SN">中奖码</param>
        /// <param name="createdTimeStart">中奖时间开始</param>
        /// <param name="createdTimeEnd">中奖时间结束</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name, string openId,int targetCode, string targetId,string sn,int prizeType, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_UserJoinCounterPageList(pageIndex, pageSize, name, openId, targetCode, targetId, sn, prizeType, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 兑奖
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public JsonResult Cash(string unid)
        {
            var effect = WebService.Cash(unid);
            return JResult(effect);
        }
    }
}