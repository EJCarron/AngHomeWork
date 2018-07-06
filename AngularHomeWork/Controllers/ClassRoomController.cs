using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Newtonsoft.Json;
using AngularHomeWork.MarshallingObjects;
using AngularHomeWork.Models;

namespace AngularHomeWork.Controllers {
    public class ClassRoomController : Controller {

        //public ActionResult Index(){
                
        //}

        public ActionResult Create(int id){

            UserResponse response = TheDataStore.FetchTeacher(id);

            Teacher teacher = (Teacher)response.user;

            CreateClassRoomView CCRV = new CreateClassRoomView(teacher);

            return View(CCRV);

        }
    }
}