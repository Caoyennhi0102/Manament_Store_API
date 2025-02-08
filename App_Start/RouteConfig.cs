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
             name: "GetBankList",
             url: "bank/list",
             defaults: new { controller = "Bank", action = "GetBankList" }
         );
            routes.MapRoute(
            name: "ContractDefault",
            url: "{controller}/{action}/{id}",
             defaults: new { controller = "Contract", action = "ContractCreate", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "CheckAccount",
             url: "bank/check-account",
            defaults: new { controller = "Bank", action = "CheckAccount" }
         );
             routes.MapRoute(
             name: "Approval",
             url: "duyet-don/{action}/{id}",
             defaults: new { controller = "SupplierOrderApproval", action = "PendingOrders", id = UrlParameter.Optional }
         );
            routes.MapRoute(
             name: "ReceiveGoodsFromSupplier",
             url: "nhan-hang/{action}/{id}",
             defaults: new { controller = "ReceiveGoodsFromSupplier", action = "Index", id = UrlParameter.Optional }
          );



            routes.MapRoute(
                name: "SupplierDefault",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ReceiveGoodsFromSupplier", action = "Index", id = UrlParameter.Optional }
            );

            
           
        }
    }
}
