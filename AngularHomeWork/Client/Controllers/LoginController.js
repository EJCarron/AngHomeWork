
(function(app){

    var LoginController = function($scope){

        $scope.loginBtnClicked = function() {

            if(($scope.email == null) || ($scope.password == null)){
                
                $scope.showAlertDialog("Email or Password cannot be left blank");
            }else{

                $scope.loginAttempt($scope.email, $scope.password);
            }
        } 
    }

    app.controller("loginController", LoginController);

}(angular.module("loginApp")));