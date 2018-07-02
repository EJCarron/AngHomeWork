"use strict";

var mcs={};   // Model Command Service -  handles model changes cmds - via the http mid tier

(function(){

    this.modelCommandServiceFactory = function ($scope,$http){

        return {
      
            createClassroom : function (modelPartsReq,name){

                var requestData = {
                    modelPartsReq : modelPartsReq,
                    name : name,
                                  
                }

                $http({
                    method: 'POST',
                    url:'ClassRoom/Create',
                    data: requestData
                    
                }).then(
                    function successCallback(response){
                        mum.modelPartsUpdate($scope,response.data.modelUpdates);
                    },
                    function errorCallback(response){
                    
                    }
                 );

           }
        }
    }

}).call(mcs);




var mum = {}; // Model Update Mechansim - handles generic model updates

(function(){

    this.ModelPartsReq = function(){
    
    }

    this.modelPartsUpdate = function(modelUpdates,$scope){

        for (var i = 0;i<modeUpdates.length;i++){
            var modelUpdate = modelUpdates[i];


            switch (modelUpdate.type){
                case 'selectedClassroom' :{
                    $scope.selectedClassroom = modelUpdate.data;
                }break;

                case 'classRoomNames' :{
                    $scope.classRoomNames = modelUpdate.data;
                }break;

            }
        }


    }

}).call(mum);