
(function(app){

    var SubscribeToClassRoomController = function($scope){

        $scope.goBtnClicked = function() {
            if ($scope.name == null) {
                $scope.$parent.showAlertDialog("The classroom name field must completed.");
            }else{
                $scope.$parent.subscribeToClassRoom($scope.name);
            }
        }
    }

    app.controller("subscribeToClassRoomController", SubscribeToClassRoomController);

}(angular.module("app")));