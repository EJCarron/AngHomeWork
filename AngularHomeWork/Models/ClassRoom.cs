using System;

namespace AngularHomeWork.Models {
    public class ClassRoom {
        public string name;
        public AssignmentListItem[] assignments;

        public ClassRoom(string name, AssignmentListItem[] assignments) {
            this.name = name;
            this.assignments = assignments;
        }
    }

    public class AssignmentListItem{
        public string name;
        public string dueDate;
        public int id;

    }

    public class ClassRoomResponse{
        public ClassRoom classRoom;
        public Response response;

        public ClassRoomResponse(){
            this.response = new Response();
        }
    }
}
