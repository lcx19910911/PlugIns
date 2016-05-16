﻿using Core.Util;
using EntityFramework.Audit;
using StackExchange.Profiling;
using System;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Nuoya.Plugins.WeChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogHelper.WriteCustom(string.Format("Application_Start At {0} \r\n", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")), @"Application\", false);
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //脚本资源注册
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            MiniProfiler.Settings.Results_Authorize = Request =>
            {
                //string name = Request.Cookies["name"] == null ? "" : Request.Cookies["name"].Value;
                //if (name.Equals("admin"))
                //    return true;
                //else
                //    return false;
                return true;
            };

            StackExchange.Profiling.EntityFramework6.MiniProfilerEF6.Initialize();

            var auditConfiguration = AuditConfiguration.Default;
            auditConfiguration.IncludeRelationships = true;
            auditConfiguration.LoadRelationships = false;
            auditConfiguration.DefaultAuditable = true;

        }

        protected void Application_End(object sender, EventArgs e)
        {
            var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
            if (runtime != null)
            {
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                ApplicationShutdownReason shutDownReason = (ApplicationShutdownReason)runtime.GetType().InvokeMember("_shutdownReason", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                LogHelper.WriteCustom(string.Format("Application_End:shutDownMessage:\r\n{0}\r\nshutDownStack:\r\n{1}\r\nshutDownReason:\r\n{2}\r\n", shutDownMessage, shutDownStack, shutDownReason.ToString()), @"Application\", false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is ThreadAbortException)
            {
                Thread.ResetAbort();
                HttpContext.Current.ClearError();
                return;
            }
            var httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                LogHelper.WriteCustom(httpException.ToString(), "404Error\\");
            }
            else
            {
                LogHelper.WriteException("Application Error.", Server.GetLastError());
            }
            Server.ClearError();
            Response.Clear();
            Response.RedirectToRoute(new
            {
                Controller = "base",
                Action = "Error"
            });
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)//这里是允许本地访问启动监控,可不写
            {
                MiniProfiler.Start();

            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}