

(function(app) {


    var ModelCommand = function($http, magicStrings) {
        
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

//-----------------------Http Request--------------------------


        var doHttpGetRequest = function(controller, parameter, $scope){

            $http({
                method : "GET",
                url : (magicStrings.URL + controller + "/" + magicStrings.getAction + "/" + parameter)
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                  
                });
        }


        var doHttpRequest = function(method, controller,action, $scope, commandObject){

            $http({
                method : method,
                url : (magicStrings.URL + controller + "/" + action),
                data : commandObject
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                   
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


    }

    app.service('modelCommand',['$http','magicStrings', ModelCommand]);

}(angular.module("app")));




