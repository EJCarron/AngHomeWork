
(function(app){

    var TeacherClassRoomController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.name = $routeParams.name;

        $scope.$watch('selectedClassRoom', function(newValue, oldValue) {

            $scope.classRoom = newValue;
           
        });

        $scope.deleteBtnClicked = function(ev) {
            
            var confirm = $mdDialog.confirm()
                .title('Are you sure that you want to delete this classRoom?')
                .textContent('You can un-archive from home screen.')
                .ariaLabel('Sure?')
                .targetEvent(ev)
                .ok('Delete')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function() {
                $scope.$parent.archiveClassRoom($scope.name);
            });
        };


        $scope.assignmentBtnClicked = function(assignmentId){

            $scope.$parent.setSelectedAssignment(assignmentId);

            modelCommand.getAssignment(assignmentId, $scope.$parent);
        }

    }

    app.controller("teacherClassRoomController", TeacherClassRoomController);

}(angular.module("app")));
