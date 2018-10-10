
(function(app){

    var LoginPageController = function($scope, modelCommand){

        $scope.registerNewUser = function(newEmail, newName, userType, newPassword) {

            modelCommand.registerNewUser(newEmail, newName, userType, newPassword);
        }

        $scope.loginAttempt = function(email, password){

            modelCommand.attemptLogin(email, password);
        }

        $scope.showAlertDialog = function(message) {

            modelCommand.showAlertDialog(message);
        }
    }

    app.controller("loginPageController", LoginPageController);

}(angular.module("loginApp")));