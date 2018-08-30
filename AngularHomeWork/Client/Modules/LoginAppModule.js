var app = angular.module('loginApp', ['ngMaterial', 'ngRoute']);

app.config(['$routeProvider',

        function($routeProvider){
        $routeProvider.
        when('/Login',{
            templateUrl : '/Client/Views/LoginView.html',
            controller: 'loginController'
        }).
        when("/Register", {
            templateUrl: '/Client/Views/RegisterView.html',
            controller: 'registerController'
        }).
            when("/Landing", {
            templateUrl: '/Client/Views/LandingView.html',
            controller: 'landingController'
        }).
        otherwise({
            redirectTo:'/Landing'
        });
    }]);

app.config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('cyan')
            .accentPalette('blue-grey');
    });

