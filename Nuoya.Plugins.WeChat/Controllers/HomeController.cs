using Nuoya.Plugins.WeChat.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class HomeController : BaseController
    {
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}