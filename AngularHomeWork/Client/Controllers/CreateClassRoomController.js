
(function(app){

    var CreateClassRoomController = function($rootScope,$scope, modelCommand, $location ){
       

        $scope.goBtnClicked = function() {

            $scope.classRooms = modelCommand.createClassroom($scope.newName, $scope.classRooms);

            $rootScope.currentCR = {name : $scope.newName};

            $location.path("/TeacherClassRoom");
        }
    }

    app.controller("createClassRoomController", CreateClassRoomController);

}(angular.module("app")));
