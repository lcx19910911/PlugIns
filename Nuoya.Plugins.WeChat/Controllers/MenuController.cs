using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extension;
using Repository;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class MenuController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Add(Domain.Menu.Add entity)
        {
            string result=WebService.Add_Menu(entity);
            return JResult(result);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public JsonResult Delete(string unids)
        {
            var effect = WebService.Delete<Menu>(unids);
            return JResult(effect);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Update(Domain.Menu.Update entity)
        {
            WebService.Update<Domain.Menu.Update, Menu>(entity.UNID, entity);
            return JResult(true);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="groupName">分组名称 - 搜索项</param>
        /// <param name="keyValue">键值 - 搜索项</param>
        /// <returns></returns>
        public JsonResult GetPageList(int pageIndex, int pageSize, string name)
        {
            var pagelist = WebService.Get_MenuPageList(pageIndex, pageSize, name).AutoMap<Menu, Domain.Menu.List>();
           
            return JResult(pagelist);
        }

        /// <summary>
        /// 获取菜单ZTree
        /// </summary>
        /// <returns></returns>
        public JsonResult GetZTreeNodes()
        {
            var list = WebService.GetAll<Menu>();
            var group = list.GroupBy(x => x.ParentID).ToList();
            var ztreeNodes = WebService.GetZTreeChildren(null, group);
            return JResult(ztreeNodes);
        }

        /// <summary>
        /// 获取菜单的FlagZTree
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFlagZTreeNodes()
        {
            var list = WebService.GetAll<Menu>();
            var group = list.GroupBy(x => x.ParentID).ToList();
            var ztreeNodes = WebService.GetZTreeFlagChildren(null, group);
            return JResult(ztreeNodes);
        }


        /// <summary>
        /// 获取操作属性节点
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public JsonResult GetOperateZtreeNodes(string roleId)
        {
            var list = WebService.GetAll<Menu>();
            var group = list.GroupBy(x => x.ParentID).ToList();


            var ztreeNodes = WebService.GetZTreeOperateChildren(null, group);

            return JResult(ztreeNodes);
        }

        public PartialViewResult PartialMenu()
        {
            var list = WebService.GetAll<Menu>().OrderBy(x => x.Sort);
            var group = list.GroupBy(x => x.ParentID).ToList();
            var menuList = WebService.GetChildrenMenu(null, group);
            return PartialView(menuList);
        }
    }
}