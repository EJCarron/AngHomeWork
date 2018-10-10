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
    [Authorize (Roles="Teacher")]
    public class AssignmentController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage get(int assignmentId) {


            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.assignment,"", assignmentId), userId);

            if (!dataResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage create(CreateAssignmentCO cO) {


            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
            
            Response response = TheDataStore.createAssignment(cO.newName, cO.classRoomName, cO.newDueDateTicks, cO.newDescription);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);

        }

        [HttpPut]
        public HttpResponseMessage edit(EditAssignmentCO cO){

            int userId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);

            Response response = TheDataStore.editAssignment(cO.id, cO.newName, cO.newDueDateTicks, cO.newDescription, cO.newArchiveStatus);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request, userId);
        }
    }
}
