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

            DataResponse dataResponse = TheDataStore.getData(new SubRequest(RequestType.assignment,"", assignmentId));

            if (!dataResponse.response.isOk) {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, dataResponse.response.message);
            } else {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, dataResponse.responseObject);
            }

        }

        [HttpPost]
        public HttpResponseMessage create(CreateAssignmentCO cO) {
            
            Response response = TheDataStore.createAssignment(cO.newName, cO.classRoomName, cO.newDueDate, cO.newDescription);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request);

        }

        [HttpPut]
        public HttpResponseMessage edit(EditAssignmentCO cO){

            Response response = TheDataStore.editAssignment(cO.id, cO.newName, cO.newDueDate, cO.newDescription, cO.newArchiveStatus);

            return TheDataStore.makeHttpResponseMessage(response, cO.requestObject, Request);
        }
    }
}
