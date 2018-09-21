
(function(app){

    var TeacherAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.assignmentId = $routeParams.assignmentId;

        $scope.backBtnClicked = function(classRoomName){

            $scope.$parent.classRoomBtnClicked(classRoomName);
        }



        $scope.$watch('selectedAssignment', function(newValue, oldValue) {

            $scope.assignment = newValue;
           
        });
    }

    app.controller("teacherAssignmentController", TeacherAssignmentController);

}(angular.module("app")));
