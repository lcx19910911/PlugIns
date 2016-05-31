﻿using Core;
using Core.Web;
using Nuoya.Plugins.WeChat.Filters;
using Server;
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
        /// <returns></returns>
        public JsonResult Add(Domain.ScratchCard.Update model)
        {
            var result = WebService.Add_ScratchCard(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Update(Domain.ScratchCard.Update model,string unid)
        {
            var result = WebService.Update_ScratchCard(model, unid);
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


        /// <summary>
        /// 明细(接收openid)
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        //[OAuthFilter]
        public ActionResult Details(string unid,string openId)
        {
            //保存openid
            CacheHelper.Get<string>("openId", CacheTimeOption.TwoHour, () => {
                return openId;
                //return "12345678";
            });
            var model = WebService.Show_ScratchCard(unid, openId);
            return View(model);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Do(string unid)
        {         
            var model = WebService.Do_ScratchCard(unid);
            return JResult(model);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var model = WebService.Delete_ScratchCard(unids);
            return JResult(model);
        }
    }
}