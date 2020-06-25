var app = angular.module("myApp", []);



app.controller("myCtrl", function ($scope, $http) {
    //debugger;
   // $scope.selectProf = "";
    $scope.ids = [];
    $scope.showModal = false;
    $scope.check = false;
    $scope.toggleModal = function () {
    $scope.showModal = !$scope.showModal;
    };
    $scope.InsertData = function () {
        getIDS();// verifica is check marcados
       
        var Action = document.getElementById("btnSave").getAttribute("value");
        if (Action == "Gravar") {
            $scope.Curso = {};
            $scope.Curso.nome = document.getElementById("nome").value;// $scope.nome;
            $scope.Curso.descricao = document.getElementById("descricao").value;// $scope.descricao;
           // var dataI = { curso: $scope.Curso, ids: $scope.ids, idprof: $scope.selectProf.id };
            var dataI = { curso: $scope.Curso, ids: $scope.ids  };
            $http({
                method: "post",
                url: "/Curso/InserirCurso",
                datatype: "json",
                data: JSON.stringify(dataI)
            }).then(function (response) {
                alert(response.data);
                $('#Modal').modal('hide');
                $scope.getCursos();
                limpaCampo();
            })
        } else {
            $scope.Curso = {};
            $scope.Curso.nome = document.getElementById("nome").value;// $scope.nome;
            $scope.Curso.descricao = document.getElementById("descricao").value;// $scope.descricao;
            

            $scope.Curso.id = document.getElementById("id").value;
           // var data = { curso: $scope.Curso, ids: $scope.ids, idprof: $scope.selectProf.id };
            var data = { curso: $scope.Curso, ids: $scope.ids };
            $http({
                method: "post",
                url: "/Curso/UpdateCurso",
                datatype: "json",
                data: 
                     JSON.stringify(data)
                
                
            }).then(function (response) {
                alert(response.data);



                document.getElementById("btnSave").setAttribute("value", "Gravar");
                document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";
                //document.getElementById("spn").innerHTML = "Adicionar novo Aluno";
                $scope.getCursos();
                $('#Modal').modal('hide');
                limpaCampo();
                

            })
        }
    }

    //$scope.setSelectProf = function (obj) {
        
    //    console.log(obj);
    //    $scope.selectProf = obj;
    //};
    $scope.getCursos = function () {


        $http({
            method: "get",
            url: "/Curso/getCursos"
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
            $scope.cursos = response.data;
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

    function getIDS() {
        var items = document.getElementsByName('checkDisci');
        var selectedItems = "";
        var arr = [];
        for (var i = 0; i < items.length; i++) {
            if (items[i].type == 'checkbox' && items[i].checked == true)
                selectedItems += items[i].id + "|";

            
        }

       // $scope.ids.push(items[i].id);
        $scope.ids = selectedItems.split("|");
         
        
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
    $scope.DeleteCurso = function (curso) {
        if (confirm('Deseja apagar o Registo ' + curso.nome + ' ?')) {
            $http({
                method: "post",
                url: "/Curso/DeleteCurso",
                datatype: "json",
                data: JSON.stringify(curso)
            }).then(function (response) {
                alert(response.data);
                $scope.getCursos();
            })
        } else {
            alert('Cancelado');
        }


    };
    
    $scope.UpdateCurso = function (curso) {
        $('#Modal').find('.modal-title').text('Editar Curso');
        document.getElementById("id").value = curso.id;
        $scope.nome = curso.nome;
        $scope.descricao = curso.descricao;
        $scope.id = curso.id;
        $http({
            method: "post",
            url: "/Curso/GetDisciplinaCurso",
            datatype: "json",
            data: JSON.stringify(curso)
        }).then(function (response) {
            //alert(response.data);
            $scope.Disciplinas = response.data;



            angular.forEach($scope.Disciplinas, function (value, key) {

                if ($scope.Disciplinas[key].ativo > 0) {

                    $scope.ids.push($scope.Disciplinas[key].id);//(key + ': ' + value);

                }
            }, $scope.ids);


        });

   

        document.getElementById("btnSave").setAttribute("value", "Update");
        document.getElementById("btnSave").style.backgroundColor = "Yellow";

        $('#Modal').modal('show');
    };
     
     

    $scope.NovoCurso = function () {
        limpaCampo();
        $('#Modal').find('.modal-title').text('Novo Curso');

        document.getElementById("btnSave").setAttribute("value", "Gravar");
        document.getElementById("btnSave").style.backgroundColor = "cornflowerblue";

        $('#Modal').modal('show');
        $scope.ids = [];
        $scope.Curso = {};
        $scope.Curso.id = -1;
        
         
        $http({
            method: "post",
            url: "/Curso/GetDisciplinaCurso",
            datatype: "json",
            data: JSON.stringify($scope.Disciplina)
        }).then(function (response) {
            //alert(response.data);
            $scope.Disciplinas = response.data;
 


        })
        ////Professores
        //$http({
        //    method: "post",
        //    url: "/Curso/GetCursoProfessor",
        //    datatype: "json",
        //    data: JSON.stringify($scope.Disciplina)
        //}).then(function (response) {
        //    //alert(response.data);
        //    $scope.Professores = response.data;





        //});


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



