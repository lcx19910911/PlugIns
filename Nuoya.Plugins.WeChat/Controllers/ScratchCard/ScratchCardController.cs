using Core;
using Core.Extensions;
using Core.Web;
using Nuoya.Plugins.WeChat.Filters;
using Repository;
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
        /// <param name="groupName">分组名称 - 搜索项</param>
        /// <param name="keyValue">键值 - 搜索项</param>
        /// <returns></returns>
        [LoginFilter]
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
        [LoginFilter]
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
        [LoginFilter]
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
        [LoginFilter]
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
        public ActionResult Details(string unid, string info)
        {
            string openId = string.Empty;
            //接收微信用户数据
            if (!string.IsNullOrEmpty(info))
            {
                User model = info.DeserializeJson<User>();
                if (model != null)
                {
                    //更新数据
                    WebService.Update_User(model);
                    CacheHelper.Get<string>("scra-openId", CacheTimeOption.TwoHour, () =>
                    {
                        openId = model.OpenId;
                        return model.OpenId;
                    });
                }
            }
            if (string.IsNullOrEmpty(openId))
            {
                return View("/base/Error");
            }
            else
            {
                var item = WebService.Show_ScratchCard(unid, openId);
                return View(item);
            }
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Do(string unid)
        {         
            var result = WebService.Do_ScratchCard(unid);
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
            var effect = WebService.Delete_ScratchCard(unids);
            return JResult(effect);
        }
    }
}