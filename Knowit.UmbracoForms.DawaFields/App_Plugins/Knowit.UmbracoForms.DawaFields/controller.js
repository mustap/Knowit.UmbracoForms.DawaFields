angular.module("umbraco").controller("DawaRenderController", ['$scope', function ($scope) {

    $scope.jsonField = [];

    // Example data to populate the field variable
    $scope.myInitialize = function (text) {
        $scope.jsonField = JSON.parse(text);
    }

}]);
