

(function(app) {


    var ModelCommand = function() {
        this.createClassroom = function(newCR, cRs) {
            
            cRs.push(newCR);
            return cRs;
        };

        var nameScramble = function(name){
            var scramble = "";

           for(var i = 0 ; i < name.length ; i++){
                for(var j = 0 ; j < i ; j++){
                    scramble += name[j+1] ;
                }
           }

            return scramble ;
        }

        this.getClassRoom = function(classRoomName){

            var essayName = "essay" + classRoomName;

            var workSheetName = nameScramble(classRoomName);

            var classRoom = {
                name : classRoomName ,
                assignments : [
                    {
                        title: essayName,
                        dueDate: "1/12/18"
                        },
                    {
                        title: workSheetName,
                        dueDate: "2/8/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        },{
                        title: essayName,
                        dueDate: "1/12/18"
                        }
                    ]            
                };

            return classRoom;

        }
           
    }

    app.service('testModelCommand', ModelCommand);

}(angular.module("app")));

