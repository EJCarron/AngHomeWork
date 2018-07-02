
(function(app){

    var TeacherHomeController = function($scope ){
        var output = "";

        var names = $scope.classrooms;

        var len = names.length ;

        for (var i = 0 ; i < len ; i++){
            
            var name = names[i];

            output += name[0];
        }

        $scope.firstOfEach = output;
  
    }

    app.controller("teacherHomeController", TeacherHomeController);

}(angular.module("app")));
