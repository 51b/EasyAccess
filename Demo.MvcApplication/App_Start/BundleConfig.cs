﻿using System.Web;
using System.Web.Optimization;

namespace Demo.MvcApplication
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.0.3.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/css").Include(
                        "~/Scripts/Plugins/EasyUi/themes/icon.css",
                        "~/Scripts/Plugins/EasyUi/themes/metro/easyui.css"));
        }
    }
}