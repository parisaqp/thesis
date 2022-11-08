using System.Web.Optimization;

namespace RentalAdmin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //           "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            //old

            bundles.Add(new StyleBundle("~/ContentAdmin/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/SiteAdmin.css"

                      ));
            bundles.Add(new StyleBundle("~/ContentAgent/css").Include(
              "~/Content/bootstrap.css",
              "~/Content/SiteAgent.css"

              ));




            //new 
            bundles.Add(new StyleBundle("~/cssbundles/css").Include(
           "~/Content/bootstrap.css",
           "~/Content/site.css",
                    "~/Content/font-awesome.min.css"
                     ));
            //"~/Content/owl.carousel.min.css"
            bundles.Add(new StyleBundle("~/cssbundles/cssadmin").Include(
                    "~/Content/bootstrap-3.4.1-dist/css/bootstrap.css",
                    "~/Content/siteadmin.css",
           "~/Content/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/cssbundles/cssagent").Include(
                //"~/Content/bootstrap-4.5.3-rtl/bootstrap.min.css"
                // "~/Content/bootstrap.css"
                "~/Content/siteagent.css"
       , "~/Content/font-awesome.min.css"
       ));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"

                        ));

            bundles.Add(new ScriptBundle("~/bundles/generaljs").Include(
                       "~/Scripts/jquery-{version}.js",
//"~/Scripts/mainwebsitejs.js",
    "~/Scripts/bootstrap.js"

                       ));

            //"~/Scripts/owl.carousel.min.js",
            //            "~/Scripts/owl.carousel2.thumbs.min.js"


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"

                     ));


            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                     "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new StyleBundle("~/cssbundles/dropzonescss").Include(
                     "~/Scripts/dropzone/dropzone.css"));
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;
        }
    }
}
