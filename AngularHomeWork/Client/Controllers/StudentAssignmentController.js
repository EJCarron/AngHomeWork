
(function(app){

    var StudentAssignmentController = function($scope,  $routeParams , $mdDialog, nullObjects){


        $scope.doneText = "";

        $scope.assignment = nullObjects.studentAssignment;

        $scope.markDoneBtnClicked = function(){

            $scope.$parent.changeAssignmentDoneStatus($routeParams.assignmentId, $scope.assignment.markedDone, $scope.assignment.classRoomName);
        }

        
        $scope.assignmentId = $routeParams.assignmentId;

        $scope.backBtnClicked = function(classRoomName){

            $scope.$parent.subscriptionBtnClicked(classRoomName);
        }



        $scope.$watch('selectedAssignment', function(newValue, oldValue) {

            $scope.assignment = newValue;

            if (newValue.markedDone) {

                $scope.doneText = "Undo";
            } else {
                $scope.doneText = "DONE";
            }
           
        });

        $scope.classRoom = $scope.selectedClassRoomName;
    }

    app.controller("studentAssignmentController", StudentAssignmentController);

}(angular.module("app")));