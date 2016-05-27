using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Controllers;
using Server;

namespace Nuoya.Plugins.WeChat.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LoginFilterAttribute : ActionFilterAttribute
    {
        public List<Tuple<string, string>> allowAction
        {
            get
            {
                List<Tuple<string, string>> allowAction = new List<Tuple<string, string>>();
                allowAction.Add(new Tuple<string, string>("scratchcard", "details"));
                allowAction.Add(new Tuple<string, string>("scratchcard", "do"));
                allowAction.Add(new Tuple<string, string>("upload", "uploadimage"));
                allowAction.Add(new Tuple<string, string>("login", "submit"));
                allowAction.Add(new Tuple<string, string>("login", "index"));
                return allowAction;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;

            
            var actionName = filterContext.RouteData.Values["Action"].ToString();
            var controllerName = filterContext.RouteData.Values["Controller"].ToString();
            var actionMethodList = filterContext.Controller.GetType().GetMethods();
            var url = string.Format("{0}/{1}", controllerName, actionName);
            //判断页面是否需要登录
            if (allowAction.FirstOrDefault(x => x.Item1.Equals(controllerName.ToLower()) && x.Item2.Equals(actionName.ToLower())) == null)
            {
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
                                RedirectResult redirectResult = new RedirectResult("/login");
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
                else if (!controllerName.ToLower().Equals("home")&&!WebService.IsHaveAuthority(url, controller.Client.LoginUser.MenuLimitFlag))
                {
                    RedirectResult redirectResult = new RedirectResult("/");
                    filterContext.Result = redirectResult;
                }
            }
        }
    }
}