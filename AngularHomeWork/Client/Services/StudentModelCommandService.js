
(function(app) {


    var modelCommand = function($http, magicStrings, $location,$mdDialog) {

        this.logout = function() {

            doLogoutHttpRequest();

        }

        this.getClassRoom = function(cRName, $scope) {

            doHttpGetRequest(magicStrings.subscriptionController, cRName, $scope);

        }


        this.getAssignment = function(assignmentId, $scope){

            doHttpGetRequest(magicStrings.studAssignmentController, assignmentId, $scope);
        }

        this.changeAssignmentDoneStatus = function(assignmentId, currentDoneState, classRoomName, $scope) {

            var commandObject = makeChangeDoneStateCO(assignmentId, currentDoneState, classRoomName);

            doHttpRequest(magicStrings.POST, magicStrings.studAssignmentController, magicStrings.changeDoneAction, $scope, commandObject, null);
        }


        this.subscribeToClassRoom = function(name, $scope){

            var commandObject = makeSubscribeCO(name);

            doHttpRequest(magicStrings.POST, magicStrings.subscriptionController, magicStrings.subscribeAction, $scope, commandObject, (magicStrings.subSucc + name));
        }

        this.unsubFromClassRoom = function(name, $scope){

            var commandObject = makeUnsubCO(name);

            doHttpRequest(magicStrings.PUT, magicStrings.subscriptionController, magicStrings.unsubscribeAction, $scope, commandObject, (magicStrings.unsubSucc + name));
        }

        this.getOutStandingAssignments = function($scope){

            doHttpGetRequest(magicStrings.outStandingController, null,  $scope);
        }

        this.showAlertDialog = function(message) {

            var $event = "";

            showDialog( $event, message, 2);

        } 


//-----------------------HttpRequests ---------------------------


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

        var doHttpGetRequest = function(controller, parameter, $scope){

            var paramaterPathExtenstion = parameter?( "/" + parameter):'';

            $http({
                method : "GET",
                url : (magicStrings.apiURL + controller + "/" + magicStrings.getAction + paramaterPathExtenstion)
                }).then(function success(response) {

                    dealWithGenericResponse(response.data, $scope)
                  
                }, function error(response){

                    var $event = "";

                    showDialog($event, response.data.Message, 2);

                });
        }

        


        var doHttpRequest = function(method, controller,action, $scope, commandObject, successMessage){

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
                   
                }, function error(response){

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

                    case 4: {
                        studentClassRoomArrived(subResponse, $scope)
                    }break;
                    case 5: {
                        subscriptionListArrived(subResponse, $scope)
                    }break;
                    case 6:{
                        studentAssignmentArrived(subResponse, $scope)
                    }break;
                    case 7:{
                        outStandingAssignmentsArrived(subResponse, $scope)
                    }   
                }

            }
        }

        var studentClassRoomArrived = function(subResponse, $scope){

            var classRoom = subResponse.modelObject;

            if ($scope.selectedClassRoomName == classRoom.name) {

                $scope.selectedClassRoom = classRoom;
            }

        }

        var subscriptionListArrived = function(subResponse, $scope){

            var subscriptions = subResponse.modelObject;

            $scope.subscriptions = subscriptions;

            $location.path("/StudentHome");
        }

        var studentAssignmentArrived = function(subResponse, $scope){

            var assignment = subResponse.modelObject;

            if($scope.selectedAssignmentId == assignment.id){

                $scope.selectedAssignment = assignment;
            }

        }

        var outStandingAssignmentsArrived = function(subResponse, $scope){

            var assignments = subResponse.modelObject;

            $scope.outStandingAssignments = assignments;
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

        var makeChangeDoneStateCO = function(assignmentId, currentDoneState, classRoomName) {

            var classRoomRequest = makeSubRequest(4,classRoomName,-1);

            var requestObject = makeRequestObject([classRoomRequest]);

            var cO = {
                requestObject: requestObject,
                assignmentId: assignmentId,
                currentDoneState: currentDoneState
                }

            return cO;
        }

        var makeSubscribeCO = function(name){

            var subscriptionListRequest = makeSubRequest(5,"",-1);

            var requestObject = makeRequestObject([subscriptionListRequest]);

            var cO = {
                requestObject: requestObject,
                classRoomName: name
                }

            return cO

        }

        var makeUnsubCO = function(name){

            var subscriptionListRequest = makeSubRequest(5,"",-1);

            var requestObject = makeRequestObject([subscriptionListRequest]);

            var cO = {
                requestObject: requestObject,
                classRoomName: name
                }

            return cO
        }

    }
    app.service('modelCommand',['$http','magicStrings','$location','$mdDialog', modelCommand]);


}(angular.module("app")));