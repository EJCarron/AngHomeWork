﻿using System;
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
        public long newDueDateTicks;
        public string newDescription;

    }

    public class EditAssignmentCO : CommandObject{
        public int id;
        public string newName;
        public long newDueDateTicks;
        public string newDescription;
        public int newArchiveStatus;

    }


    public class ChangeDoneStateCO: CommandObject{
        public int assignmentId;
        public bool currentDoneState;
    }

    public class SubscribeCO : CommandObject{
        public string classRoomName;
    }

    public class UnsubscribeCO : CommandObject {
        public string classRoomName;
    }
}
