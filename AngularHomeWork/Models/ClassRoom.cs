using System;

namespace AngularHomeWork.Models {
    public class ClassRoom {
        public string name;
        public AssignmentListItem[] assignments;
        public string[] students;

        public ClassRoom(string name, AssignmentListItem[] assignments, string[] students) {
            this.name = name;
            this.assignments = assignments;
            this.students = students;
        }
    }

    public class AssignmentListItem{
        public string title;
        public string dueDate;
        public int id;

    }


}
