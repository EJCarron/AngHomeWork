using System.Web.Mvc;
using System.Web.Routing;

namespace AngularHomeWork {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Teacher", id = 1}
            );

            routes.MapRoute(
                name: "CreateClassRoom",
                url: "ClassRoom/Create/{id}",
                defaults: new { controller = "ClassRoom", action = "Create", id = UrlParameter.Optional }
            );



            //routes.MapRoute(
            //    name: "Teacher",
            //    url: "{controller}/Teacher/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
