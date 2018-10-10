
(function(app){

    var CreateClassRoomController = function($scope, $http ){
       

        $scope.goBtnClicked = function() {

            if ($scope.newName == null) {
                $scope.$parent.showAlertDialog("Classroom name field must be completed.");
            }else{
                $scope.$parent.createClassRoom($scope.newName)
            }
        }
    }

    app.controller("createClassRoomController", CreateClassRoomController);

}(angular.module("app")));
