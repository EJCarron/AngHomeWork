

(function(app) {


    var ModelCommand = function($http) {
        this.createClassroom = function(newCR, cRs) {

            $http({
                method : "POST",
                data : {
                    hello : "hello1",
                    world : "world1"
                },
                url : "http://127.0.0.1:8080/ClassRoom/Create"
            }).then(function success(response) {
                
            }, function error(response) {
                
            });

        }
    }

    app.service('modelCommand',['$http', ModelCommand]);

}(angular.module("app")));




