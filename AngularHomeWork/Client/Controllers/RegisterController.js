
(function(app){

    var RegisterController = function($scope){

        $scope.registerBtnClicked = function() {

            if(
                ($scope.email == null)
                ||
                ($scope.name == null)
                ||
                ($scope.userType == 0)
                ||
                ($scope.password == null)
            ) {
                $scope.showAlertDialog("All fields must be complete.");
            }else{

                if(!$scope.validateEmail($scope.email)){
                    $scope.showAlertDialog("Invalid email address.");
                }else{
                    $scope.registerNewUser($scope.email, $scope.name, $scope.userType, $scope.password);
                }
            }

        }

        $scope.validateEmail = function(mail) {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(mail)){
                return true;
            }

            return false;
        }
    }

    app.controller("registerController", RegisterController);

}(angular.module("loginApp")));