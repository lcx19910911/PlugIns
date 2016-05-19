using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    /// <summary>
    /// 微信墙
    /// </summary>
    public class WallController : BaseController
    {
        // GET: Wall
        public ActionResult Index()
        {
            return View();
        }
    }
}