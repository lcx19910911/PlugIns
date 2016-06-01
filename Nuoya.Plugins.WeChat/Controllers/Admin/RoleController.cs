using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extension;
using Repository;
using Nuoya.Plugins.WeChat.Filters;

namespace Nuoya.Plugins.WeChat.Controllers
{
    [LoginFilter]
    public class RoleController : BaseController
    {

        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Add(Domain.Role.Add entity)
        {
            WebService.Add_Role(entity);
            return JResult(true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public JsonResult Delete(string unids)
        {
            var effect = WebService.Delete<Role>(unids);
            return JResult(effect);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Update(Domain.Role.Update entity)
        {
            WebService.Update<Domain.Role.Update, Role>(entity.UNID, entity);
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
            var pagelist = WebService.Get_RolePageList(pageIndex, pageSize, name).AutoMap<Role, Domain.Role.List>();
            return JResult(pagelist);
        }

        /// <summary>
        /// 获取部门选择项
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSelectItem(long roleFlag)
        {
            var listSelectItem = WebService.Get_RoleSelectItem(roleFlag);
            return JResult(listSelectItem);
        }
    }
}