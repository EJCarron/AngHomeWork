
var app = angular.module('app', ['ngMaterial']);


app.config(function($mdThemingProvider) {
        $mdThemingProvider.theme('default')
            .primaryPalette('pink')
            .accentPalette('orange');
    });
