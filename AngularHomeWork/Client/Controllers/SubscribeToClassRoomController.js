
(function(app){

    var SubscribeToClassRoomController = function($scope){

        $scope.goBtnClicked = function() {

           $scope.$parent.subscribeToClassRoom($scope.name)

        }
    }

    app.controller("subscribeToClassRoomController", SubscribeToClassRoomController);

}(angular.module("app")));