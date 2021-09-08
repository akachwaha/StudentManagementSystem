using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMS_3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            ////localhost::8080/Students/8
            //routes.MapRoute(name: "StudentRoute",
            //    url: "Students/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            //localhost:8080/Home/About
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
