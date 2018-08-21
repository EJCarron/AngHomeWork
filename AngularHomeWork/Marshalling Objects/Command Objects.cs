using System;
namespace AngularHomeWork.MarshallingObjects {
    public class CommandObject {
        public RequestObject requestObject;


        public CommandObject() {
        }
    }


    public class CreateClassRoomCO : CommandObject{
        public int teacherId;
        public string classRoomName;

    }

    public class ArchiveClassRoomCO : CommandObject{
        public string classRoomName;
        public int newArchiveStatus;
    }
}
