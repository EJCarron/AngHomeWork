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
    public class ClassRoomController : ApiController {
        
        [HttpGet]
        public HttpResponseMessage get(string classRoomName){
            
            ClassRoomResponse classRoomResponse = (ClassRoomResponse)TheDataStore.getData(new SubRequest(RequestType.assignmentsForClassRoom, classRoomName));

            if(!classRoomResponse.response.isOk){
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, classRoomResponse.response.message);
            }else{
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, classRoomResponse.classRoom);
            }

        }

        [HttpPost]
        public HttpResponseMessage create(CreateClassRoomCO cO){

            Response response = TheDataStore.createClassRoom(cO.teacherId, cO.classRoomName);

            if (!response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, response.message);
            } else {
                ClassRoomResponse classRoomResponse = (ClassRoomResponse)TheDataStore.getData(cO.requestObject.subRequests[0]);

                if (!classRoomResponse.response.isOk) {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, classRoomResponse.response.message);
                } else {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, classRoomResponse.classRoom);
                }
            }

        }


        //public ActionResult Create(int id){

        //    UserResponse response = TheDataStore.FetchTeacher(id);

        //    Teacher teacher = (Teacher)response.user;

        //    CreateClassRoomView CCRV = new CreateClassRoomView(teacher);

        //    return View(CCRV);

        //}
    }
}