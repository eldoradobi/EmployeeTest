using System.Web;
using System.Web.Optimization;

namespace MetinvestTest
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/easyui").Include(
                       "~/Scripts/jquery.easyui.min.js",
                       "~/Scripts/locale/easyui-lang-ru.js"));
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                       "~/Scripts/app.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/default/easyui.css",
                      "~/Content/themes/icon.css",
                      "~/Content/themes/color.css"));
        }
    }
}
