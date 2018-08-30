using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;

using AngularHomeWork.Models;


namespace AngularHomeWork.Controllers {

    
    public class HomeController : Controller {
        public ActionResult Index() {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }

        public ActionResult Login(){


            if (HttpContext.User.Identity.Name != null) {
                int userId = Convert.ToInt32(HttpContext.User.Identity.Name);

                UserTypeResponse userTypeResponse = TheDataStore.getUserType(userId);

                if(userTypeResponse.response.isOk){

                    switch(userTypeResponse.type){
                        
                        case UserType.teacher:
                            return RedirectToAction("Teacher");

                        case UserType.student:
                            return RedirectToAction("Student");
                    }


                }
            }

            return View();
        }


        [Authorize (Roles = "Teacher")]
        public ActionResult Teacher() {
           
            int userId = Convert.ToInt32( HttpContext.User.Identity.Name);

            UserResponse response = TheDataStore.FetchTeacher(userId);

            Teacher teacher = (Teacher)response.user;

            return View(teacher);
        }


    }
}
