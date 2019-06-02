using System.Web;
using System.Web.Optimization;

namespace DrDone
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                .Include("~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery.validate*")
                .Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/all.min.js")
                .Include("~/areas/admin/Content/script.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.min.css")
                .Include("~/Content/site.css")
                .Include("~/Content/all.min.css"));
        }
    }
}
