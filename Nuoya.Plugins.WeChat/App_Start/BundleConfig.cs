﻿using System.Web;
using System.Web.Optimization;

namespace Nuoya.Plugins.WeChat
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region 样式
            bundles.Add(new StyleBundle("~/Content/Admin").Include(
                "~/Styles/zTreeStyle/zTreeStyle.css",
                "~/Styles/css/amazeui.css",
                "~/Scripts/tipso/css/tipso.min.css",
                "~/Styles/admin.css",
                "~/Scripts/uploadify/uploadify.css",
                "~/Styles/css/jquery-ui.css"));
            #endregion

            #region 脚本
            bundles.Add(new ScriptBundle("~/Scripts/Login").Include(
                "~/Scripts/jquery-2.2.3.min.js",
                "~/Scripts/amazeui.js",
                "~/Scripts/Nuoya/nuoya.core.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Admin").Include(
               "~/Scripts/jquery-2.2.3.min.js",
               "~/Scripts/jquery.form.js",
               "~/Scripts/amazeui.js",
               "~/Scripts/jquery-validation/js/jquery.validate.js",
               "~/Scripts/jquery.ztree.all-3.5.min.js",
               "~/Scripts/ztree-select.js",
               "~/Scripts/tipso/js/tipso.js",

               "~/Scripts/Nuoya/nuoya.core.js",
               "~/Scripts/Nuoya/nuoya.grid.js",
               "~/Scripts/Nuoya/nuoya.form.js",
               "~/Scripts/Nuoya/nuoya.other.js",

               "~/Scripts/datetimepicker/jquery-ui.js",
               "~/Scripts/datetimepicker/jquery-ui-slide.min.js",
               "~/Scripts/datetimepicker/jquery-ui-timepicker-addon.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/Avalon").Include(
                "~/Scripts/avalon.js"
               ));
            #endregion

        }
    }
}
