


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

        }
    app.service('nullObjects',[nullObjects]);


}(angular.module("app")));




