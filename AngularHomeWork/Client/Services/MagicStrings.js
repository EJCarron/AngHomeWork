
(function(app) {


    var magicStrings = function() {

        this.PUT = "PUT";
        this.POST = "POST"

        this.URL = "http://127.0.0.1:8080/api/";

        this.classRoomController = "ClassRoom";
        this.assignmentController = "Assignment";

        this.archiveAction = "archive";
        this.createAction = "create";
        this.getAction = "get";

        this.archiveCode = 1;
        this.unarchiveCode = 0;
        }

    app.service('magicStrings',[magicStrings]);


}(angular.module("app")));