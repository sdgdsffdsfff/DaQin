using System.Web;
using System.Web.Optimization;

namespace DaQin
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery/jquery.unobtrusive*",
                        "~/Scripts/jquery/jquery.validate*"));

            // 使用 Modernizr 的开发版本进行开发和了解信息。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/jquery/modernizr-*"));

            //页面公共css
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/main.css",
                "~/Content/form.css"));

            //easyui的js
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                "~/Scripts/jquery-easyui-1.4/jquery.easyui.js",
                "~/Scripts/jquery-easyui-1.4/locale/easyui-lang-zh_CN.js",
                "~/Scripts/jquery-easyui-1.4/plugins/datagrid-detailview.js"));

            //easyui的css
            bundles.Add(new StyleBundle("~/bundles/easyuicss").Include(
                "~/Scripts/jquery-easyui-1.4/themes/default/easyui.css",
                "~/Scripts/jquery-easyui-1.4/themes/icon.css"));
        }
    }
}