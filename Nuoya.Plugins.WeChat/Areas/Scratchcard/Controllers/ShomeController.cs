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
using Nuoya.Plugins.WeChat.Controllers;
using Core.AuthAPI;

namespace Nuoya.Plugins.WeChat.Areas.Scratchcard.Controllers
{
    public class ShomeController : BaseController
    {
        public IScratchCardService IScratchCardService;
        public IUserService IUserService;

        public ShomeController(IScratchCardService _IScratchCardService, IUserService _IUserService)
        {
            this.IScratchCardService = _IScratchCardService;
            this.IUserService = _IUserService;
        }


        [LoginFilter]
        public ActionResult Index()
        {
            if (this.LoginUser.IsChildren)
                return Redirect("/home/index");
            else
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
        [LoginFilter]
        public JsonResult GetPageList(int pageIndex, int pageSize, string title, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IScratchCardService.Get_ScratchCardPageList(pageIndex, pageSize, title, createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult Add(Domain.ScratchCard.Update model)
        {
            var result = IScratchCardService.Add_ScratchCard(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [LoginFilter]
        public JsonResult Update(Domain.ScratchCard.Update model, string unid)
        {
            var result = IScratchCardService.Update_ScratchCard(model, unid);
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
            var result = IScratchCardService.Find_ScratchCard(unid);
            return JResult(result);
        }


        /// <summary>
        /// 明细(接收openid)
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        [OAuthFilter]
        public ActionResult Details(string unid)
        {
            var item = IScratchCardService.Show_ScratchCard(unid);
            return View(item);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Do(string unid)
        {
            var result = IScratchCardService.Do_ScratchCard(unid);
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
            var effect = IScratchCardService.Delete_ScratchCard(unids);
            return JResult(effect);
        }
    }
}