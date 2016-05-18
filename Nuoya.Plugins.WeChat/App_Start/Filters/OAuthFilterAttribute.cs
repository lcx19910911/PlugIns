using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nuoya.Plugins.WeChat.Controllers;
using Core.Web;
using Core;

namespace Nuoya.Plugins.WeChat.Filters
{
    /// <summary>
    /// 过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OAuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string requestUrl = filterContext.RequestContext.HttpContext.Request.Url.ToString();
            string openId = CacheHelper.Get<string>("openId");
            if (string.IsNullOrEmpty(openId))
            {
                string userAgent = filterContext.RequestContext.HttpContext.Request.UserAgent;
                if (userAgent.ToLower().Contains("micromessenger"))
                {
                    string redirect_uri = string.Format("{0}/User/DoAuth", Params.DomianName);
                    string oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}", Params.AppId, redirect_uri, requestUrl);
                    RedirectResult redirectResult = new RedirectResult("/User/DoAuth?code=11222333&state=" + requestUrl);

                    filterContext.Result = redirectResult;
                }
            }
        }
    }
}