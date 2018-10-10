
(function(app){

    var CreateAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.cRName = $routeParams.classRoomName;

        $scope.cancelBtnClicked = function(classRoomName){

            $scope.$parent.classRoomBtnClicked(classRoomName);
        }

        $scope.goBtnClicked = function() {

            if(
                ($scope.newName == null)
                ||
                ($scope.newDueDate == null)
                ||
                ($scope.newDescription == null)
            ) {
                $scope.$parent.showAlertDialog("All fields must be completed");
            }else{

                var dueDateTicks = (($scope.newDueDate.getTime() * 10000) + 621356832000000000);

                $scope.$parent.createAssignment($scope.newName, $routeParams.classRoomName, dueDateTicks, $scope.newDescription)
            }
        }
    }

    app.controller("createAssignmentController", CreateAssignmentController);

}(angular.module("app")));
