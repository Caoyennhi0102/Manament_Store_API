using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Manament_Store_API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Admin_ImageSettings",
                url: "Admin/ImageSettings/{action}/{id}",
                defaults: new
                {
                    controller = "ImageSettings",
                    action = "EditImage",
                    id = UrlParameter.Optional
                },
                namespaces: new[] { "Manament_Store_API.Controllers.Admin" }
                );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ProductCategory", action = "AddProductCategory", id = UrlParameter.Optional }
            );
            
           
        }
    }
}
