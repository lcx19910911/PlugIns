using Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload()
        {
            var file = Request.Files["image"];
            UploadHelper.Save(file, "ScratchCard111");
            return JResult(true);
        }
    }
}