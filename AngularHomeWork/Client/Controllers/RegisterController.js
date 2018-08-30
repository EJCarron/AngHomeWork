
(function(app){

    var RegisterController = function($scope){

        $scope.registerBtnClicked = function() {

            $scope.registerNewUser($scope.email, $scope.name, $scope.userType, $scope.password);


        }
    }

    app.controller("registerController", RegisterController);

}(angular.module("loginApp")));