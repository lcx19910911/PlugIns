using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using Nuoya.Plugins.WeChat.Filters;
using IService;
using Core.Web;
using MPUtil;
using MPUtil.UserMng;

namespace Nuoya.Plugins.WeChat.Areas.Puzzle.Controllers
{
    /// <summary>
    /// 拼图管理员控制器
    /// </summary>

    [LoginFilter]
    public class AdminController : PuzzleBaseController
    {
        public IPuzzleService IPuzzleService;

        public AdminController(IPuzzleService _IPuzzleService)
        {
            this.IPuzzleService = _IPuzzleService;
        }

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">昵称</param>
        /// <param name="openId"></param>
        /// <param name="createdTimeStart">发布日期起 - 搜索项</param>
        /// <param name="createdTimeEnd">发布日期止 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name,  DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            var pagelist = IPuzzleService.Get_PuzzlePageList(pageIndex, pageSize, name,createdTimeStart, createdTimeEnd);
            return JResult(pagelist);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult Add(Model.Puzzle model)
        {
            
            var result = IPuzzleService.Add_Puzzle(model);
            return JResult(result);
        }


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        [ValidateInput(false)]
        public JsonResult Update(Model.Puzzle model, string unid)
        {
            var result = IPuzzleService.Update_Puzzle(model, unid);
            return JResult(result);
        }

        /// <summary>
        /// 查找实体
        /// </summary>
        /// <param name="model"</param>
        /// <returns></returns>
        public JsonResult Find(string unid)
        {
            var result = IPuzzleService.Find_Puzzle(unid);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unid"></param>
        /// <returns></returns>
        public ActionResult Delete(string unids)
        {
            var effect = IPuzzleService.Delete_Puzzle(unids);
            return JResult(effect);
        }

    }
}