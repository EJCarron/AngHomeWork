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
    public class StudentAssignmentController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage get(int assignmentId) {

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.studentAssignment, "", assignmentId), userId);

            if (!dataResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage changeDoneState(ChangeDoneStateCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.changeDoneState(cO.assignmentId, cO.currentDoneState, userId);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);

        }
    }
}
