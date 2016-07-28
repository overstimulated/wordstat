(function () {
    'use strict';
    
    angular.module('wordstatApp.controllers', []).
        controller('wordstatController', wordstatController);

    wordstatController.$inject = ['$scope', 'wordstatService'];

    function wordstatController($scope, wordstatService) {
        $scope.models = {
            paragraph: 'Warwick Willis was thinking about Heather Kowalski again. Heather was a stupid knight with dirty abs and moist toes. Warwick walked over to the window and reflected on his urban surroundings. He had always loved pretty Chicago with its grotesque, gifted gates. It was a place that encouraged his tendency to feel barmy. Then he saw something in the distance, or rather someone. It was the a stupid figure of Heather Kowalski. Warwick gulped. He glanced at his own reflection. He was a cowardly, tight-fisted, beer drinker with moist abs and curvaceous toes. His friends saw him as a bumpy, brave brute. Once, he had even rescued a naughty puppy from a burning building. But not even a cowardly person who had once rescued a naughty puppy from a burning building, was prepared for what Heather had in store today. The drizzle rained like cooking rats, making Warwick angry. Warwick grabbed a squidgy hawk that had been strewn nearby; he massaged it with his fingers. As Warwick stepped outside and Heather came closer, he could see the yummy glint in her eye.',
            title: 'Word Statistics'
        }

        $scope.clearParagraph = clearParagraph;
        $scope.clearStat = clearStat;
        $scope.getWordStats = getWordStats;

        function clearParagraph() {
            $scope.models.paragraph = '';
            $scope.stats = {};
        }

        function clearStat() {
            $scope.stats = {};
        }

        function getWordStats(paragraph) {
            wordstatService.get(paragraph)
                .success(function (wordstats) {
                    $scope.stats = wordstats;
                })
                .error(function (error) {
                    $scope.status = 'Unable to process the data!'
                    console.log(error.message);
                });
        }
    }

})();