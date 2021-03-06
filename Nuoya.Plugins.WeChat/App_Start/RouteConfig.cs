﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nuoya.Plugins.WeChat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "home", action = "Index", id = UrlParameter.Optional },
                namespaces:new string[] { "Nuoya.Plugins.WeChat.Controllers", "Nuoya.Plugins.WeChat.Api"}
            );
        }
    }
}
