﻿using System.Web.Mvc;
using System.Web.Routing;

namespace AngularHomeWork {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
               defaults: new { controller = "Home", action = "Login" }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Teacher", id = 1}
            //);

            routes.MapRoute(
                name: "CreateClassRoomPage",
                url: "ClassRoom/Create/{id}",
                defaults: new { controller = "ClassRoom", action = "Create", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "NewClassRoom",
                url: "ClassRoom/New",
                defaults: new { controller = "ClassRoom", action = "New" }
            );



            //routes.MapRoute(
            //    name: "IndexClassRoom",
            //    url: "ClassRoom/Index",
            //    defaults: new { controller = "ClassRoom", action = "Index" }
            //);

            routes.MapRoute(
                name: "Teacher",
                url: "{controller}/Teacher",
                defaults: new { controller = "Home", action = "Teacher" }
            );

            routes.MapRoute(
                name: "Student",
                url: "{controller}/Student",
                defaults: new { controller = "Home", action = "Student" }
            );

            //routes.MapRoute(
            //    name: "Login",
            //    url: "{controller}/Login",
            //    defaults: new { controller = "Home", action = "Login" }
            //);
        }
    }
}
