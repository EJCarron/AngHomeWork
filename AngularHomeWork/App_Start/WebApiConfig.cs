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


            config.Routes.MapHttpRoute(
                name: "RegisterUser",
                routeTemplate: "api/User/register",
                defaults: new { controller = "User" }
            );

            config.Routes.MapHttpRoute(
                name: "AttemptLogin",
                routeTemplate: "api/User/attemptLogin/{email}/{passwordAttempt}",
                defaults: new { controller = "User" }
            );

            config.Routes.MapHttpRoute(
                name: "GetSubscription",
                routeTemplate: "api/Subscription/get/{classRoomName}",
                defaults: new { controller = "Subscription", classRoomName = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Subscribe",
                routeTemplate: "api/Subscription/subscribe",
                defaults: new { controller = "Subscription" }
            );

            config.Routes.MapHttpRoute(
                name: "Unsubscribe",
                routeTemplate: "api/Subscription/unsubscribe",
                defaults: new { controller = "Subscription" }
            );

            config.Routes.MapHttpRoute(
                name: "GetStudentAssignment",
                routeTemplate: "api/StudentAssignment/get/{assignmentId}",
                defaults: new { controller = "StudentAssignment", assignmentId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ChangeMarkDoneState",
                routeTemplate: "api/StudentAssignment/changeDoneState",
                defaults: new { controller = "StudentAssignment" }
            );

            config.Routes.MapHttpRoute(
                name: "GetOutStanding",
                routeTemplate: "api/OutStanding/get",
                defaults: new { controller = "OutStanding"}
            );

        }
    }
}
