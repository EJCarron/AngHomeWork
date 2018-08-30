
(function(app) {


    var ModelCommand = function($http, $window) {


        this.registerNewUser = function(newEmail, newName, userType, newPassword) {

            var commandObject = makeRegisterUserCO(newEmail, newName, userType, newPassword);


             $http({
                method : "POST",
                url : ("http://127.0.0.1:8080/api/User/register"),
                data : commandObject
                }).then(function success(response) {

                    var newUserId = response.data;

                   loginSuccess(newUserId, userType);
                   
                });

        }


        this.attemptLogin = function(email, passwordAttempt){


            $http({
                method : "GET",
                url : ("http://127.0.0.1:8080/api/User/attemptLogin/"+email+"/"+passwordAttempt)
                }).then(function success(response) {

                    var loginResponse = response.data;

                    if(loginResponse.success){

                        loginSuccess(loginResponse.userId, loginResponse.userType);
                    }else{

                        loginFailure("incorrect username or password");

                    }
                });


        }


        var loginSuccess = function(userId, userType){

            

            var controller = "";

            switch (userType) {

                case 1: {
                    controller = "Teacher";
                }break;
                case 2: {
                    controller = "Student";
                }break;
            }

            window.location.href = 'http://127.0.0.1:8080/Home/'+ controller + "/" + userId;

        }

        var loginFailure = function(message){

            //make it come up with error display.

        }


        var makeRegisterUserCO = function(newEmail, newName, userType, newPassword) {

            var cO = {
                requestObject: null,
                emailAddress: newEmail,
                name: newName,
                type: userType,
                password: newPassword,
                schoolId: 1

                }

            return cO;
        }




    }

    app.service('modelCommand',['$http','$window', ModelCommand]);


}(angular.module("loginApp")));