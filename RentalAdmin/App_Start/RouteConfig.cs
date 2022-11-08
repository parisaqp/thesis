using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RentalAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Embassy",
                url: RentalAdmin.Models.StaticList.EmbassyStart+"{id}/{FriendlyUrl}",
                defaults: new { controller = "home", action = "EmbassyPage", id = UrlParameter.Optional, FriendlyUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "childhood",
              url: RentalAdmin.Models.StaticList.RegionStart+ "{id}/{FriendlyUrl}",
              defaults: new { controller = "home", action = "RegionPage1", id = UrlParameter.Optional, FriendlyUrl = UrlParameter.Optional }
          );

            routes.MapRoute(
            name: "property",
            url: RentalAdmin.Models.StaticList.PropertyStart + "{id}/{FriendlyUrl}",
            defaults: new { controller = "home", action = "PropertyPage" , id = UrlParameter.Optional, FriendlyUrl = UrlParameter.Optional }
        );

            routes.MapRoute(
          name: "blog",
          url: "blog/{id}/{FriendlyUrl}",
          defaults: new { controller = "home", action = "OneBlogPage", id = UrlParameter.Optional, FriendlyUrl = UrlParameter.Optional }
      );

            routes.MapRoute(
            name: "regionspage",
            url: "home/best-neighborhoods-in-tehran",
            defaults: new { controller = "home", action = "regionspage" }
        );

            routes.MapRoute(
        name: "embassiespage",
        url: "home/list-embassies-tehran",
        defaults: new { controller = "home", action = "embassiespage" }
    );
            routes.MapRoute(
    name: "Google API Sign-in",
    url: "signin-google",
    defaults: new { controller = "Account", action = "ExternalLoginCallbackRedirect" });

        routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional }
            );

        }
    }
}
