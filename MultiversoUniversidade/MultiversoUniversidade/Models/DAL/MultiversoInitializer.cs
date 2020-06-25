using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

 
using System.Data.Entity;
using MultiversoUniversidade.Models;

namespace MultiversoUniversidade.Models.DAL
{
    public class MultiversoInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MultiversoContext>
    {
        protected override void Seed(MultiversoContext context)
        {


            //----------------Professores-------------------
            var professores = new List<Professor>
            {
                 new Professor{   id= 1,ativo=1,dataNascimento =  DateTime.Parse("1952-01-01"),nome ="Pardal" ,apelido ="garnizé", email="Pardal@Multiverso.pt",salario = "4.664,97" },
                 new Professor{   id= 2,ativo=1,dataNascimento =  DateTime.Parse("1978-10-05"),nome ="Denzel" ,apelido ="Crocker", email="Crocker@Multiverso.pt",salario = "4.010,23" },
                 new Professor{   id= 3,ativo=1,dataNascimento =  DateTime.Parse("1997-10-05"),nome ="Herbert" ,apelido ="Garrison", email="Garrison@Multiverso.pt",salario = "5.401,54" }

            };

            professores.ForEach(s => context.Professores.Add(s));
            context.SaveChanges();
            //----------------Professores-------------------

            //----------------DisciplinaCurso-------------------


            var disciplinaCurso = new List<DisciplinaCurso>
            {
                new DisciplinaCurso { id = 1,idCurso=1,idDisciplina=1  },
                new DisciplinaCurso { id = 2 ,idCurso=1,idDisciplina=2  } ,
                 new DisciplinaCurso { id =3 ,idCurso=1,idDisciplina=3  }
            };

            disciplinaCurso.ForEach(s => context.DisciplinasCursos.Add(s));
            context.SaveChanges();
            //----------------DisciplinaCurso-------------------

            //----------------DisciplinaProfessor-------------------


            var disciplinaProfessor = new List<DisciplinaProfessor>
            {
                new DisciplinaProfessor { id = 1,  idProfessor=1,idDisciplina=1  },
                new DisciplinaProfessor { id = 2 ,idProfessor=2,idDisciplina=2  },
                new DisciplinaProfessor { id = 2 ,idProfessor=3,idDisciplina=3  }
            };

            disciplinaProfessor.ForEach(s => context.DisciplinasProfessores.Add(s));
            context.SaveChanges();
            //----------------DisciplinaProfessor-------------------

            //----------------Disciplinas Curso Astronomia-------------------
            var disciplinas = new List<Disciplina>
            {
                 new Disciplina{ id=1, ativo = 1, nome= "Prática de Investigação em Astronomia e Astrofísica" , ciclo= 1, descricao ="", disciplinaCurso=context.DisciplinasCursos.Where(x=>x.id==1).ToList(),disciplinaProfessor = disciplinaProfessor.Where(x=>x.id==1).ToList() },
                 new Disciplina{ id=2,  ativo = 1, nome= "Tópicos Avançados em Sistemas Planetários" , ciclo= 1, descricao ="",          disciplinaCurso=context.DisciplinasCursos.Where(x=>x.id==2).ToList() ,disciplinaProfessor = disciplinaProfessor.Where(x=>x.id==2).ToList() },
                 new Disciplina{ id=3,  ativo = 1, nome= "Investigação em Astronomia e Astrofísica" , ciclo= 1, descricao =""   ,      disciplinaCurso=context.DisciplinasCursos.Where(x=>x.id==3).ToList() ,disciplinaProfessor = disciplinaProfessor.Where(x=>x.id==3).ToList() },

            };

            disciplinas.ForEach(s => context.Disciplinas.Add(s));
            context.SaveChanges();
            //----------------Disciplinas-------------------



            //----------------Alunos-------------------
            var alunos = new List<Aluno>
            {
                 new Aluno{  id= 1, numeroFicha= 9686810,nome="Juca",apelido="Castro",dataNascimento=DateTime.Parse("1982-09-01"),       },
                 new Aluno{  id= 2, numeroFicha= 9686811,nome="Maria",apelido="Joaquina",dataNascimento=DateTime.Parse("1979-12-15"),    },
                 new Aluno{  id= 3, numeroFicha= 9686812,nome="Manoel",apelido="Sardinha",dataNascimento=DateTime.Parse("1999-10-08"),   }

            };
            alunos.ForEach(s => context.Alunos.Add(s));
            context.SaveChanges();
            //----------------Alunos-------------------



            //----------------AlunosCurso-------------------
            var alunosCurso = new List<AlunosCurso>
            {
                 new AlunosCurso{  id=1, idCurso=1, idAluno=1   },
                 new AlunosCurso{  id= 2, idCurso=1, idAluno=2 ,    },
                 new AlunosCurso{  id= 3, idCurso=1, idAluno=3 ,   }

            };
            alunosCurso.ForEach(s => context.AlunosCurso.Add(s)) ;
            context.SaveChanges();
            //----------------AlunosCurso-------------------
            //----------------Cursos-------------------

            var cursos = new List<Curso>
            {
                new Curso { id = 1, ativo =1, nome="Curso Astronomia", descricao="" ,  disciplinaCurso=context.DisciplinasCursos.Where(x=> new List<int>{ 1,2,3}.Contains( x.id)).ToList() , alunosCurso =  alunosCurso.Where(x=>x.idCurso ==1).ToList() }

            };

            cursos.ForEach(s => context.Cursos.Add(s));
            context.SaveChanges();
            //----------------Cursos-------------------


            

            //----------------Notas-------------------
            var notas = new List<Nota>
            {
                 new Nota{ id= 1, idAluno =1,  idDisciplina=1, nota=15 , data = DateTime.Now,idProfessor = 1},
                 new Nota{ id= 1, idAluno =1,  idDisciplina=2, nota=17 , data = DateTime.Now,idProfessor = 1},
                 new Nota{ id= 1, idAluno =1,  idDisciplina=3, nota=10 , data = DateTime.Now,idProfessor = 1},

                 new Nota{ id= 1, idAluno =2,  idDisciplina=1, nota=18 , data = DateTime.Now,idProfessor = 2},
                 new Nota{ id= 1, idAluno =2,  idDisciplina=2, nota=20 , data = DateTime.Now,idProfessor = 2},
                 new Nota{ id= 1, idAluno =2 , idDisciplina=3, nota=7 , data = DateTime.Now,idProfessor = 2},

                 new Nota{ id= 1, idAluno =3,  idDisciplina=1, nota=19 , data = DateTime.Now,idProfessor = 3},
                 new Nota{ id= 1, idAluno =3,  idDisciplina=2, nota=18 , data = DateTime.Now,idProfessor = 3},
                 new Nota{ id= 1, idAluno =3 , idDisciplina=3, nota=12 , data = DateTime.Now,idProfessor = 3}
            };
            notas.ForEach(s => context.Notas.Add(s));
            context.SaveChanges();
            //----------------Notas-------------------

        }
    }
}