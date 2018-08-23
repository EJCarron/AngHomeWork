
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
        when("/TeacherClassRoom/:name", {
            templateUrl: 'Client/Views/TeacherClassRoomView.html',
            controller: 'teacherClassRoomController'
        }).
        when("/TeacherAssignmennt/:assignmentId", {
            templateUrl: 'Client/Views/TeacherAssignmentView.html',
            controller: 'teacherAssignmentController'
        }).
        when("/CreateAssignment/:classRoomName", {
            templateUrl: 'Client/Views/CreateAssignmentView.html',
            controller: 'createAssignmentController'
        }).
        when("/EditAssignment", {
            templateUrl: 'Client/Views/EditAssignmentView.html',
            controller: 'editAssignmentController'
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
