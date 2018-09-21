

(function(app) {


    var ModelCommand = function($http, magicStrings, $mdDialog) {


        this.logout = function() {

            doLogoutHttpRequest();

        }

        this.getClassRoom = function(cRName, $scope) {

            doHttpGetRequest(magicStrings.classRoomController, cRName, $scope);

        }

        this.createClassRoom = function(newName, $scope, successHandler){

            var commandObject = makeCreateClassRoomCO(newName);

            doHttpRequest(magicStrings.POST, magicStrings.classRoomController, magicStrings.createAction, $scope, commandObject, magicStrings.crtClrSucc, successHandler);

        }

        this.changeClassRoomArchiveStatus = function(classRoomName, newArchiveStatus, $scope) {
            var commandObject = makeArchiveClassRoomCO(classRoomName, newArchiveStatus);

            doHttpRequest(magicStrings.PUT, magicStrings.classRoomController, magicStrings.archiveAction, $scope, commandObject);
        }


        this.getAssignment = function(assignmentId, $scope){

            doHttpGetRequest(magicStrings.assignmentController, assignmentId, $scope);
        }

        this.createAssignment = function(newName, classRoomName, newDueDate, newDescription, $scope){

            var commandObject = makeCreateAssignmentCO(newName, classRoomName, newDueDate, newDescription);

            doHttpRequest(magicStrings.POST, magicStrings.assignmentController, magicStrings.createAction, $scope, commandObject, magicStrings.crtAssSucc);

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
                  
                }, function error(response){

                    var $event = "";

                    showDialog($event, response.data.Message, 2);

                });
        }


        var doHttpRequest = function(method, controller,action, $scope, commandObject, successMessage, successHandler){

           var $event = "";


            $http({
                method : method,
                url : (magicStrings.apiURL + controller + "/" + action),
                data : commandObject
                }).then(function success(response) {

                    if(successMessage != null){
                       showDialog($event, successMessage, 1);
                    }

                    dealWithGenericResponse(response.data, $scope)

                    if (successHandler) {
                       
                        successHandler();
                    }
                   
                }, function error(response){

                    showDialog($event, response.data.Message, 2);

                });
        }

        var doLogoutHttpRequest = function() {

            $http({
                method : magicStrings.PUT,
                url : (magicStrings.URL + "Home/Logout")
                }).then(function success(response){
                
                    location.reload(true);

                }, function error(response){

                    var $event = "";

                    showDialog($event, response.data.Message, 2);

                });
        }

        var showDialog = function(ev, message, type) {

            var titleText = "";

            if(type == 1){

                titleText = "Success";
            }else{

                titleText = "Error";
            }

            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#studentPage')))
                    .clickOutsideToClose(true)
                    .title(titleText)
                    .textContent(message)
                    .ariaLabel('Alert Dialog')
                    .ok('Got it!')
                    .targetEvent(ev)
            );

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

       var makeCreateClassRoomCO = function(newName){
            
            var classRoomListRequest = makeSubRequest(2, "", -1);
            var classRoomRequest = makeSubRequest(1, newName, -1);

            var requestObject = makeRequestObject([classRoomListRequest, classRoomRequest]);

            var createClassRoomCO = {
                requestObject: requestObject,
                classRoomName: newName
            }

            return createClassRoomCO;

       }

        var makeArchiveClassRoomCO = function(classRoomName, newArchiveStatus) {

            var classRoomListRequest = makeSubRequest(2, "", -1);

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

    app.service('modelCommand',['$http','magicStrings','$mdDialog', ModelCommand]);

}(angular.module("app")));




