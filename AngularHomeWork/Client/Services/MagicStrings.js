﻿
(function(app) {


    var magicStrings = function() {

        this.PUT = "PUT";
        this.POST = "POST";
        this.DELETE = "DELETE";

        this.apiURL = "/api/";

        this.URL = "/";

        this.classRoomController = "ClassRoom";
        this.assignmentController = "Assignment";
        this.subscriptionController = "Subscription";
        this.studAssignmentController = "StudentAssignment";
        this.outStandingController = "OutStanding";
        this.todaysAssignmentsController = "TodaysAssignments";

        this.archiveAction = "archive";
        this.createAction = "create";
        this.getAction = "get";
        this.editAction = "edit";
        this.changeDoneAction = "changeDoneState";
        this.subscribeAction = "subscribe";
        this.unsubscribeAction = "unsubscribe";

        this.archiveCode = 1;
        this.unarchiveCode = 0;

        this.unsubSucc = "Unsubscribed from ";
        this.subSucc = "Subscribed to ";
        this.crtClrSucc = "ClassRoom Created";
        this.crtAssSucc = "Assignment Created";
        }

    app.service('magicStrings',[magicStrings]);


}(angular.module("app")));