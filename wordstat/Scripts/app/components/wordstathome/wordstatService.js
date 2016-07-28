(function () {
    'use strict';

    angular.module('wordstatApp.services', []).
        factory('wordstatService', wordstatService);

    wordstatService.$inject = ['$http'];

    function wordstatService($http) {
        var service = {
            get: get
        };

        function get(paragraph) {
            return $http({
                url: 'api/WordStatistics',
                type: 'GET',
                params: { paragraph: paragraph }
            });
        }
        return service;
    }
})();