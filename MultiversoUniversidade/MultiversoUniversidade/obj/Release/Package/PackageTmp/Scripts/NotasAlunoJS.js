var app = angular.module("myApp", []);



app.controller("myCtrl", function ($scope, $http) {
    //debugger;
    $scope.selectProf = "";
    
    $scope.selectCurso = "";

    $scope.aluno = "";
    $scope.dataReg =   new Date();  
    $scope.showModal = false;
    
    $scope.InsertData = function () {
        

        var Action = document.getElementById("btnSave").getAttribute("value");
        if (Action == "Gravar") {
            //$scope.Curso = {};
            //$scope.Curso.nome = document.getElementById("nome").value;// $scope.nome;
            //$scope.Curso.descricao = document.getElementById("descricao").value;// $scope.descricao;
            $scope.dataReg = document.getElementById("data").value;
            var dataI = { lDisciplinaAlunos: $scope.DisciplinasAluno, data: $scope.dataReg, professor: $scope.selectProf  };

            $http({
                method: "post",
                url: "/NotasAluno/InserirNotas",
                datatype: "json",
                data: JSON.stringify(dataI)
            }).then(function (response) {
                alert(response.data);
                $('#Modal').modal('hide');
                //$scope.getCursos();
                //limpaCampo();
            })
        }  
    }
    $scope.setSelectCurso = function (obj) {

         //console.log(obj);
        $scope.selectCurso = obj;
       // $scope.selectProf = $scope.selectCurso.professor;

        //Curso Alunos do curso Escolhido 
       

        $http({
            method: "post",
            url: "/NotasAluno/GetAlunosCurso",
            datatype: "json",
            data: JSON.stringify($scope.selectCurso)
        }).then(function (response) {
            //alert(response.data);
            $scope.lAlunos  = response.data;


             


        })

    };
    $scope.editItem = function (obj ) {
        
        console.log(obj);
        
    };
    
    $scope.setSelectProf = function (obj) {

       console.log(obj);
        $scope.selectProf = obj;


        var dataPesq = { prof: $scope.selectProf, aluno: $scope.aluno, curso: $scope.selectCurso  };

            $http({
                method: "post",
                url: "/NotasAluno/GetDisciplinaProfessor",
                datatype: "json",
                data: JSON.stringify(dataPesq)
            }).then(function (response) {
                //alert(response.data);
                $scope.DisciplinasAluno = response.data;





            })
 
    };
    $scope.getCursos = function () {


        $http({
            method: "get",
            url: "/NotasAluno/getCursos"
        }).then(function (response) {



            $scope.cursos = response.data;
        }, function () {
            alert("Erro");

            });


       
    };
    function limpaCampo() {
        $scope.nome = "";
        $scope.descricao = "";
        document.getElementById("nome").value = "";
        document.getElementById("descricao").value = "";
    }
    $scope.EditarNotas = function (aluno) {
        
        $('#Modal').find('.modal-title').text('Editar Nota');
        document.getElementById("id").value = aluno.id;
        $scope.nome = aluno.nome + ' ' + aluno.apelido;
        $scope.aluno = aluno;
        $http({
            method: "post",
            url: "/NotasAluno/getProfessores",
            datatype: "json",
           data: JSON.stringify($scope.selectCurso)
            
        }).then(function (response) {



            $scope.professores = response.data;
        }, function () {
            alert("Erro");
        });


       
        //    
        //});
        $('#Modal').modal('show');

    };
   
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



