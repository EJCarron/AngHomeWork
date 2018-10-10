using System;
namespace AngularHomeWork.Models {
    public class LoginPageModel : PageModel{
        

        public LoginPageModel(SystemSettings settings) {
            this.settings = settings;
        }
    }
}
