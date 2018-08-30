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


    public class LoginResponse{
        public int userId;
        public bool success;
        public UserType userType;
        public Response response;


        public LoginResponse(){
            this.response = new Response();
        }
    }

    public class UserTypeResponse{
        public Response response;
        public UserType type;

        public UserTypeResponse(){
            this.response = new Response();
        }
    }

    public class Teacher : User {



        public string[] classRoomNames;

        public Teacher(int id, string[] classRoomNames ) {

            this.classRoomNames = classRoomNames;
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
