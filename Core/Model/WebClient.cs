using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Core.Code;
using Core.Extensions;
using System.Web.SessionState;
using System.Web.Security;

namespace Core.Model
{
    public class WebClient
    {
        public WebClient(HttpContext httpContext)
        {
            this.HttpContext = httpContext;
        }
        public HttpContext HttpContext { get; set; }
        public HttpRequest Request
        {
            get
            {
                return HttpContext.Request;
            }

        }

        public HttpSessionState Session
        {
            get
            {
                return HttpContext.Session;
            }
        }

        /// <summary>
        /// 日志编码
        /// </summary>
        public LogCode LogCode { get; set; } = LogCode.None;

        private LoginUser _loginUser = null;

        public LoginUser LoginUser
        {
            get
            {
                return _loginUser != null ? _loginUser : Session["LoginUser"] as LoginUser;
            }
            set
            {
                Session["LoginUser"] = value;
            }
        }
        private string _postData = null;

        /// <summary>
        /// Post数据流
        /// </summary>
        public string PostData
        {
            get
            {
                if (_postData == null)
                {
                    var bytes = new byte[Request.InputStream.Length];
                    Request.InputStream.Read(bytes, 0, bytes.Length);
                    _postData = Encoding.Default.GetString(bytes);
                }
                return _postData;
            }
        }

        /// <summary>
        /// 获取上传的字符串值
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string GetParam(string paramName)
        {
            return Request.Params[paramName];
        }

        /// <summary>
        /// 获取传值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public T GetParam<T>(string paramName) where T : class
        {
            return Request[paramName] as T;
        }



        #region 全局参数
        private string ip = null;
        /// <summary>
        /// 当前IP
        /// </summary>
        public string IP
        {
            get
            {
                if (ip.IsNullOrEmpty())
                {
                    ip = Request.UserHostAddress;
                    try
                    {
                        if (!ip.IsNullOrEmpty() && ip.StartsWith("10.", StringComparison.Ordinal))
                        {
                            ip = Request.ServerVariables["HTTP_X_REAL_IP"].Split(',')[0].Trim();
                        }
                    }
                    catch { }
                }
                return ip;
            }
        }

       private SourceCode _sourceCode = SourceCode.None;

        /// <summary>
        /// 来源
        /// </summary>
        public SourceCode SourceCode
        {
            get
            {
                if (_sourceCode == SourceCode.None)
                {
                    _sourceCode = GetParam("SourceCode").ToEnum(SourceCode.None);
                }
                return _sourceCode;
            }
        }

        private int _pageIndex = 0;

        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (_pageIndex == 0)
                {
                    _pageIndex = GetParam("PageIndex").ToInt32(0);
                    if (_pageIndex <= 0)
                    {
                        _pageIndex = 1;
                    }
                }
                return _pageIndex;
            }
        }

        private int _pageSize = 0;


        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (_pageSize == 0)
                {
                    _pageSize = GetParam("PageSize").ToInt32(0);
                    if (_pageSize <= 0)
                    {
                        _pageSize = 12;
                    }
                }
                return _pageSize;
            }
        }


        /// <summary>
        /// 获取起始位置 (this.PageIndex - 1) * this.PageSize;
        /// </summary>
        public int Page_Skip
        {
            get
            {
                return (this.PageIndex - 1) * this.PageSize;
            }
        }


        #endregion
    }
}
