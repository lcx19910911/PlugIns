using Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nuoya.Plugins.WeChat.Filters
{
    public class ExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
                return;
            var ex = filterContext.Exception ?? new Exception("no further information exists");
            filterContext.ExceptionHandled = true;
            
            Core.Util.LogHelper.WriteException(ex);
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "base" }, { "action", "_505" } });
        }
    }
}