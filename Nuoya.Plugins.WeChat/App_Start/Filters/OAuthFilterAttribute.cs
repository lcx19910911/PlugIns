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
    /// 过滤器  微信身份验证
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
                    // https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxf0e81c3bee622d60&redirect_uri=http%3A%2F%2Fnba.bluewebgame.com%2Foauth_response.php&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect
                    string oauthUrl = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state={2}#wechat_redirect", Params.AppId, HttpUtility.HtmlEncode(redirect_uri), HttpUtility.HtmlEncode(requestUrl));

                    filterContext.HttpContext.Response.Redirect(oauthUrl);
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.End();
                }
            }
        }
    }
}