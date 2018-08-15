

(function(app) {


    var ModelCommand = function($http) {
        this.createClassroom = function(newCR, cRs) {

            $http({
                method : "POST",
                data : {
                    hello : "hello1",
                    world : "world1"
                },
                url : "http://127.0.0.1:8080/ClassRoom/Create"
            }).then(function success(response) {
                
            }, function error(response) {
                
            });

        }

        this.getClassRoom = function(cRName, $scope) {
            $http({
                method : "GET",
                url : ("http://127.0.0.1:8080/api/ClassRoom/get/"+cRName)
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                   
                    
                });
        }

        this.createClassRoom = function(newName, teacherId, $scope){

            var commandObject = makeCreateClassRoomCO(newName, teacherId);

            $http({
                method : "POST",
                url : ("http://127.0.0.1:8080/api/ClassRoom/create"),
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
                        
                }

            }
        }


        var classRoomArrived = function(subResponse, $scope) {

            var classRoom = subResponse.modelObject;

            if ($scope.selectedClassRoomName == classRoom.name) {

                $scope.selectedClassRoom = classRoom;
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
                SubRequests: subRequests
            }

            return requestObject;

       }

       var makeCreateClassRoomCO = function(newName, teacherId){
            
            var classRoomListRequest = makeSubRequest(2, null, teacherId);
            var getClassRoomRequest = makeSubRequest(1, newName, null);

            var requestObject = makeRequestObject([classRoomListRequest, getClassRoomRequest]);

            var createClassRoomCO = {
                requestObject: requestObject,
                teacherId: teacherId,
                classRoomName: newName
            }

            return createClassRoomCO;

       }


    }

    app.service('modelCommand',['$http', ModelCommand]);

}(angular.module("app")));




