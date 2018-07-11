(function(app){

    var TeacherPageController = function($scope, $mdSidenav, testModelCommand, $location ){

        $scope.classRoomClicked() = function(classRoomName) {
            $scope.selectedClassRoom = testModelCommand.getClassRoom(classRoomName);

            $location.path("/TeacherClassRoom")
        }
    }
    app.controller("teacherPageController", TeacherPageController);

}(angular.module("app")));
