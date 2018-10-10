using System;
namespace AngularHomeWork.Models {
    public class StudentPageModel : PageModel {
        public Student student;


        public StudentPageModel(Student student, SystemSettings settings) {
            this.settings = settings;
            this.student = student;
        }
    }
}
