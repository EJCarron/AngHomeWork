using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System;
using System.Web.Security;

namespace AngularHomeWork {
    public class Global : HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //protected void FormsAuthentication_OnAuthenticate(Object sender, FormsAuthenticationEventArgs e) {
        //    if (FormsAuthentication.CookiesSupported == true) {
        //        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null) {
        //            try {
        //                //let us take out the username now
        //                string username =  FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
        //                string roles = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).UserData;

        //                //Let us set the Pricipal with our user specific details
        //                e.User = new System.Security.Principal.GenericPrincipal(
        //                  new System.Security.Principal.GenericIdentity(username, "Forms"),
        //                  roles.Split(';')
        //                );


        //            } catch (Exception) {
                        
        //            }
        //        }
        //    }
        //}



        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e) {
            if (FormsAuthentication.CookiesSupported == true) {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null) {
                    try {
                        //let us take out the username now

                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value);

                        string username = ticket.Name;
                        string roles = ticket.UserData;

                        //Let us set the Pricipal with our user specific details
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                            new System.Security.Principal.GenericIdentity(username, "Forms"), 
                            roles.Split(';')
                        );


                    } catch (Exception) {
                        //somehting went wrong
                    }
                }
            }
        }

    }
}
