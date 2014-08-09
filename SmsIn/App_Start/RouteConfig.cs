using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmsIn
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("VirtualSmsGlobal", "http-api.php", new { controller = "Sms", action = "VirtualSmsGlobal" });
            routes.MapRoute("VirtualExetel", "api_sms.php", new { controller = "Sms", action = "VirtualExetel" });

            routes.MapRoute("RouteName","{action}",new { controller = "Sms", action = "Index" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
