using System;
using MultiversoUniversidade.Models;
using MultiversoUniversidade.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace MultiversoUniversidade.Controllers
{
    public class NotasAlunoController : Controller
    {
        // GET: NotasAluno
        public ActionResult Index()
        {
            return View();
        }

        private MultiversoContext db = new MultiversoContext();


        public JsonResult getCursos()
        {

            List<Curso> curso = db.Cursos.Include(m => m.disciplinaCurso).Include(m => m.alunosCurso).ToList();


            return Json(curso, JsonRequestBehavior.AllowGet);

        }

        public JsonResult getProfessores(Curso curso)
        {

            List<Professor> prof = new List<Professor>();





            foreach (DisciplinaCurso di in curso.disciplinaCurso) {
                List<DisciplinaProfessor> disciplinaProfessor = db.DisciplinasProfessores.Where(x=>x.idDisciplina ==di.idDisciplina).ToList();

                foreach (DisciplinaProfessor dip in disciplinaProfessor) {

                    prof.Add(db.Professores.Where(x => x.id == dip.idProfessor).SingleOrDefault());
                }
                

            }
            //prof.Where(x => disciplinaProfessor.Contains(x.id) ).ToList().ForEach(c => { c.ativo = 1; });
            return Json(prof, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAlunosCurso(Curso curso)
        {

            List<Aluno> alunos = new List<Aluno>();
            if (curso.alunosCurso != null) { 
                    foreach (AlunosCurso a in curso.alunosCurso) {
                        Aluno al = new Aluno();
                        al = db.Alunos.Where(x=>x.id==a.idAluno).SingleOrDefault();
                        if (al != null) {
                            alunos.Add(al);
                        }

                    }
            }

            return Json(alunos, JsonRequestBehavior.AllowGet);

        }
        

        public string InserirNotas(List<DisciplinaAlunos> lDisciplinaAlunos, DateTime data, Professor professor)
        {
            string ret = "Inserido com sucesso!!";

            try
            {
                if (lDisciplinaAlunos != null)
                {
                    
                    foreach (DisciplinaAlunos di in lDisciplinaAlunos)
                    {
                        // adicionar a tabela nota
                        Nota nota = new Nota();
                        nota.idAluno = di.idAluno;
                        nota.idDisciplina = di.idDisciplina;
                        nota.idProfessor = professor.id;
                        nota.nota = int.Parse(di.nota);
                        nota.data = data;

                        db.Notas.Add(nota);
                        db.SaveChanges();

                    }
                }
                else {
                    ret = "Error a Inserir ";

                }
                 
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a Inserir : " + exe.Message + " Linha Nº" + linenum;

            }
            return ret;
        }

        public JsonResult GetDisciplinaAluno(Curso curso,Aluno aluno)
        {

            List<Disciplina> disciplinas = new List<Disciplina>();
            List<DisciplinaAlunos> disciplinaAlunos = new List<DisciplinaAlunos>();
            foreach (DisciplinaCurso disciplina in curso.disciplinaCurso )
            {
                DisciplinaAlunos di = new DisciplinaAlunos();

                di.descricaoDisciplina = db.Disciplinas.Where(x => x.id == disciplina.idDisciplina).SingleOrDefault().nome;
                di.nota = "";
                di.idAluno = aluno.id;
                di.idDisciplina = disciplina.idDisciplina;
                disciplinaAlunos.Add(di);

            }



        


            return Json(disciplinaAlunos, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetDisciplinaProfessor(Professor prof, Aluno aluno,Curso curso)
        {

            List<DisciplinaProfessor> disciplinaProf = new List<DisciplinaProfessor>();// db.DisciplinasProfessores.Where(x => x.idProfessor == prof.id).ToList();

            

            foreach (DisciplinaCurso diC in curso.disciplinaCurso)
            {

                DisciplinaProfessor popo = db.DisciplinasProfessores.Where(x => x.idDisciplina == diC.idDisciplina && x.idProfessor == prof.id).SingleOrDefault();

                if (popo != null) 
                {
                    disciplinaProf.Add(popo);
                }
                


            }

            disciplinaProf.Select(c => { c.idAluno = aluno.id; return c; }).ToList();
            disciplinaProf.Select(c => { c.nota = "0"; return c; }).ToList();
            foreach (DisciplinaProfessor disciplina in disciplinaProf)
            {

               
                disciplina.Descricao = db.Disciplinas.Where(x => x.id == disciplina.idDisciplina).SingleOrDefault().nome;
              

            }






            return Json(disciplinaProf, JsonRequestBehavior.AllowGet);

        }


    }
}
