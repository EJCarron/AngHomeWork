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


namespace AngularHomeWork.Controllers {
    [Authorize(Roles = "Teacher")]
    public class ClassRoomController : ApiController {
        
        [HttpGet]
        public HttpResponseMessage get(string classRoomName){
            
            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.classRoom, classRoomName, -1), userId);

            if(!dataResponse.response.isOk){
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            }else{
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage create(CreateClassRoomCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.createClassRoom(userId, cO.classRoomName);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);

        }

        [HttpPut]
        public HttpResponseMessage archive(ArchiveClassRoomCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.changeClassRoomArchiveStatus(cO.classRoomName, cO.newArchiveStatus);


            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);


        }


        //public ActionResult Create(int id){

        //    UserResponse response = TheDataStore.FetchTeacher(id);

        //    Teacher teacher = (Teacher)response.user;

        //    CreateClassRoomView CCRV = new CreateClassRoomView(teacher);

        //    return View(CCRV);

        //}
    }
}