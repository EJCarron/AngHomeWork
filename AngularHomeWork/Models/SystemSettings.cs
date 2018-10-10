using System;
namespace AngularHomeWork.Models {
    public class SystemSettings {
        public string schoolName;
        public string phoneNumber;

        public SystemSettings(string schoolName, string phoneNumber) {

            this.schoolName = schoolName;
            this.phoneNumber = phoneNumber;
        }
    }

    public class Setting {
        public int id;
        public string value;

        public Setting(int id, string value){
            this.id = id;
            this.value = value;
        }

    }
}
