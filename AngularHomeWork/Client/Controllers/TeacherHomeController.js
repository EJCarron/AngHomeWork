
(function(app){

    var TeacherHomeController = function($scope, modelCommand ){
        
        $scope.$parent.getTodaysAssignments();

        $scope.assignmentBtnClicked = function(assignmentId){

            $scope.$parent.setSelectedAssignment(assignmentId);

            modelCommand.getAssignment(assignmentId, $scope.$parent);
        }
  
    }

    app.controller("teacherHomeController", TeacherHomeController);

}(angular.module("app")));
