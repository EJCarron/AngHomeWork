using System;
using System.Collections.ObjectModel;
namespace AngularHomeWork.Models {
    public class User {
        public int id { get; set; }
        public string name { get; set; }
        public int userType { get; set; }
        public int schoolId { get; set; }

    }

    public class UserResponse {
        public User user { get; set; }
        public Response response { get; set; }

        public UserResponse() {
            this.response = new Response();
        }

    }

    public class Teacher : User {

        public Collection<ClassRoomListItem> classRooms = new Collection<ClassRoomListItem>();

        public Teacher(int id, Collection<ClassRoomListItem> classRooms) {
            this.classRooms = classRooms;
            this.id = id;
        }
    }

    public class ClassRoomListItem {
        public string classRoomName { get; set; }
        public string nextDueDate { get; set; }


        public ClassRoomListItem(string classRoomName, string nextDueDate) {
            this.classRoomName = classRoomName;
            this.nextDueDate = nextDueDate;
        }



    }
}
