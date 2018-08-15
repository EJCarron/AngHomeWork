(function(app){

    var TeacherPageController = function($scope, $mdSidenav, modelCommand, nullObjects, $location){

        $scope.selectedClassRoom = nullObjects.classRoom;

        $scope.classRoomBtnClicked = function(classRoomName) {

            $scope.setSelectedClassRoom(classRoomName);

            modelCommand.getClassRoom(classRoomName, $scope);

        }

       $scope.setSelectedClassRoom = function(classRoomName){

            $scope.selectedClassRoomName = classRoomName;

            $scope.selectedClassRoom = nullObjects.classRoom;
            
            
       }

        $scope.createClassRoom = function(newName){

            $location.path("/TeacherClassRoom/"+newName)

            $scope.setSelectedClassRoom(newName);
            
            modelCommand.createClassRoom(newName, $scope.teacherId);


        }

    }

    app.controller("teacherPageController", TeacherPageController);

}(angular.module("app")));
