using Core.Code;
using Core.Model;
using Domain;
using IService;
using Nuoya.Plugins.WeChat.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;

namespace Nuoya.Plugins.WeChat.Api
{
    [Timer]
    public class ApiBaseController : ApiController
    {

        protected internal WebResult<T> Result<T>(T model)
        {
            return new WebResult<T>() { Result = model, Code = ErrorCode.sys_success };
        }

        protected internal WebResult<T> Result<T>(T model, ErrorCode code)
        {          
            return new WebResult<T>() { Result = model, Code = code };
        }

        public LoginUser LoginUser { get; set; }
    }
}
