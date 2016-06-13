using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Controllers;
using Core.AuthAPI;
using Service;

namespace Nuoya.Plugins.WeChat.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public List<Tuple<string, string, string>> allowAction
        {
            get
            {
                List<Tuple<string, string, string>> allowAction = new List<Tuple<string, string, string>>();
                allowAction.Add(new Tuple<string, string, string>("scratchcard", "shome", "details"));
                allowAction.Add(new Tuple<string, string, string>("scratchcard", "shome", "do"));
                allowAction.Add(new Tuple<string, string, string>("dinner", "menu", "index"));
                allowAction.Add(new Tuple<string, string, string>("dinner", "menu", "getdish"));
                return allowAction;
            }
        }

        private PersonService PersonService = new PersonService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;


            var actionName = filterContext.RouteData.Values["Action"].ToString();
            var controllerName = filterContext.RouteData.Values["Controller"].ToString();
            var actionMethodList = filterContext.Controller.GetType().GetMethods();
            string requestUrl = filterContext.HttpContext.Request.Url.ToString();
            string token = filterContext.HttpContext.Request["token"];

            //判断用户token是否有效
            if (!string.IsNullOrEmpty(token) && controller.LoginUser == null)
            {
                CheckResult result = AuthAPI4Fun.ValidateToken(token);
                if (result.code == 100)
                {

                    var entity = PersonService.LoginByComId(result.tokenInfo.UID);
                    if (entity == null)
                    {
                        entity = PersonService.Add_Person(result.tokenInfo.Name, result.tokenInfo.UID);
                    }
                    if (entity != null)
                    {
                        filterContext.HttpContext.Session["LoginUser"] = new Core.Model.LoginUser(entity.UNID, entity.Account, entity.Name, entity.ComId, null, false);
                    }
                }
            }

            //判断页面是否需要登录
            if (allowAction.FirstOrDefault(x => x.Item1.Equals(controllerName, StringComparison.OrdinalIgnoreCase) && x.Item2.Equals(actionName, StringComparison.OrdinalIgnoreCase)) == null)
            {
                if (controller.LoginUser == null)
                {
                    if (!controllerName.Equals("login", StringComparison.OrdinalIgnoreCase))
                    {
                        var actionMethod = actionMethodList.FirstOrDefault(x => x.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
                        if (actionMethod != null)
                        {
                            if (actionMethod.ReturnType.Name == "ViewResult" || actionMethod.ReturnType.Name == "ActionResult")
                            {
                                RedirectResult redirectResult = new RedirectResult("/login/index?redirecturl=" + requestUrl);
                                filterContext.Result = redirectResult;
                            }
                            else if (actionMethod.ReturnType.Name == "JsonResult")
                            {
                                JsonResult jsonResult = new JsonResult();
                                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                                filterContext.RequestContext.HttpContext.Response.StatusCode = 9999;
                                filterContext.Result = jsonResult;
                            }
                        }
                    }

                }
            }
        }
    }
}