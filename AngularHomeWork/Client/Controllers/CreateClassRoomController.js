
(function(app){

    var CreateClassRoomController = function($scope, $http ){
       

        $scope.goBtnClicked = function() {

           $scope.$parent.createClassRoom($scope.newName)

        }
    }

    app.controller("createClassRoomController", CreateClassRoomController);

}(angular.module("app")));
