var app = angular.module("myApp", ['ngPrint']);



app.controller("myCtrl", function ($scope, $http) {
    //debugger;
    $scope.selectProf = "";

    $scope.selectCurso = "";
    $scope.dataReg = new Date();
    $scope.showModal = false;

     

    
    $scope.getLCursos = function () {


        $http({
            method: "get",
            url: "/Listagem/getLCursos"
        }).then(function (response) {



            $scope.lcurso = response.data;

            console.log($scope.lcurso);
        }, function () {
            alert("Erro");
        })
    };
    function limpaCampo() {
        $scope.nome = "";
        $scope.descricao = "";
        document.getElementById("nome").value = "";
        document.getElementById("descricao").value = "";
    }
    

    function ConvertJsonDateToDate(date) {
        var parsedDate = new Date(parseInt(date.substr(6)));
        var newDate = new Date(parsedDate);
        var month = ('0' + (newDate.getMonth() + 1)).slice(-2);
        var day = ('0' + newDate.getDate()).slice(-2);
        var year = newDate.getFullYear();
        //alert(day + "/" + month + "/" + year);
        return day + "-" + month + "-" + year;
    }





})

app.directive('modal', function () {
    return {
        template: '<div class="modal fade">' +
            '<div class="modal-dialog">' +
            '<div class="modal-content">' +
            '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
            '<h4 class="modal-title">{{ title }}</h4>' +
            '</div>' +
            '<div class="modal-body" ng-transclude></div>' +
            '</div>' +
            '</div>' +
            '</div>',
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        link: function postLink(scope, element, attrs) {
            scope.title = attrs.title;

            scope.$watch(attrs.visible, function (value) {
                if (value == true)
                    $(element).modal('show');
                else
                    $(element).modal('hide');
            });

            $(element).on('shown.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = true;
                });
            });

            $(element).on('hidden.bs.modal', function () {
                scope.$apply(function () {
                    scope.$parent[attrs.visible] = false;
                });
            });
        }
    };
});

app.directive('loading', ['$http', function ($http) {
    return {
        restrict: 'A',
        link: function (scope, elm, attrs) {
            scope.isLoading = function () {
                return $http.pendingRequests.length > 0;
            };

            scope.$watch(scope.isLoading, function (v) {
                if (v) {
                    elm.css('display', 'block');
                } else {
                    elm.css('display', 'none');
                }
            });
        }
    };

}]);