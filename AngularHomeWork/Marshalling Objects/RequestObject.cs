using System;
namespace AngularHomeWork.MarshallingObjects {
    public class RequestObject {
        public SubRequest[] subRequests;
        public RequestObject(params SubRequest[] subRequests) {
            this.subRequests = subRequests;
        }
    }

    public class SubRequest{
        public RequestType requestType;
        public string name;
        public int id;

        public SubRequest(RequestType requestType, string name, int id){
            this.requestType = requestType;
            this.name = name;
            this.id = id;
        }

        //public SubRequest(RequestType requestType, int id){
        //    this.requestType = requestType;
        //    this.id = id;
        //}
      
    }

    public class ResponseObject{
        public SubResponse[] subResponses;

        public ResponseObject(SubResponse[] subResponses){
            this.subResponses = subResponses;
        }
    }


    public class SubResponse{
        public RequestType requestType;

        public object modelObject;
    }




    //public class SubRequest_ClassRoom : SubRequest{
    //    public string classRoomName;

    //    public SubRequest_ClassRoom(string classRoomName){
    //        this.classRoomName = classRoomName;
    //        this.requestType = RequestType.assignmentsForClassRoom;
    //    }
    //}
}
