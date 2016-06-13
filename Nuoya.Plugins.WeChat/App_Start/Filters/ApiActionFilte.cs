using Core.AuthAPI;
using Nuoya.Plugins.WeChat.Api;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Nuoya.Plugins.WeChat.App_Start.Filters
{
    public class ActionFilter : ActionFilterAttribute
    {


        public const string SessionKeyName = "LoginUser";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
          
        }

        public static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}