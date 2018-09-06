
(function(app){

    var StudentPageController = function($scope,  $routeParams , modelCommand, $mdDialog, nullObjects){


        $scope.selectedClassRoom = nullObjects.classRoom;


        $scope.logoutBtnClicked = function(){

            modelCommand.logout()
        }


        $scope.subscriptionBtnClicked = function(classRoomName) {

            $scope.setSelectedClassRoom(classRoomName);

            modelCommand.getClassRoom(classRoomName, $scope);

        }

       $scope.setSelectedClassRoom = function(classRoomName){

            $scope.selectedClassRoomName = classRoomName;

            $scope.selectedClassRoom = nullObjects.classRoom;
            
            
       }

        $scope.getAssignment = function(assignmentId){

            $scope.setSelectedAssignment(assignmentId);


            modelCommand.getAssignment(assignmentId, $scope);
            
        }


        $scope.setSelectedAssignment = function(assignmentId){

            $scope.selectedAssignmentId = assignmentId;

            $scope.selectedAssignment = nullObjects.assignment;
        }

        $scope.changeAssignmentDoneStatus = function(assignmentId, currentDoneState, classRoomName) {

            modelCommand.changeAssignmentDoneStatus(assignmentId, currentDoneState, classRoomName, $scope);
        }


        $scope.subscribeToClassRoom = function(name){

            modelCommand.subscribeToClassRoom(name, $scope);
        }

        $scope.unsubClassRoom = function(name){

            modelCommand.unsubFromClassRoom(name, $scope);
        }

        $scope.getOutStandingAssignments = function(){

            modelCommand.getOutStandingAssignments($scope);
        }
    }

    app.controller("studentPageController", StudentPageController);

}(angular.module("app")));