
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Filters;
using Core.Model;
using Core.Code;
using Core.Extensions;
using Core.Helper;

namespace Nuoya.Plugins.WeChat.Controllers
{
    [Timer]
    public class BaseController : Controller
    {

        //
        // GET: /Web/
        public ActionResult JError()
        {
            return JResult(false);
        }

        public ActionResult Error()
        {
            return View("Error");
        }

        public ActionResult _505()
        {
            return View();
        }

        public ActionResult TokenNotAvailable()
        {
            return View();
        }

        /// <summary>
        /// 返回部分视图的错误页
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PartialError()
        {
            return null;
        }

        #region Json返回

        /// <summary>
        /// 返回异常编号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected internal JsonResult JResult(ErrorCode code)
        {
            return Json(new
            {
                Code = code,
                ErrorDesc = code.GetDescription()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回异常编号附带自定义消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appendMsg"></param>
        /// <returns></returns>
        protected internal JsonResult JResult(ErrorCode code, string appendMsg)
        {
            return Json(new
            {
                Code = code,
                ErrorDesc = code.GetDescription(),
                Append = appendMsg
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult JResult<T>(T model)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }

        protected internal JsonResult JResult<T>(WebResult<T> model)
        {
            if (model.OccurError)
            {
                return JResult(model.Code, model.Append);
            }
            return JResult(model.Result);
        }

        protected internal JsonResult JResult(string model)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult JResult<T>(WebResult<PageList<T>> model, Func<T, object> selector)
        {
            if (model.OccurError)
            {
                return JResult(model.Code);
            }
            return Json(new
            {
                Code = model.Code,
                Result = new
                {
                    RecordCount = model.Result.RecordCount,
                    PageCount = model.Result.PageCount,
                    IsLastPage = model.Result.IsLastPage,
                    List = model.Result.List.Select(selector).ToList()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        protected internal JsonResult JResult<T>(WebResult<List<T>> model, Func<T, object> selector)
        {
            if (model.OccurError)
            {
                return JResult(model.Code);
            }
            return Json(new
            {
                Code = model.Code,
                Result = model.Result != null ? model.Result.Select(selector).ToList() : null
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected internal ViewResult View<T>(WebResult<T> model)
        {
            if (model.OccurError)
            {
                return View("Error");
            }
            return View(model.Result);
        }

        protected internal ActionResult ReLogin()
        {
            return RedirectToAction("Login", "Home");
        }



        private LoginUser _loginUser = null;

        public LoginUser LoginUser
        {
            get
            {
                return _loginUser != null ? _loginUser : CookieHelper.GetCurrentUser();
            }
        }

          

    }
}