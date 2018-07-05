

(function(app) {


    var ModelCommand = function() {
        this.createClassroom = function(newCR, cRs) {
            
            cRs.push(newCR);
            return cRs;
        }
    }

    app.service('testModelCommand', ModelCommand);

}(angular.module("app")));

