﻿using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class ScratchCardController : BaseController
    {
        // GET: ScratchCard
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
        public JsonResult GetPageList(int pageIndex, int pageSize, string title, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = WebService.Get_ScratchCardPageList(pageIndex, pageSize, title, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns>0:保存出错 -1:开始时间小于当前时间  -2:结束时间必须大于当前时间和开始时间  -3:个人抽奖次数总计要大于或等于每天次数限制</returns>
        public JsonResult Add(Domain.ScratchCard.Update model)
        {
            var result = WebService.Add_ScratchCard(model);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = WebService.Find_ScratchCard(unid);
            return JResult(result);
        }
    }
}