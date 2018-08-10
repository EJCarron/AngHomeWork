using System;
namespace AngularHomeWork.MarshallingObjects {
    public class RequestObject {
        public SubRequest[] subRequests;
        public RequestObject() {
        }
    }

    public class SubRequest{
        public RequestType requestType;
        public string name;
        public int id;

        public SubRequest(RequestType requestType, string name){
            this.requestType = requestType;
            this.name = name;
        }
      
    }

    //public class SubRequest_ClassRoom : SubRequest{
    //    public string classRoomName;

    //    public SubRequest_ClassRoom(string classRoomName){
    //        this.classRoomName = classRoomName;
    //        this.requestType = RequestType.assignmentsForClassRoom;
    //    }
    //}
}
