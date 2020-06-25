using MultiversoUniversidade.Models;
using MultiversoUniversidade.Models.DAL;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiversoUniversidade.Controllers
{
    public class AlunoController : Controller
    {
        private MultiversoContext db = new MultiversoContext();


        public JsonResult getAlunos()
        {
            
                List<Aluno> aluno = db.Alunos.ToList();
                return Json(aluno, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult getCursos(Aluno aluno)
        {

            List<Curso> curso = db.Cursos.Include(m => m.disciplinaCurso).Include(m => m.alunosCurso).ToList();

              curso.Select(c => { c.ativo = -1; return c; }).ToList();

            if (aluno != null) {
                AlunosCurso aluC = db.AlunosCurso.Where(x => x.idAluno == aluno.id).SingleOrDefault();
                if (aluC != null)
                {
                    curso.Where(x => x.id == aluC.idCurso ).ToList().ForEach(c => { c.ativo = 1; });
                }
                       
            }

            return Json(curso, JsonRequestBehavior.AllowGet);

        }
        public string InserirAluno(Aluno aluno,Curso curso)
        {
            if (curso ==null)
            {
                return "Deve seleciona o curso";
            }
            if (curso.id <=0 ){
                return "Deve seleciona o curso";
            }
            string ret = "Inserido com sucesso!!";

            try
            {
                if (aluno != null)
            {
 
                    aluno.ativo = 1;
                    db.Alunos.Add(aluno);
                    db.SaveChanges();
                    
                    //Guardar o curso escolhido
                    AlunosCurso alC = new AlunosCurso();
                    alC.idAluno = aluno.id;
                    alC.idCurso = curso.id;
                    
                    
                    
                    db.AlunosCurso.Add(alC);
                    db.SaveChanges();


                    curso = db.Cursos.Where(x => x.id == curso.id).SingleOrDefault();
                    
                    if (curso.alunosCurso == null)
                    {
                        curso.alunosCurso = new List<AlunosCurso>();
                    }
                    curso.alunosCurso.Add(alC);

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
        public string UpdateAluno(Aluno aluno,Curso curso)
        {
            string ret = "Editado com sucesso!!";
            try {  
            if (aluno != null)
            {

                var Emp_ = db.Entry(aluno);
                Aluno EmpObj = db.Alunos.Where(x => x.id == aluno.id).FirstOrDefault();
                EmpObj.nome = aluno.nome;
                EmpObj.apelido = aluno.apelido;
                EmpObj.email = aluno.email;
                EmpObj.dataNascimento =  aluno.dataNascimento ;
                db.SaveChanges();

                //Apaga se existir 
                AlunosCurso alC = new AlunosCurso();

                    alC = db.AlunosCurso.Where(x=>x.idAluno ==aluno.id).SingleOrDefault();


                if(alC!= null){ 
                            var alc_ = db.Entry(alC).State = EntityState.Deleted;
                            db.AlunosCurso.Attach(alC);
                            db.AlunosCurso.Remove(alC);
                            db.SaveChanges();
                    }
                    //Guardar o curso escolhido
                    alC = new AlunosCurso();

                    alC.idAluno = aluno.id; 
                alC.idCurso = curso.id;
                db.AlunosCurso.Add(alC);
                db.SaveChanges();

                    curso = db.Cursos.Where(x => x.id == curso.id).SingleOrDefault();

                    if (curso.alunosCurso == null)
                    {
                        curso.alunosCurso = new List<AlunosCurso>();
                    }
                    curso.alunosCurso.Add(alC);

                    db.SaveChanges();

                }
            else
            {
                ret =  "Erro na edição";
            }
        } catch (Exception exe)
            {
                    int linenum = exe.LineNumber();
                    ret = "Error a  Editar : " + exe.Message + " Linha Nº" + linenum;
                        
              }

            return ret;
}
        public string DeleteAluno(Aluno aluno)
        {
            string ret = "Removido com sucesso!!";
            try
            {
                if (aluno != null)
                {
                    //Apaga se existir 
                    AlunosCurso alC = new AlunosCurso();

                    alC = db.AlunosCurso.Where(x => x.idAluno == aluno.id).SingleOrDefault();


                    if (alC != null)
                    {
                        var alc_ = db.Entry(alC).State = EntityState.Deleted;
                        db.AlunosCurso.Attach(alC);
                        db.AlunosCurso.Remove(alC);
                        db.SaveChanges();
                    }
                    var Emp_ = db.Entry(aluno).State = EntityState.Deleted;
                    db.Alunos.Attach(aluno);
                    db.Alunos.Remove(aluno  );
                    db.SaveChanges();

             


                }
                else
                {
                    ret = "Erro ao Apagar";
                }
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error ao Apagar : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }

        public string getNFicha( )
        {
            string ret = "1";
            try
            {
                Aluno al = db.Alunos.OrderByDescending(u => u.id).FirstOrDefault() ;

                if (al == null)
                {
                    al = new Aluno();
                    al.numeroFicha = 1;
                }
                else
                {
                    al.numeroFicha++;
                }

                ret =(al.numeroFicha ).ToString();
            }
            catch (Exception exe)
            {
                int linenum = exe.LineNumber();
                ret = "Error a getNFicha : " + exe.Message + " Linha Nº" + linenum;

            }

            return ret;
        }
        // GET: Aluno
        public ActionResult Index( )
        {

 
             return View( );
        }

     
    }
}
