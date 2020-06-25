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
    public class CursoController : Controller
    {
        private MultiversoContext db = new MultiversoContext();


        public JsonResult getCursos()
        {

            List<Curso> curso = db.Cursos.Include(m => m.disciplinaCurso).Include(m => m.alunosCurso).ToList();

             
            return Json(curso, JsonRequestBehavior.AllowGet);

        }

        public string InserirCurso(Curso curso, string[] ids)//, string idprof)
        {
            string ret = "Inserido com sucesso!!";

            try
            {
                if (curso != null)
                {
                    curso.ativo = 1;
                    
                   
                    curso.disciplinaCurso = new List<DisciplinaCurso>();
                    // Adicionar professor
                    //int idprofI = int.Parse(idprof);
                    //Professor prof = db.Professores.Where(x => x.id == idprofI).SingleOrDefault();
                    //if (prof != null) {
                    //    curso.professor = prof;
                    //}
                   
                    //Adicionar a tabela Curso
                    db.Cursos.Add(curso);
                    db.SaveChanges();

                    //Inserir diciplinas
                    DisciplinaCurso di = new DisciplinaCurso();
                    foreach (string item in ids)
                    {

                        if (item.Trim().Length > 0)
                        {

                            di = new DisciplinaCurso();
                            di.idCurso = curso.id;
                            di.idDisciplina = int.Parse(item);
                            curso.disciplinaCurso.Add(di);
                             
                        }
                    }
                     
                    
                    //Adicionar na tabela  DisciplinasCurso
                    db.DisciplinasCursos.AddRange(curso.disciplinaCurso);
                    db.SaveChanges();
                    
                }
                else
                {
                    return "Error a Inserir: Objeto nulo";
                }
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a Inserir : " + exe.Message + " Linha Nº" + linenum;

            }
            return ret;
        }
        
        public JsonResult GetDisciplinaCurso(Curso curso)
        {
            List<Disciplina> disciplinas = db.Disciplinas.ToList();

            
            disciplinas.Select(c => { c.ativo = -1; return c; }).ToList();
            if (curso.disciplinaCurso != null) { 
                foreach (var di in curso.disciplinaCurso) {

                    disciplinas.Where(x =>  x.id==di.idDisciplina).ToList().ForEach(c => { c.ativo = 1; });

                }
            }
               
            return Json(disciplinas, JsonRequestBehavior.AllowGet);
        }
        public string UpdateCurso(Curso curso,string[] ids)//,string idprof)
        {
            string ret = "Editado com sucesso!!";
            try
            {
                if (curso != null)
                {

                    var Emp_ = db.Entry(curso);
                    Curso curs = db.Cursos.Where(x => x.id == curso.id).Include(s=>s.disciplinaCurso).FirstOrDefault();
                    //// Adicionar professor
                    //int idprofI = int.Parse(idprof);

                    //Professor prof = db.Professores.Where(x => x.id == idprofI).SingleOrDefault();
                    //if (prof != null)
                    //{
                    //    curs.professor = prof;
                    //}
                     
                    //preencher as disciplinas selecionadas
                    DisciplinaCurso di = new DisciplinaCurso();
                    

                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso 
                    if (curs.disciplinaCurso != null) {
                        db.DisciplinasCursos.RemoveRange(curs.disciplinaCurso);
                        db.SaveChanges();
                    }
                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso

                    //limpar listas de disciplinas para adicionar a partir do escolhido
                    curs.disciplinaCurso = new List<DisciplinaCurso>();

                    foreach (string item in ids)
                    {

                        if (item.Trim().Length > 0) {

                            
                            di = new DisciplinaCurso();
                            di.idCurso = curs.id;
                            di.idDisciplina = int.Parse( item);
 

                            curs.disciplinaCurso.Add(di);
                        }
                    }
                    //Adicionar na tabela  DisciplinasCursos
                    db.DisciplinasCursos.AddRange(curs.disciplinaCurso);
                    db.SaveChanges();

                    //Guardar alterações curso
                    curs.descricao = curso.descricao;
                    curs.nome = curso.nome;
                    db.SaveChanges();


                }
                else
                {
                    ret = "Erro na edição";
                }
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a  Editar : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }
        public string DeleteCurso(Curso curso)
        {
            string ret = "Removido com sucesso!!";
            try
            {
                if (curso != null)
                {
                    
                    
                     
                     

                    //apagar tabela Curso
                    var Emp_ = db.Entry(curso).State = EntityState.Deleted;

                    db.Cursos.Attach(curso);
                    db.Cursos.Remove(curso);
                    db.SaveChanges();
                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso 
                    if (curso.disciplinaCurso != null)
                    {
                        //Curso curs = db.Cursos.Where(x => x.id == curso.id).Include(s => s.disciplinaCurso).FirstOrDefault();
                        var di = db.DisciplinasCursos.Where(x => x.idCurso == curso.id);
                        db.DisciplinasCursos.RemoveRange(di);
                        db.SaveChanges();
                    }
                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso 

                }
                else
                {
                    ret = "Erro ao Apagar";
                }
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a Apagar : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }

        // GET: Curso
        public ActionResult Index()
        {
            return View();
        }
 
    }
}
