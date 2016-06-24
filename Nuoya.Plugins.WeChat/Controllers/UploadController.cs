using Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nuoya.Plugins.WeChat.Controllers
{
    [AllowAnonymous]
    public class UploadController : BaseController
    {
        // GET: Upload
        public ActionResult UploadImage(string mark)
        {
            HttpPostedFileBase file = Request.Files[0];
            if (file != null)
            {
                string path = UploadHelper.Save(file, mark);
                return JResult(path);
            }
            else
                return JResult("");
        }
    }
}