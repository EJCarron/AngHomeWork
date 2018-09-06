


(function(app) {


    var nullObjects = function() {

            this.classRoom = {
            assignments:{},
            name:""
            }

        this.assignment = {
            id:-1,
            name:"",
            classRoomName:"",
            dueDate:"",
            description:""
            }

        this.studentAssignment = {
            id:-1,
            name:"",
            classRoomName:"",
            dueDate:"",
            description:"",
            markedDone:null
        }

        }
    app.service('nullObjects',[nullObjects]);


}(angular.module("app")));




