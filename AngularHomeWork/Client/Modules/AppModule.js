
var app = angular.module('app', ['ngMaterial', 'ngRoute']);

app.config(['$routeProvider',

        function($routeProvider){
        $routeProvider.
        when('/TeacherHome',{
            templateUrl : 'Client/Views/TeacherHomeView.html',
            controller: 'teacherHomeController'
        }).
        when("/CreateClassRoom", {
            templateUrl: 'Client/Views/CreateClassRoomView.html',
            controller: 'createClassRoomController'
        }).
        when("/TeacherClassRoom", {
            templateUrl: 'Client/Views/TeacherClassRoomView.html',
            controller: 'teacherClassRoomController'
        }).
        otherwise({
            redirectTo:'/TeacherHome'
        });
    }]);

app.config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('cyan')
            .accentPalette('blue-grey');
    });
