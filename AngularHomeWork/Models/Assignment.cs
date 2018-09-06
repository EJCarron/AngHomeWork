using System;
namespace AngularHomeWork.Models {
    public class Assignment {
        public int id;
		public string name;
        public string classRoomName;
        public string dueDate;
        public string description;



        public Assignment() {
        }
    }

    public class TeacherAssignment : Assignment{}

    public class StudentAssignment : Assignment{
        public bool markedDone;

        public StudentAssignment(Assignment assignment, bool markedDone){

            this.id = assignment.id;
            this.name = assignment.name;
            this.classRoomName = assignment.classRoomName;
            this.dueDate = assignment.dueDate;
            this.description = assignment.description;
            this.markedDone = markedDone;
        }
    }
}
