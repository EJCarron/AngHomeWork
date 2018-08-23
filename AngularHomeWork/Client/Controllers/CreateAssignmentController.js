
(function(app){

    var CreateAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.cRName = $routeParams.classRoomName;

        $scope.cancelBtnClicked = function(classRoomName){

            $scope.$parentScope.classRoomBtnClicked(classRoomName);
        }

        $scope.goBtnClicked = function() {
            $scope.$parent.createAssignment($scope.newName, $routeParams.classRoomName, $scope.newDueDate, $scope.newDescription)
        }
    }

    app.controller("createAssignmentController", CreateAssignmentController);

}(angular.module("app")));
