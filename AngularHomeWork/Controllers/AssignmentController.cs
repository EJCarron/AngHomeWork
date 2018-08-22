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
    public class AssignmentController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage get(int assignmentId) {

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.assignment, assignmentId));

            if (!dataResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }
    }
}
