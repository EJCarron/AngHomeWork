(function(app){

    var TeacherPageController = function($scope, $mdSidenav, modelCommand, nullObjects, magicStrings, $location){

        $scope.selectedClassRoom = nullObjects.classRoom;

        $scope.logoutBtnClicked = function() {

            modelCommand.logout();
        }


        $scope.classRoomBtnClicked = function(classRoomName) {

            $scope.setSelectedClassRoom(classRoomName);

            modelCommand.getClassRoom(classRoomName, $scope);

        }

       $scope.setSelectedClassRoom = function(classRoomName){

            $scope.selectedClassRoomName = classRoomName;

            $scope.selectedClassRoom = nullObjects.classRoom;
            
            
       }

        $scope.createClassRoom = function(newName){


            
            modelCommand.createClassRoom(newName, $scope, function(){
            
                $location.path("/TeacherClassRoom/"+newName)

                $scope.setSelectedClassRoom(newName);
            
            }
                );

        }


        $scope.archiveClassRoom = function(classRoomName) {
            $location.path("/TeacherHome");

            modelCommand.changeClassRoomArchiveStatus( classRoomName, magicStrings.archiveCode, $scope);
        }

        $scope.setSelectedAssignment = function(assignmentId){

            $scope.selectedAssignmentId = assignmentId;

            $scope.selectedAssignment = nullObjects.assignment;
        }

        $scope.createAssignment = function(newName, classRoomName, newDueDateTicks, newDescription){

            $location.path("/TeacherClassRoom/"+classRoomName);

            modelCommand.createAssignment(newName, classRoomName, newDueDateTicks, newDescription, $scope);
        }


        $scope.editAssignment = function(assignment, newDueDateTicks) {

            $scope.setSelectedAssignment(assignment.id);

            modelCommand.editAssignment(assignment,newDueDateTicks, $scope);
        }

        $scope.archiveAssignment = function(assignment){

            $location.path("/TeacherClassRoom/"+assignment.classRoomName);

            modelCommand.changeAssignmentArchiveStatus(assignment, magicStrings.archiveCode, $scope);
        }

        $scope.showAlertDialog = function(message) {

            modelCommand.showAlertDialog(message);
        }

        $scope.getTodaysAssignments = function() {

            modelCommand.getTodaysAssignments($scope);
        }
    }

    app.controller("teacherPageController", TeacherPageController);

}(angular.module("app")));
