
(function(app){

    var StudentHomeController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.$parent.getTodaysAssignments();

        $scope.assignmentBtnClicked = function(assignmentId){

            $scope.$parent.setSelectedAssignment(assignmentId);

            $scope.$parent.getAssignment(assignmentId);
        }
    }

    app.controller("studentHomeController", StudentHomeController);

}(angular.module("app")));