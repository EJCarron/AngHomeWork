using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using AngularHomeWork.MarshallingObjects;
using AngularHomeWork.Models;
using System.Web.Http;
using System.Net.Http;
using System.Web.Security;
using System.Net.Http.Formatting;

namespace AngularHomeWork.Controllers
{
    public class UserController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage attemptLogin(string email, string passwordAttempt){


            LoginResponse loginResponse = TheDataStore.attemptLogin(email, passwordAttempt);

            if (!loginResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, loginResponse.response.message);
            } else {

                if(loginResponse.success){

                    string idString = loginResponse.userId.ToString();


                    FormsAuthentication.SetAuthCookie(idString, false);

                    FormsAuthenticationTicket ticket1 = new FormsAuthenticationTicket(
                         1,                                   // version
                         idString,   // get username  from the form
                         DateTime.Now,                        // issue time is now
                         DateTime.Now.AddMinutes(60*24*7),         // expires in
                         true,      // cookie is persists ovr sessions
                         ((loginResponse.userType == UserType.teacher)?"Teacher" : "Student")                              // role assignment is stored in userData
                    );




                   HttpCookie cookie = new HttpCookie(
                   FormsAuthentication.FormsCookieName,
                   FormsAuthentication.Encrypt(ticket1)
                    );

                    Request.Headers. Add("Set-Cookie", cookie.ToString());


                }


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, loginResponse);
            }


        }


        [HttpPost]
        public HttpResponseMessage register(RegisterUserCO cO) {

            UserResponse userResponse = TheDataStore.registerNewUser(cO);

            if (!userResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, userResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, userResponse.user.id);
            }
        }



        
    }
}
