
(function(app){

    var TeacherClassRoomController = function($scope, $rootScope, $routeParams ){

        $scope.name = $routeParams.name;

        $scope.classRoom = $rootScope.selectedClassRoom;

    }

    app.controller("teacherClassRoomController", TeacherClassRoomController);

}(angular.module("app")));
