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
                .Include("~/assets/js/jquery-3.3.1.min.js")
                .Include("~/Scripts/bootstrap.bundle.min.js")
                .Include("~/Scripts/popper.min.js")
                .Include("~/Scripts/all.min.js")
                .Include("~/Scripts/dropify/js/dropify.min.js")
                .Include("~/Scripts/script.js")
                .Include("~/areas/admin/Content/script.js")
                .Include("~/assets/plugins/greensock/TweenMax.min.js")
                .Include("~/assets/plugins/greensock/TimelineMax.min.js")
                .Include("~/assets/plugins/scrollmagic/ScrollMagic.min.js")
                .Include("~/assets/plugins/greensock/animation.gsap.min.js")
                .Include("~/assets/plugins/greensock/ScrollToPlugin.min.js")
                .Include("~/assets/plugins/OwlCarousel2-2.2.1/owl.carousel.js")
                .Include("~/assets/plugins/slick-1.8.0/slick.js")
                .Include("~/assets/plugins/easing/easing.js")
                .Include("~/assets/plugins/Isotope/isotope.pkgd.min.js")
                .Include("~/assets/plugins/jquery-ui-1.12.1.custom/jquery-ui.js")
                .Include("~/assets/plugins/parallax-js-master/parallax.min.js")
                .Include("~/assets/js/custom.js")
                .Include("~/assets/js/shop_custom.js")
                );

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.min.css")
                .Include("~/Content/site.css")
                .Include("~/Content/all.min.css")
                .Include("~/Scripts/dropify/css/dropify.min.css")
                .Include("~/assets/plugins/OwlCarousel2-2.2.1/owl.carousel.css")
                .Include("~/assets/plugins/OwlCarousel2-2.2.1/owl.theme.default.css")
                .Include("~/assets/plugins/OwlCarousel2-2.2.1/animate.css")
                .Include("~/assets/plugins/slick-1.8.0/slick.css")
                .Include("~/assets/plugins/jquery-ui-1.12.1.custom/jquery-ui.css")
                .Include("~/assets/styles/responsive.css")
                .Include("~/assets/styles/main_styles.css")
                );
        }
    }
}
