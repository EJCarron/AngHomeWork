(function(app){

    var TeacherPageController = function($scope, $mdSidenav, modelCommand, $location , $rootScope){

        $scope.classRoomClicked = function(classRoomName) {
            $rootScope.selectedClassRoom = modelCommand.getClassRoom(classRoomName);

            

        }
    }
    app.controller("teacherPageController", TeacherPageController);

}(angular.module("app")));
