


(function(app) {


    var nullObjects = function() {

            this.classRoom = {
            assignments:{},
            name:""
            }

        }
    app.service('nullObjects',[nullObjects]);


}(angular.module("app")));




