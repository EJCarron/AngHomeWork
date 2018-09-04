using System;
namespace AngularHomeWork.MarshallingObjects {
    public class CommandObject {
        public RequestObject requestObject;


        public CommandObject() {
        }
    }

    public class RegisterUserCO : CommandObject{
        public string emailAddress;
        public string name;
        public UserType type;
        public string password;
        public int schoolId;

    }

    public class CreateClassRoomCO : CommandObject{
        
        public string classRoomName;

    }

    public class ArchiveClassRoomCO : CommandObject{
        public string classRoomName;
        public int newArchiveStatus;
    }

    public class CreateAssignmentCO : CommandObject{
        public string newName;
        public string classRoomName;
        public DateTime newDueDate;
        public string newDescription;

    }

    public class EditAssignmentCO : CommandObject{
        public int id;
        public string newName;
        public DateTime newDueDate;
        public string newDescription;
        public int newArchiveStatus;

    }
}
