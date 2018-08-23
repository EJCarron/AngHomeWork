
(function(app){

    var EditAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.assignment = $scope.selectedAssignment;

        

        $scope.goBtnClicked = function(){


            $scope.editAssignment($scope.assignment);
        }

        $scope.deleteBtnClicked = function(ev) {
            
            var confirm = $mdDialog.confirm()
                .title('Are you sure that you want to delete this assignment?')
                .textContent('You can un-archive from home screen.')
                .ariaLabel('Sure?')
                .targetEvent(ev)
                .ok('Delete')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function() {
                $scope.archiveAssignment($scope.assignment);
            });
        };
    }

    app.controller("editAssignmentController", EditAssignmentController);

}(angular.module("app")));