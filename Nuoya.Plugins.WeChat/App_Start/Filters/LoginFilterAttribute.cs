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

            //判断页面是否需要登录
            if (allowAction.FirstOrDefault(x => x.Item1.Equals(controllerName.ToLower()) && x.Item2.Equals(actionName.ToLower())) == null)
            {
                //判断用户token是否有效
                if (controller.LoginUser == null)
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        CheckResult result = AuthAPI4Fun.ValidateToken(token);
                        if (result.code == 100)
                        {
                            var entity = PersonService.LoginByComId(result.tokenInfo.UID);
                            filterContext.HttpContext.Session["LoginUser"]=new Core.Code.LoginUser(entity.UNID, entity.Account, entity.Name, entity.ComId, null, false);
                        }
                    }
                    else
                    {
                        if (!controllerName.ToLower().Equals("login"))
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
}