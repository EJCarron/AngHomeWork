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

        //public ActionResult Index(int id, string classRoomName){

        //    //Get Teacher Data
        //    UserResponse teacherResponse = TheDataStore.FetchTeacher(id);

        //    Teacher teacher = (Teacher)teacherResponse.user;


        //    //Get classRoom data
        //    ClassRoomResponse classRoomResponse = TheDataStore.FetchClassRoom(classRoomName);

        //    ClassRoom classRoom = classRoomResponse.classRoom;

        //    // make view model

        //    TeacherClassRoomViewModel tCRVM = new TeacherClassRoomViewModel(teacher, classRoom);

        //    // return view

        //    return View(tCRVM);
        //}

        //[HttpPost]
        //public ActionResult New(CreateClassRoomModel model){

        //    Console.Write(model);

        //    return Json(new { });
        //}


        //public ActionResult Create(int id){

        //    UserResponse response = TheDataStore.FetchTeacher(id);

        //    Teacher teacher = (Teacher)response.user;

        //    CreateClassRoomView CCRV = new CreateClassRoomView(teacher);

        //    return View(CCRV);

        //}
    }
}