using Core.AuthAPI;
using Core.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;

namespace Nuoya.Plugins.WeChat.App_Start.Filters
{
    public class ApiAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public const string TokenName = "token";
        public const string LogonUserName = "LoginUser";
        private PersonService PersonService = new PersonService();

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            var qs = HttpUtility.ParseQueryString(filterContext.Request.RequestUri.Query);
            string token = qs[TokenName];
            bool isValidate = false;
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
                        isValidate = true;
                        HttpContext.Current.Session.Add(LogonUserName, new Core.Model.LoginUser(entity.UNID, entity.Account, entity.Name, entity.ComId, null, false));
                    }
                }
            }


            if (!isValidate)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        //private void SetPrincipal(IPrincipal principal)
        //{
        //    Thread.CurrentPrincipal = principal;
        //    if (HttpContext.Current != null)
        //    {
        //        HttpContext.Current.User = principal;
        //    }
        //}

        //public override void OnAuthorization(HttpActionContext filterContext)
        //{
        //    var qs = HttpUtility.ParseQueryString(filterContext.Request.RequestUri.Query);
        //    string token = qs[TokenName];
        //    bool isValidate = false;
        //    //判断用户token是否有效
        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        CheckResult result = AuthAPI4Fun.ValidateToken(token);
        //        if (result.code == 100)
        //        {
        //            var entity = PersonService.LoginByComId(result.tokenInfo.UID);
        //            if (entity == null)
        //            {
        //                entity = PersonService.Add_Person(result.tokenInfo.Name, result.tokenInfo.UID);
        //            }
        //            if (entity != null)
        //            {
        //                filterContext.ControllerContext.RouteData.Values[LogonUserName] = entity;
        //                SetPrincipal(new UserPrincipal<string>(new ApiLoginUsere() { ShopId = entity.ShopId, PersonId = entity.UNID, LoginName = entity.Name }));
        //                isValidate = true;
        //            }
        //        }
        //    }

        //    if (!isValidate)
        //    {
        //        base.HandleUnauthorizedRequest(filterContext);
        //    }
        //}
    }
}