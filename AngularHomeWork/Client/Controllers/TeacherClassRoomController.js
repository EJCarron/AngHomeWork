
(function(app){

    var TeacherClassRoomController = function($scope, $rootScope ){

        $scope.name = $rootScope.selectedClassRoom.name;

        

    }

    app.controller("teacherClassRoomController", TeacherClassRoomController);

}(angular.module("app")));
