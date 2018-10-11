using System;

namespace AngularHomeWork.Models {
    public class ClassRoom {
        
        public string name;


        public ClassRoom(){}
    }

    public class AssignmentListItem {
        public string title;
        public string dueDate;
        public int id;
        public string classRoomName;
    }






    public class TeacherClassRoom : ClassRoom{
        public string[] students;
        public TeacherAssignmentListItem[] assignments;


        public TeacherClassRoom(string name, TeacherAssignmentListItem[] assignments, string[] students) {
            this.name = name;
            this.assignments = assignments;
            this.students = students;
        }

    }

   

    public class TeacherAssignmentListItem : AssignmentListItem{
        
    }





    public class StudentClassRoom : ClassRoom{
        public StudentAssignmentListItem[] assignments;

        public StudentClassRoom(string name, StudentAssignmentListItem[] assignments) {
            this.name = name;
            this.assignments = assignments;

        }
    }

    public class StudentAssignmentListItem :AssignmentListItem{
        public bool markedDone;


        public StudentAssignmentListItem(string name, int id, string dueDate, bool markedDone){

            this.title = name;
            this.dueDate = dueDate;
            this.id = id;
            this.markedDone = markedDone;
        }
    }








}
