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
    public class ProfessorController : Controller
    {

        private MultiversoContext db = new MultiversoContext();


        public JsonResult getProfessores()
        {

            List<Professor> prof = db.Professores.ToList();
            return Json(prof, JsonRequestBehavior.AllowGet);

        }
        
        public string InserirProfessor(Professor prof)
        {
            string ret = "Inserido com sucesso!!";

            try
            {
                if (prof != null)
                {

                    prof.ativo = 1;
                    db.Professores.Add(prof);
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
        public string UpdateProfessor(Professor prof)
        {
            string ret = "Editado com sucesso!!";
            try
            {
                if (prof != null)
                {

                    var Emp_ = db.Entry(prof);
                    Professor EmpObj = db.Professores.Where(x => x.id == prof.id).FirstOrDefault();
                    EmpObj.nome = prof.nome;
                    EmpObj.apelido = prof.apelido;
                    EmpObj.email = prof.email;
                    EmpObj.salario = prof.salario;
                    EmpObj.dataNascimento = prof.dataNascimento;
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
        public string DeleteProfessor(Professor prof)
        {
            string ret = "Removido com sucesso!!";
            try
            {
                if (prof != null)
                {

                    var Emp_ = db.Entry(prof).State = EntityState.Deleted;
                    db.Professores.Attach(prof);
                    db.Professores.Remove(prof);
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
                ret = "Error a Apagar : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }

        public string getNFicha()
        {
            string ret = "1";
            try
            {
                Aluno al = db.Alunos.OrderByDescending(u => u.id).FirstOrDefault();

                if (al == null)
                {
                    al = new Aluno();
                    al.numeroFicha = 1;
                }
                else
                {
                    al.numeroFicha++;
                }

                ret = (al.numeroFicha).ToString();
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a getNFicha : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }
       
        // GET: Professor
        public ActionResult Index()
        {


            return View( );
        }

    }
}
