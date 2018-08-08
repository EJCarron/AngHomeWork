(function(app){

    var TeacherPageController = function($scope, $mdSidenav, testModelCommand, $location , $rootScope){

        $scope.classRoomClicked = function(classRoomName) {
            $rootScope.selectedClassRoom = testModelCommand.getClassRoom(classRoomName);

            

        }
    }
    app.controller("teacherPageController", TeacherPageController);

}(angular.module("app")));
