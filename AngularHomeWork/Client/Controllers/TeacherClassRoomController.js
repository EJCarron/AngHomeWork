
(function(app){

    var TeacherClassRoomController = function($scope,  $routeParams , modelCommand){

        $scope.name = $routeParams.name;

        $scope.$watch('selectedClassRoom', function(newValue, oldValue) {

            $scope.classRoom = newValue;
           
        });

    }

    app.controller("teacherClassRoomController", TeacherClassRoomController);

}(angular.module("app")));
