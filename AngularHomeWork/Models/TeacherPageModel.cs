using System;
namespace AngularHomeWork.Models {
    public class TeacherPageModel : PageModel {
        public Teacher teacher;



        public TeacherPageModel(Teacher teacher, SystemSettings settings) {
            this.teacher = teacher;
            this.settings = settings;
        }
    }
}
