
(function(app){

    var TeacherClassRoomController = function($scope, $rootScope, $routeParams ){

        $scope.name = $routeParams.name;

        $scope.assignments = $rootScope.selectedClassRoom.assignments;

    }

    app.controller("teacherClassRoomController", TeacherClassRoomController);

}(angular.module("app")));
