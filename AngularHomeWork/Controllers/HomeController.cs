using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;

using AngularHomeWork.Models;
using System.Web.Security;
using System.Security.Principal;
using System.Diagnostics.Contracts;

namespace AngularHomeWork.Controllers {

    
    public class HomeController : Controller {
        public ActionResult Index() {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        public ActionResult Logout(){
            Contract.Ensures(Contract.Result<ActionResult>() != null);

            FormsAuthentication.SignOut();

            HttpContext.User =
                new GenericPrincipal(new GenericIdentity(string.Empty), null);

            return RedirectToAction("Login");
        }

        public ActionResult Login(){


            if (HttpContext.User.Identity.Name != "") {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);

                UserTypeResponse userTypeResponse = TheDataStore.getUserType(userId);

                if(userTypeResponse.response.isOk){

                    switch(userTypeResponse.type){
                        
                        case UserType.teacher:
                            return RedirectToAction("Teacher");

                        case UserType.student:
                            return RedirectToAction("Student");
                    }; 


                }
            }

            return View();
        }


        [Authorize]
        public ActionResult Teacher() {
           
            int userId = Convert.ToInt32( HttpContext.User.Identity.Name);

            UserResponse response = TheDataStore.FetchTeacher(userId);

            Teacher teacher = (Teacher)response.user;

            return View(teacher);
        }

        [Authorize]
        public ActionResult Student(){

            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);

            UserResponse userResponse = TheDataStore.FetchStudent(userId);

            Student student = (Student)userResponse.user;

            return View(student);

        }


    }
}
