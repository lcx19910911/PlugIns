using Core.Code;
using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nuoya.Plugins.WeChat.Api
{
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
    }
}
