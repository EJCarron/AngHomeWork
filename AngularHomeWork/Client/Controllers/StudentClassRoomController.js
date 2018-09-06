
(function(app){

    var StudentClassRoomController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.classRoomName = $routeParams.name;



        $scope.$watch('selectedClassRoom', function(newValue, oldValue) {

            $scope.classRoom = newValue;
           
        });

        $scope.unsubBtnClicked = function() {

            $scope.$parent.unsubClassRoom($routeParams.name);
        }

        $scope.assignmentBtnClicked = function(assignmentId){

            $scope.$parent.setSelectedAssignment(assignmentId);

            $scope.$parent.getAssignment(assignmentId);
        }
    }

    app.controller("studentClassRoomController", StudentClassRoomController);

}(angular.module("app")));