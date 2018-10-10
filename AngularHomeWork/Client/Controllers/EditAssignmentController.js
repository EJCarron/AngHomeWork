
(function(app){

    var EditAssignmentController = function($scope,  $routeParams , modelCommand, $mdDialog){

        $scope.assignment = $scope.selectedAssignment;

       

        $scope.dueDate = new Date(
            ($scope.assignment.dueDateTicks / 10000)-(Math.abs(new Date(0, 0, 1).setFullYear(1))));

        $scope.goBtnClicked = function(){

            if(
                ($scope.assignment.name == null)
                ||
                ($scope.assignment.dueDate == null)
                ||
                ($scope.assignment.description == null)
            ) {
                $scope.$parent.showAlertDialog("All fields must be completed.");
            }else{

                var dueDateTicks = (($scope.dueDate.getTime() * 10000) + 621355968000000000);

                $scope.editAssignment($scope.assignment, dueDateTicks);
            }
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