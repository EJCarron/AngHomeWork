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

namespace AngularHomeWork.Controllers
{
    [Authorize(Roles = "Student")]
    public class SubscriptionController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage get(string classRoomName) {

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.studentClassRoom, classRoomName, -1), userId);

            if (!dataResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage subscribe(SubscribeCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.subscribeToClassRoom(userId, cO.classRoomName);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);

        }

        [HttpPut]
        public HttpResponseMessage unsubscribe(UnsubscribeCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.unsubscribeToClassRoom(userId, cO.classRoomName);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);
        }
    }
}
