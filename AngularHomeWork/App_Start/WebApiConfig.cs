using System.Web.Http;

namespace AngularHomeWork {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GetClassRoom",
                routeTemplate: "api/ClassRoom/get/{classRoomName}",
                defaults: new { controller = "ClassRoom", classRoomName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CreateClassRoom",
                routeTemplate: "api/ClassRoom/create",
                defaults: new { controller = "ClassRoom" }
            );

            config.Routes.MapHttpRoute(
                name: "ArchiveClassRoom",
                routeTemplate: "api/ClassRoom/archive",
                defaults: new { controller = "ClassRoom" }
            );

            config.Routes.MapHttpRoute(
                name: "GetAssignment",
                routeTemplate: "api/Assignment/get/{assignmentId}",
                defaults: new { controller = "Assignment", assignmentId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "CreateAssignment",
                routeTemplate: "api/Assignment/create",
                defaults: new { controller = "Assignment" }
            );

        }
    }
}
