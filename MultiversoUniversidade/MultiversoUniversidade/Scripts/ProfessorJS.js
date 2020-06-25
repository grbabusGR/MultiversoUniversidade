var app = angular.module("myApp", []);



app.controller("myCtrl", function ($scope, $http) {
    //debugger;

    $scope.showModal = false;
    $scope.toggleModal = function () {
        $scope.showModal = !$scope.showModal;
    };
    $scope.InsertData = function () {

        var Action = document.getElementById("btnSave").getAttribute("value");
        if (Action == "Gravar") {
            $scope.Professor = {};
            $scope.Professor.nome = document.getElementById("nome").value;// $scope.nome;
            $scope.Professor.apelido = document.getElementById("apelido").value;// $scope.apelido;
            $scope.Professor.email = document.getElementById("email").value;//$scope.email;
            $scope.Professor.dataNascimento = document.getElementById("dataNascimento").value;// $scope.dataNascimento;
            $scope.Professor.salario = document.getElementById("salario").value;//$scope.salario;
            $http({
                method: "post",
                url: "/Professor/InserirProfessor",
                datatype: "json",
                data: JSON.stringify($scope.Professor)
            }).then(function (response) {
                alert(response.data);
                $('#Modal').modal('hide');
                $scope.getProfessores();
                //$scope.nome = "";
                //$scope.apelido = "";
                //$scope.email = "";
                //$scope.dataNascimento = "";
                //$scope.salario = "";
                limpaCampos();
            })
        } else {
            $scope.Professor = {};
            $scope.Professor.nome = document.getElementById("nome").value;//$scope.nome;
            $scope.Professor.apelido = document.getElementById("apelido").value;// $scope.apelido;
            $scope.Professor.email = document.getElementById("email").value;//$scope.email;

            $scope.Professor.dataNascimento = document.getElementById("dataNascimento").value;//$scope.dataNascimento;
            $scope.Professor.salario = document.getElementById("salario").value;// $scope.salario;

            $scope.Professor.id = document.getElementById("id").value;
            $http({
                method: "post",
                url: "/Professor/UpdateProfessor",
                datatype: "json",
                data: JSON.stringify($scope.Professor)
            }).then(function (response) {
                alert(response.data);
                document.getElementById("btnSave").setAttribute("value", "Gravar");
                document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                
                $scope.getProfessores();
                $('#Modal').modal('hide');
              
                limpaCampos();
            })
        }
    }
    $scope.getProfessores = function () {


        $http({
            method: "get",
            url: "/Professor/getProfessores"
        }).then(function (response) {


            for (var i = 0; i < response.data.length; i++) {
                // Loop through each property in the Array.
                for (var property in response.data[i]) {
                    if (response.data[i].hasOwnProperty(property)) {
                        var resx = /Date\(([^)]+)\)/;

                        // Checking Date with regular expresion.
                        if (resx.test(response.data[i][property])) {
                            // Setting Date in dd/MM/yyyy format.
                            response.data[i][property] = ConvertJsonDateToDate(response.data[i][property]);
                        }
                    }
                }
            }
           // console.log(response.data);
            $scope.professores = response.data;
        }, function () {
            alert("Erro");
        })
    };
    function limpaCampos( ) {
        $scope.nome = "";
        $scope.apelido = "";
        $scope.email = "";
        $scope.dataNascimento = "";
        $scope.salario = "";
        document.getElementById("nome").value = "";
        document.getElementById("apelido").value = "";
        document.getElementById("email").value = "";
        document.getElementById("dataNascimento").value = "";
        document.getElementById("salario").value = "";
    }
    function ConvertJsonDateToDate(date) {
        var parsedDate = new Date(parseInt(date.substr(6)));
        var newDate = new Date(parsedDate);
        var month = ('0' + (newDate.getMonth() + 1)).slice(-2);
        var day = ('0' + newDate.getDate()).slice(-2);
        var year = newDate.getFullYear();
         
        return day + "-" + month + "-" + year;
    }
    $scope.DeleteProfessor = function (prof) {
        if (confirm('Deseja apagar o Registo ' + prof.nome + ' ?')) {
            $http({
                method: "post",
                url: "/Professor/DeleteProfessor",
                datatype: "json",
                data: JSON.stringify(prof)
            }).then(function (response) {
                alert(response.data);
                $scope.getProfessores();
            })
        } else {
            alert('Cancelado');
        }


    };
    $scope.UpdateProfessor = function (professor) {

        
        $('#Modal').find('.modal-title').text('Editar Professor');
        document.getElementById("id").value = professor.id;
        $scope.nome = professor.nome;
        $scope.apelido = professor.apelido;
        $scope.email = professor.email;
        $scope.dataNascimento = professor.dataNascimento;
        $scope.salario = professor.salario;

        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
         
        $('#Modal').modal('show');
    }

    $scope.NovoProfessor = function () {

        document.getElementById("btnSave").setAttribute("value", "Gravar");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";

        limpaCampos();
        $('#Modal').find('.modal-title').text('Novo Professor');
        $('#Modal').modal('show');

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


