using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TextEditor
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TextEditor.Controllers" }
            );
            routes.MapRoute(
            name: "DeleteUser",
            url: "Admin/Home/del/{id}",
            defaults: new
            {
                controller = "Home",
                action = "Del",
                product_id = UrlParameter.Optional
            });
            routes.MapRoute(
          name: "EditUser",
          url: "Admin/Home/Edit/{id}",
          defaults: new
          {
              controller = "Home",
              action = "Edit",
              product_id = UrlParameter.Optional
          });

        }
    }
}
