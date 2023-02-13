using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KisanKiDukan
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                   name: "Blog",
                   url: "{burl}",
                   defaults: new { controller = "Home", action = "BlogDetails", id = UrlParameter.Optional }
               );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "login", id = UrlParameter.Optional }
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            );
            
        }
    }
}
