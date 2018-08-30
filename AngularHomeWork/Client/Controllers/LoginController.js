
(function(app){

    var LoginController = function($scope){

        $scope.loginBtnClicked = function() {


            $scope.loginAttempt($scope.email, $scope.password);
        } 
    }

    app.controller("loginController", LoginController);

}(angular.module("loginApp")));