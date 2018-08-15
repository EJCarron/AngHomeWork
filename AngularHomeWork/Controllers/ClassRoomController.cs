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


            //Response response = new Response();

            //ClassRoom classRoom = (ClassRoom)TheDataStore.FetchClassRoom(new SubRequest(RequestType.classRoom, classRoomName), response).modelObject;

            //if (!response.isOk) {
            //    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, response.message);
            //} else {
            //    return Request.CreateResponse(System.Net.HttpStatusCode.OK, classRoom);
            //}

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.classRoom, classRoomName));

            if(!dataResponse.response.isOk){
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            }else{
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage create(CreateClassRoomCO cO){

            Response response = TheDataStore.createClassRoom(cO.teacherId, cO.classRoomName);

            if (!response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, response.message);
            } else {


                DataResponse dataResponse = TheDataStore.getData(cO.requestObject);


                if (!dataResponse.response.isOk) {
                    return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
                } else {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
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