
var app = angular.module('app', ['ngMaterial', 'ngRoute']);

app.config(['$routeProvider',

        function($routeProvider){
        $routeProvider.
        when('/StudentHome',{
            templateUrl : '/Client/Views/StudentHomeView.html',
            controller: 'studentHomeController'
        }).
        when("/SubscribeToClassRoom", {
            templateUrl: '/Client/Views/SubscribeToClassRoomView.html',
            controller: 'subscribeToClassRoomController'
        }).
        when("/StudentClassRoom/:name", {
            templateUrl: '/Client/Views/StudentClassRoomView.html',
            controller: 'studentClassRoomController'
        }).
        when("/StudentAssignmennt/:assignmentId", {
            templateUrl: '/Client/Views/StudentAssignmentView.html',
            controller: 'studentAssignmentController'
        }).
        otherwise({
            redirectTo:'/StudentHome'
        });
    }]);



app.config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('cyan')
            .accentPalette('blue-grey');
    });
