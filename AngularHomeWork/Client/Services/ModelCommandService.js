

(function(app) {


    var ModelCommand = function($http, magicStrings) {


        this.logout = function() {

            doLogoutHttpRequest();

        }

        this.getClassRoom = function(cRName, $scope) {

            doHttpGetRequest(magicStrings.classRoomController, cRName, $scope);

        }

        this.createClassRoom = function(newName, teacherId, $scope){

            var commandObject = makeCreateClassRoomCO(newName, teacherId);

            doHttpRequest(magicStrings.POST, magicStrings.classRoomController, magicStrings.createAction, $scope, commandObject);

        }

        this.changeClassRoomArchiveStatus = function(teacherId, classRoomName, newArchiveStatus, $scope) {
            var commandObject = makeArchiveClassRoomCO(classRoomName, teacherId, newArchiveStatus);

            doHttpRequest(magicStrings.PUT, magicStrings.classRoomController, magicStrings.archiveAction, $scope, commandObject );
        }


        this.getAssignment = function(assignmentId, $scope){

            doHttpGetRequest(magicStrings.assignmentController, assignmentId, $scope);
        }

        this.createAssignment = function(newName, classRoomName, newDueDate, newDescription, $scope){

            var commandObject = makeCreateAssignmentCO(newName, classRoomName, newDueDate, newDescription);

            doHttpRequest(magicStrings.POST, magicStrings.assignmentController, magicStrings.createAction, $scope, commandObject);

        }

        this.editAssignment = function(assignment, $scope){

            var commandObject = makeEditAssignmentCO(assignment.id, assignment.name, assignment.dueDate, assignment.description, 0, assignment.classRoomName);

            doHttpRequest(magicStrings.PUT, magicStrings.assignmentController, magicStrings.editAction, $scope, commandObject);
        }

        this.changeAssignmentArchiveStatus = function(assignment, newArchiveStatus, $scope){

            var commandObject = makeEditAssignmentCO(assignment.id, assignment.name, assignment.dueDate, assignment.description, newArchiveStatus, assignment.classRoomName);

            doHttpRequest(magicStrings.PUT, magicStrings.assignmentController, magicStrings.editAction, $scope, commandObject);
        }

//-----------------------Http Request--------------------------


        var doHttpGetRequest = function(controller, parameter, $scope){

            $http({
                method : "GET",
                url : (magicStrings.apiURL + controller + "/" + magicStrings.getAction + "/" + parameter)
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                  
                });
        }


        var doHttpRequest = function(method, controller,action, $scope, commandObject){

            $http({
                method : method,
                url : (magicStrings.apiURL + controller + "/" + action),
                data : commandObject
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                   
                });
        }

        var doLogoutHttpRequest = function() {

            $http({
                method : magicStrings.PUT,
                url : (magicStrings.URL + "Home/Logout")
                }).then(function success(response){
                
                    location.reload(true);

                });
        }


//----------------------GENERIC RESPONSES ----------------------

        var dealWithGenericResponse = function(responseObject, $scope) {

            var numResponses = responseObject.subResponses.length;

            for (var i = 0; i < numResponses; i++) {

                var subResponse = responseObject.subResponses[i];

                switch (subResponse.requestType) {

                    case 1: {
                        classRoomArrived(subResponse, $scope)
                    }break;
                    case 2: {
                        classRoomListArrived(subResponse, $scope)
                    }break;
                    case 3:{
                        assignmentArrived(subResponse, $scope)
                    }    
                }

            }
        }


        var classRoomArrived = function(subResponse, $scope) {

            var classRoom = subResponse.modelObject;

            if ($scope.selectedClassRoomName == classRoom.name) {

                $scope.selectedClassRoom = classRoom;
            }

        }

        var classRoomListArrived = function(subResponse, $scope){

            var classRoomList = subResponse.modelObject;

            $scope.classRooms = classRoomList;
        }

        var assignmentArrived = function(subResponse, $scope){

            var assignment = subResponse.modelObject;

            if($scope.selectedAssignmentId == assignment.id){

                $scope.selectedAssignment = assignment;
            }
        }

//----------------------COMMAND OBJECTS -------------------------------

       var makeSubRequest = function(type, name, id){

            var subRequest = {
                requestType: type,
                name: name,
                id: id
            }

            return subRequest;
       }


       var makeRequestObject = function(subRequests){

            var requestObject = {
                subRequests: subRequests
            }

            return requestObject;

       }

       var makeCreateClassRoomCO = function(newName, teacherId){
            
            var classRoomListRequest = makeSubRequest(2, "", teacherId);
            var classRoomRequest = makeSubRequest(1, newName, -1);

            var requestObject = makeRequestObject([classRoomListRequest, classRoomRequest]);

            var createClassRoomCO = {
                requestObject: requestObject,
                teacherId: teacherId,
                classRoomName: newName
            }

            return createClassRoomCO;

       }

        var makeArchiveClassRoomCO = function(classRoomName, teacherId, newArchiveStatus) {

            var classRoomListRequest = makeSubRequest(2, "", teacherId);

            var requestObject = makeRequestObject([classRoomListRequest]);

            var archiveClassRoomCO = {
                requestObject: requestObject,
                classRoomName: classRoomName,
                newArchiveStatus: newArchiveStatus
                }

            return archiveClassRoomCO;
        }

        var makeCreateAssignmentCO = function(newName, classRoomName, newDueDate, newDescription){

            var classRoomRequest = makeSubRequest(1, classRoomName, -1);

            var requestObject = makeRequestObject([classRoomRequest]);

            var createAssignmentCO = {
                requestObject: requestObject,
                newName: newName,
                classRoomName: classRoomName,
                newDueDate: newDueDate,
                newDescription: newDescription
            }


            return createAssignmentCO;
        }


        var makeEditAssignmentCO = function(id, newName, newDueDate, newDescription, newArchiveStatus, classRoomName){

            var assignmentRequest =  makeSubRequest(1,classRoomName,-1);

            var requestObject = makeRequestObject([assignmentRequest]);

            var editAssignmentCO = {
                requestObject: requestObject,
                id: id,
                newName: newName,
                newDueDate: newDueDate,
                newDescription: newDescription,
                newArchiveStatus: newArchiveStatus        
            }

            return editAssignmentCO; 
        }


    }

    app.service('modelCommand',['$http','magicStrings', ModelCommand]);

}(angular.module("app")));




