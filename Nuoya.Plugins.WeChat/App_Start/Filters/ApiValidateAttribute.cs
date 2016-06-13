
using Core.AuthAPI;
using Core.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;

namespace Nuoya.Plugins.WeChat.Filters
{
    public class ApiValidateAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public const string TokenName = "token";
        public const string LogonUserName = "LoginUser";
        private PersonService PersonService = new PersonService();

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var qs = HttpUtility.ParseQueryString(filterContext.Request.RequestUri.Query);
            string token = qs[TokenName];

            //判断用户token是否有效
            if (!string.IsNullOrEmpty(token))
            {
                CheckResult result = AuthAPI4Fun.ValidateToken(token);
                if (result.code == 100)
                {
                    var entity = PersonService.LoginByComId(result.tokenInfo.UID);
                    if (entity == null)
                    {
                        entity = PersonService.Add_Person(result.tokenInfo.Name, result.tokenInfo.UID);
                    }
                    if (entity != null)
                    {
                        filterContext.ControllerContext.RouteData.Values[LogonUserName] = entity;
                        SetPrincipal(new UserPrincipal<string>(new ApiLoginUsere() { ShopId = entity.ShopId, PersonId = entity.UNID, LoginName = entity.Name }));
                    }
                }
            }
            else
            {
                filterContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            }          
        }

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}