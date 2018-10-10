using System;
namespace AngularHomeWork.Models {
    public class Assignment {
        public int id;
		public string name;
        public string classRoomName;
        public string dueDate;
        public long dueDateTicks;
        public string description;



        public Assignment() {
        }
    }

    public class TeacherAssignment : Assignment{

        public string[] students;

        public TeacherAssignment(Assignment assignment, string[] students) {

            this.id = assignment.id;
            this.name = assignment.name;
            this.classRoomName = assignment.classRoomName;
            this.dueDate = assignment.dueDate;
            this.dueDateTicks = assignment.dueDateTicks;
            this.description = assignment.description;
            this.students = students;
        }
    }

    public class StudentAssignment : Assignment{
        public bool markedDone;

        public StudentAssignment(Assignment assignment, bool markedDone){

            this.id = assignment.id;
            this.name = assignment.name;
            this.classRoomName = assignment.classRoomName;
            this.dueDateTicks = assignment.dueDateTicks;
            this.dueDate = assignment.dueDate;
            this.description = assignment.description;
            this.markedDone = markedDone;
        }
    }
}
