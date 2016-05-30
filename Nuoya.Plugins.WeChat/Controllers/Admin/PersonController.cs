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
    public class PersonController : BaseController
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
        public JsonResult Add(Domain.Person.Add entity, string password)
        {
            WebService.Add_Person(entity, password);
            return JResult(true);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="unids"></param>
        /// <returns></returns>
        public JsonResult Delete(string unids)
        {
            var effect = WebService.Delete<Person>(unids);
            return JResult(effect);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult Update(Domain.Person.Update entity, string password)
        {
            WebService.Update_Person(entity.UNID, entity, password);
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
        public JsonResult GetPageList(int pageIndex, int pageSize, string keyword)
        {
            var pagelist = WebService.Get_PersonPageList(pageIndex, pageSize, keyword).AutoMap<Person, Domain.Person.List>();
            return JResult(pagelist);
        }
    }
}