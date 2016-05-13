using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Controllers;

namespace Nuoya.Plugins.WeChat.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            var actionName = filterContext.RouteData.Values["Action"].ToString();
            var controllerName = filterContext.RouteData.Values["Controller"].ToString();
            var actionMethodList = filterContext.Controller.GetType().GetMethods();

            //判断用户token是否有效
            if (controller.Client.LoginUser == null)
            {
                if (!controllerName.ToLower().Equals("login"))
                {
                    var actionMethod = actionMethodList.FirstOrDefault(x => x.Name.Equals(actionName, StringComparison.OrdinalIgnoreCase));
                    if (actionMethod != null)
                    {
                        if (actionMethod.ReturnType.Name == "ViewResult" || actionMethod.ReturnType.Name == "ActionResult")
                        {
                            RedirectResult redirectResult = new RedirectResult("/login/index");
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