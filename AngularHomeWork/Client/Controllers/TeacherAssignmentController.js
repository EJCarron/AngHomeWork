
(function(app){

    var TeacherAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.assignmentId = $routeParams.assignmentId;


        $scope.$watch('selectedAssignment', function(newValue, oldValue) {

            $scope.assignment = newValue;
           
        });
    }

    app.controller("teacherAssignmentController", TeacherAssignmentController);

}(angular.module("app")));
