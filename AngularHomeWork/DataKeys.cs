using System;
namespace AngularHomeWork {
    public static class DataKeys {         public const string dataBaseConnectionString = "SERVER=mysql5.loosefoot.com;UID=edcarron102;PWD=futurama4;database=paper;SslMode=none";

    }

    public enum RequestType {
            none = 0,
            classRoom = 1,
            classRoomList = 2,
            assignment = 3,
            studentClassRoom = 4,
            subscriptionList = 5,
            studentAssignment = 6,
            outStandingAssignments = 7
    }

    public enum UserType {

        none = 0,
        teacher = 1,
        student = 2
    }

}
