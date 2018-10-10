
(function(app) {


    var ModelCommand = function($http, $window, $location, $mdDialog) {


        this.registerNewUser = function(newEmail, newName, userType, newPassword, schoolCode) {

            var commandObject = makeRegisterUserCO(newEmail, newName, userType, newPassword, schoolCode);


             $http({
                method : "POST",
                url : ("http://127.0.0.1:8080/api/User/register"),
                data : commandObject
                }).then(function success(response) {

                    $location.path("#!/Login");

                    var $event = "";

                    showDialog($event, "Account registered, now Login",  1);
                   
                }, function error(response){

                    var $event = "";

                    showDialog($event, response.data.Message, 2);

                });

        }



        this.attemptLogin = function(email, passwordAttempt){


            $http({
                method : "GET",
                url : ("http://127.0.0.1:8080/api/User/attemptLogin/"+email+"/"+passwordAttempt)
                }).then(function success(response) {

                    var loginResponse = response.data;

                    if(loginResponse.success){

                        loginSuccess(loginResponse.userType);
                    }else{

                        var $event = "";

                        showDialog( $event,"incorrect username or password", 2);

                    }
                });


        }

        this.showAlertDialog = function(message) {

            var $event = "";

            showDialog( $event, message, 2);

        } 

        var loginSuccess = function(userType){

            

            var controller = "";

            switch (userType) {

                case 1: {
                    controller = "Teacher";
                }break;
                case 2: {
                    controller = "Student";
                }break;
            }

            window.location.href = 'http://127.0.0.1:8080/Home/'+ controller;

        }

        var showDialog = function(ev, message, type) {

            var titleText = "";

            if(type == 1){

                titleText = "Success";
            }else{

                titleText = "Error";
            }

            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#studentPage')))
                    .clickOutsideToClose(true)
                    .title(titleText)
                    .textContent(message)
                    .ariaLabel('Alert Dialog')
                    .ok('Got it!')
                    .targetEvent(ev)
            );

        }


        var makeRegisterUserCO = function(newEmail, newName, userType, newPassword, schoolCode) {

            var cO = {
                requestObject: null,
                emailAddress: newEmail,
                name: newName,
                type: userType,
                password: newPassword,
                schoolCode: schoolCode

                }

            return cO;
        }




    }

    app.service('modelCommand',['$http','$window','$location','$mdDialog', ModelCommand]);


}(angular.module("loginApp")));