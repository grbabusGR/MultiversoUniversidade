using MultiversoUniversidade.Models;
using MultiversoUniversidade.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace MultiversoUniversidade.Controllers
{
    public class DisciplinaController : Controller
    {
        // GET: Disciplina
        public ActionResult Index()
        {
            return View();
        }
        private MultiversoContext db = new MultiversoContext();


        public JsonResult getDisciplina()
        {

            List<Disciplina> disciplina = db.Disciplinas.Include(s=>s.disciplinaProfessor).ToList();
            return Json(disciplina, JsonRequestBehavior.AllowGet);

        }

        public string InserirDisciplina(Disciplina disciplina,string [] ids)
        {
            string ret = "Inserido com sucesso!!";
            try
            {
                if (disciplina != null)
                {
                    disciplina.ativo = 1;

                   
                    disciplina.disciplinaProfessor = new List<DisciplinaProfessor>();

                    //Adicionar a tabela 
                    db.Disciplinas.Add(disciplina);
                    db.SaveChanges();

                    //Inserir Professores
                    DisciplinaProfessor di = new DisciplinaProfessor();
                    foreach (string item in ids)
                    {

                        if (item.Trim().Length > 0)
                        {

                            di = new DisciplinaProfessor();
                            di.idDisciplina = disciplina.id;
                            di.idProfessor = int.Parse(item);
                            disciplina.disciplinaProfessor.Add(di);

                        }
                    }


                    //Adicionar na tabela  DisciplinasCurso
                    db.DisciplinasProfessores.AddRange(disciplina.disciplinaProfessor);
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

        public JsonResult GetDisciplinaProfessor(Disciplina dis)
        {
            List<Professor> prof = db.Professores.ToList();


            prof.Select(c => { c.ativo = -1; return c; }).ToList();
            prof.Select(c => { c.nome = c.nome + " " + c.apelido; return c; }).ToList();
            if (dis.disciplinaProfessor != null)
            {
                foreach (var di in dis.disciplinaProfessor)
                {

                    prof.Where(x => x.id == di.idProfessor).ToList().ForEach(c => { c.ativo = 1; });
                  
                }
            }

            return Json(prof, JsonRequestBehavior.AllowGet);
        }
        public string UpdateDisciplina(Disciplina disciplina,string[] ids)
        {
            string ret = "Editado com sucesso!!";
            try
            {
                if (disciplina != null)
                {

                    var Emp_ = db.Entry(disciplina);
                    Disciplina di = db.Disciplinas.Where(x => x.id == disciplina.id).Include(s => s.disciplinaProfessor).FirstOrDefault();

                    //preencher as disciplinas selecionadas
                    DisciplinaProfessor profDi = new DisciplinaProfessor();


                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso 
                    if (di.disciplinaProfessor != null)
                    {
                        db.DisciplinasProfessores.RemoveRange(di.disciplinaProfessor);
                        db.SaveChanges();
                    }
                    //Remove da tabela DisciplinaCurso todos registos associados a esse curso

                    //limpar listas de disciplinas para adicionar a partir do escolhido
                    di.disciplinaProfessor = new List<DisciplinaProfessor>();

                    foreach (string item in ids)
                    {

                        if (item.Trim().Length > 0)
                        {


                            profDi = new DisciplinaProfessor();
                            profDi.idDisciplina = di.id;
                            profDi.idProfessor = int.Parse(item);

                            di.disciplinaProfessor.Add(profDi);
                        }
                    }
                    //Adicionar na tabela  DisciplinaProfessor
                    db.DisciplinasProfessores.AddRange(di.disciplinaProfessor);
                    db.SaveChanges();

                    //Guardar alterações 
                    di.nome = disciplina.nome;
                    di.descricao = disciplina.descricao;
                    di.ciclo = disciplina.ciclo;
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
        public string DeleteDisciplina(Disciplina disciplina)
        {
            string ret = "Removido com sucesso!!";
            try
            {
                if (disciplina != null)
                {





                    //apagar tabela Curso
                    var Emp_ = db.Entry(disciplina).State = EntityState.Deleted;

                    db.Disciplinas.Attach(disciplina);
                    db.Disciplinas.Remove(disciplina);
                    db.SaveChanges();
                    ////Remove da tabela DisciplinaCurso todos registos associados a esse curso 
                    //if (disciplina.disciplinaProfessor != null)
                    //{
                    //    //Curso curs = db.Cursos.Where(x => x.id == curso.id).Include(s => s.disciplinaCurso).FirstOrDefault();
                    //    var di = db.DisciplinasProfessores.Where(x => x.idDisciplina == disciplina.id);
                    //    db.DisciplinasProfessores.RemoveRange(di);
                    //    db.SaveChanges();
                    //}
                    ////Remove da tabela DisciplinaCurso todos registos associados a esse curso 

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

    }
}
