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
            $scope.Aluno = {};
            $scope.Aluno.nome = document.getElementById("nome").value;// $scope.nome;
            $scope.Aluno.apelido = document.getElementById("apelido").value;// $scope.apelido;
            $scope.Aluno.email = document.getElementById("email").value;//$scope.email;
            $scope.Aluno.dataNascimento = document.getElementById("dataNascimento").value;// $scope.dataNascimento;
            $scope.Aluno.numeroFicha = document.getElementById("numeroFicha").value;//$scope.numeroFicha;

            var data = { aluno: $scope.Aluno, curso: $scope.selectCurso };
            $http({
                method: "post",
                url: "/Aluno/InserirAluno",
                datatype: "json",
                data: JSON.stringify(data)
            }).then(function (response) {
                alert(response.data);
                $('#Modal').modal('hide');
                $scope.getAlunos();
                limpaCampos();
            })
        } else {
            $scope.Aluno = {};
            $scope.Aluno.nome = document.getElementById("nome").value;// $scope.nome;
            $scope.Aluno.apelido = document.getElementById("apelido").value;// $scope.apelido;
            $scope.Aluno.email = document.getElementById("email").value;//$scope.email;
            $scope.Aluno.dataNascimento = document.getElementById("dataNascimento").value;// $scope.dataNascimento;
            $scope.Aluno.numeroFicha = document.getElementById("numeroFicha").value;//$scope.numeroFicha;
            
            $scope.Aluno.id = document.getElementById("id").value;

            var dataI = { aluno: $scope.Aluno, curso: $scope.selectCurso };
            $http({
                method: "post",
                url: "/Aluno/UpdateAluno",
                datatype: "json",
                data: JSON.stringify(dataI)
            }).then(function (response) {
                alert(response.data);
               
              
             
                document.getElementById("btnSave").setAttribute("value", "Gravar");
                document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                //document.getElementById("spn").innerHTML = "Adicionar novo Aluno";
                $scope.getAlunos();
                $('#Modal').modal('hide');
                limpaCampos();
                 
            })
        }
    }

     
    $scope.setSelectCurso = function (obj) {

        console.log(obj);
        $scope.selectCurso = obj;
        $scope.selectProf = $scope.selectCurso.professor;

         

    };
    $scope.editItem = function (obj) {

        console.log(obj);

    };
    $scope.getAlunos = function () {
         
       //--------------------Buscar Alunos----------------------
        $http({
            method: "get",
            url: "/Aluno/getAlunos"
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
            //console.log(response.data   );
            $scope.alunos = response.data;


        }, function () {
            alert("Erro");
        });

       
    };

    function limpaCampos( ) {
        $scope.nome = "";
        $scope.apelido = "";
        $scope.email = "";
        $scope.dataNascimento = "";
        $scope.numeroFicha = "";
        document.getElementById("nome").value = "";
        document.getElementById("apelido").value = "";
        document.getElementById("email").value = "";
        document.getElementById("dataNascimento").value = "";
        document.getElementById("numeroFicha").value = "";
    }
    function ConvertJsonDateToDate(date) {
        var parsedDate = new Date(parseInt(date.substr(6)));
        var newDate = new Date(parsedDate);
        var month = ('0' + (newDate.getMonth() + 1)).slice(-2);
        var day = ('0' + newDate.getDate()).slice(-2);
        var year = newDate.getFullYear();
        //alert(day + "/" + month + "/" + year);
        return new Date( year + "-" + month + "-" + day);
    }



    $scope.DeleteAluno = function (aluno) {
        if (confirm('Deseja apagar o Aluno(a) ' + aluno.nome +' ?' )) {
            $http({
                method: "post",
                url: "/Aluno/DeleteAluno",
                datatype: "json",
                data: JSON.stringify(aluno)
            }).then(function (response) {
                alert(response.data);
                $scope.getAlunos();
            })
        } else {
            alert('Cancelado');
        }

       
    };
    $scope.UpdateAluno = function (aluno) {
        $('#Modal').find('.modal-title').text('Editar Aluno');
        document.getElementById("id").value = aluno.id;
        $scope.nome = aluno.nome;
        $scope.apelido = aluno.apelido;
        $scope.email = aluno.email;
        $scope.dataNascimento = new Date(aluno.dataNascimento) ;
        $scope.numeroFicha = aluno.numeroFicha;
        
        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";
       

        //---------------------------------Buscar Cursos
         
        $http({
            method: "post",
            url: "/Aluno/getCursos",
            datatype: "json" ,
            data: JSON.stringify(aluno)

        }).then(function (response) {



            $scope.cursos = response.data;


            angular.forEach($scope.cursos, function (value, key) {

                if ($scope.cursos[key].ativo > 0) {

                    $scope.selectCurso = $scope.cursos[key];

                }
            });

        }, function () {
            alert("Erro");
        });
        $('#Modal').modal('show'); 
        //var b = $scope.dataNascimento.split(/\D/);

        //var dt = new Date(b[0], --b[1], b[2]);
        //alert(dt);

        //document.getElementById('dataNascimento').valueAsDate = dt;
    }
   
    $scope.NovoAluno = function () {
        
        $('#Modal').find('.modal-title').text('Novo Aluno');
        document.getElementById("btnSave").setAttribute("value", "Gravar");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
        limpaCampos();
        //Buscar o ultimo numero da ficha disponivél
        $http({
            method: "post",
            url: "/Aluno/getNFicha",
            datatype: "json"
        }).then(function (response) {
            $scope.numeroFicha = response.data;
            $scope.nome = "";
            $scope.apelido = "";
            $scope.email = "";
            $scope.dataNascimento = "";

           
        });

        //Buscar o ultimo numero da ficha disponivél



        //---------------------------------Buscar Cursos
        $scope.Aluno = {};
        $scope.Aluno.nome = document.getElementById("nome").value;// $scope.nome;
        $scope.Aluno.apelido = document.getElementById("apelido").value;// $scope.apelido;
        $scope.Aluno.email = document.getElementById("email").value;//$scope.email;
        $scope.Aluno.dataNascimento = document.getElementById("dataNascimento").value;// $scope.dataNascimento;
        $scope.Aluno.numeroFicha = document.getElementById("numeroFicha").value;//$scope.numeroFicha;
        $http({
            method: "post",
            url: "/Aluno/getCursos",
            datatype: "json",
            data: JSON.stringify($scope.Aluno)

        }).then(function (response) {



            $scope.cursos = response.data;


            angular.forEach($scope.cursos, function (value, key) {

                if ($scope.cursos[key].ativo > 0) {

                    $scope.selectCurso = $scope.cursos[key];

                }
            });

        }, function () {
            alert("Erro");
        });
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
