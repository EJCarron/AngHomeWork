

(function(app) {


    var ModelCommand = function() {
        this.createClassroom = function(newCR, cRs) {
            
            cRs.push(newCR);
            return cRs;
        };

        this.getClassRoom = function(classRoomName){

            var classRoom = {
                name : classRoomName ,
                assignments : [
                    {
                        title: "essay",
                        dueDate: "1/12/18"
                        },
                    {
                        title: "worksheet",
                        dueDate: "2/8/18"
                    }
                    ]            
                };

            return classRoom;

        }
           
    }

    app.service('testModelCommand', ModelCommand);

}(angular.module("app")));

