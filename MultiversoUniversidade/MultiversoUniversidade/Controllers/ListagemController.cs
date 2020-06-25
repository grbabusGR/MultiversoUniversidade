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
    public class ListagemController : Controller
    {
        private MultiversoContext db = new MultiversoContext();

        // GET: Listagem
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult lCurso()
        {
            return View();
        }
        public ActionResult lDisciplina()
        {
            return View();
        }
        public ActionResult LAluno()
        {
            return View();
        }
        public JsonResult getLCursos()
        {

            List<Curso> lcurso = db.Cursos.Include(m => m.disciplinaCurso).Include(m => m.alunosCurso).ToList();



            List<LCursos> lCursos = new List<LCursos>();


            foreach (Curso cu in lcurso)
            {
                LCursos lc = new LCursos();

                lc.nomeCurso = cu.nome;
                //diciplinas por curso
                foreach (DisciplinaCurso diC in cu.disciplinaCurso)
                {
                    //Diciplina por professor
                    List<DisciplinaProfessor> diCP = db.DisciplinasProfessores.Where(x => x.idDisciplina == diC.idDisciplina).ToList();
                    foreach (DisciplinaProfessor profe in diCP)
                    {
                        LCursosSub lcS = new LCursosSub();
                        //nome professor
                        Professor pro = db.Professores.Where(x => x.id == profe.idProfessor).SingleOrDefault();
                        if (pro != null)
                        {
                            lcS.nomeProfessor = pro.nome + " " + pro.apelido;
                            lcS.idProfessor = pro.id;
                        }



                        //Alunos do curso
                        foreach (AlunosCurso al in cu.alunosCurso)
                        {
                            Aluno a = db.Alunos.Where(x => x.id == al.idAluno).SingleOrDefault();
                            DisciplinaAlunos dicA = new DisciplinaAlunos();

                            dicA.idAluno = al.idAluno;
                            dicA.idDisciplina = diC.idDisciplina;
                            //Nome do aluno
                            if (a != null)
                            {
                                dicA.nomeAluno = a.nome + " " + a.apelido;
                            }

                            //Notas por disciplina
                            Nota nt = db.Notas.Where(r => r.idAluno == al.id  && r.idProfessor == profe.idProfessor).ToList().Take(1).SingleOrDefault();
                            if (nt != null)
                            {
                                double mediaNota = db.Notas.Where(r => r.idAluno == al.id   && r.idProfessor == profe.idProfessor).Average(r => r.nota);

                                dicA.nota = String.Format("{0}", mediaNota);
                                lcS.lDisciplinaAlunos.Add(dicA);
                            }
                            
                            
                        }
                        lc.lCursosSub.Add(lcS);
                    }
                    
                }
                

                List<LCursosSub> distinctItems = lc.lCursosSub.GroupBy(x => x.idProfessor).Select(y => y.First()).ToList();
                lc.lCursosSub = distinctItems;
                lCursos.Add(lc);


            }


            
            return Json(lCursos.Distinct(), JsonRequestBehavior.AllowGet);

        }
        public JsonResult getlDisciplinas()
        {
            List<Disciplina> ldisciplina = db.Disciplinas.Include(x=>x.disciplinaProfessor).ToList();

            // classe de retorno
            List<LDisciplina> lDisciplina = new List<LDisciplina>();
            //Disciplinas
            foreach (Disciplina di in ldisciplina) {

                LDisciplina dic = new LDisciplina();
                dic.nomeDisciplina = di.nome;

                if (di.disciplinaProfessor == null) {
                    Json(lDisciplina, JsonRequestBehavior.AllowGet);
                }
                //Buscar os professores  por disciplina
                foreach (DisciplinaProfessor diP in di.disciplinaProfessor)
                {

                    LDisciplinaSub lDisciplinaSub = new LDisciplinaSub();

                    Professor pro = db.Professores.Where(x => x.id == diP.idProfessor).SingleOrDefault();
                    lDisciplinaSub.nomeProfessor = pro.nome + " " + pro.apelido;

                    //Buscar os alunos/notas por disciplina/Professor

                    LDisciplinaSub1 lDisciplinaSub1 = new LDisciplinaSub1();

                    
                    DisciplinaAlunos alu = new DisciplinaAlunos();

                    List<Nota> liNotas = db.Notas.Where(x => x.idDisciplina == diP.idDisciplina && x.idProfessor == diP.idProfessor).OrderBy(o => o.idAluno).ToList();

                    foreach (Nota no in liNotas)
                    {
                        alu = new DisciplinaAlunos();
                         
                        Aluno al = db.Alunos.Where(x => x.id == no.idAluno).SingleOrDefault();
                        if (al != null)
                        {
                            alu.nomeAluno = al.nome + " " + al.apelido;
                            double not = no.nota;
                            alu.nota = String.Format("{0}", not);
                            alu.data = no.data.ToString("dd-MM-yyyy");

                            lDisciplinaSub1.lAlunosNota.Add(alu);
                        }
                    }


                    //var notas = db.Notas.Where(x=>x.idDisciplina == diP.idDisciplina && x.idProfessor == diP.idProfessor).OrderBy(o => o.idAluno).GroupBy(s=>s.idAluno).ToList();

                    

                    //foreach (var group in notas)
                    //{
                    //    alu = new DisciplinaAlunos();
                    //    Nota n = group.Take(1).SingleOrDefault();
                         
                    //    Aluno al = db.Alunos.Where(x => x.id == n.idAluno).SingleOrDefault();
                    //    if (al!=null) {
                    //            alu.nomeAluno = al.nome + " " + al.apelido;
                    //            double no = group.Sum(x => x.nota);
                    //            alu.nota = String.Format("{0}", no);
                        

                    //            lDisciplinaSub1.lAlunosNota.Add(alu);
                    //    }
                    //}





                    List<DisciplinaAlunos> distinctItems = lDisciplinaSub1.lAlunosNota.GroupBy(x => x.idAluno).Select(y => y.First()).ToList();
                    //lDisciplinaSub1.lAlunosNota = distinctItems;

                    lDisciplinaSub.lDisciplinaSub1.Add(lDisciplinaSub1);




                    dic.LDisciplinaSub.Add(lDisciplinaSub);
                    
                }

                lDisciplina.Add(dic);
                }


           
            return Json(lDisciplina, JsonRequestBehavior.AllowGet);

        }
        public JsonResult getlAluno()
        {
            List<lAluno> rlAuno = new List<lAluno>();//Retorno
            lAluno laluno = new lAluno();
            List<AlunosCurso> alC = db.AlunosCurso.ToList();
            foreach (AlunosCurso alunosCurso in alC) {
                laluno = new lAluno();
                Aluno al = db.Alunos.Where(x=>x.id== alunosCurso.idAluno).SingleOrDefault();
                if (al != null) {
                    laluno.nomeAluno = al.nome + " " + al.apelido;
                }

                //por cada aluno vai buscar suas notas por diciplina
                List<Nota> notas = db.Notas.Where(x => x.idAluno == al.id).OrderBy(o => o.idDisciplina).OrderBy(o => o.data).OrderBy(o => o.idProfessor).ToList();
                foreach (Nota nota in notas) {
                    DisciplinaAlunos notaDi = new DisciplinaAlunos();

                    notaDi.data = nota.data.ToString("dd-MM-yyyy");
                    Disciplina di =  db.Disciplinas.Where(x => x.id == nota.idDisciplina).SingleOrDefault();
                    notaDi.descricaoDisciplina = di.nome;
                    Professor prof = db.Professores.Where(x => x.id == nota.idProfessor).SingleOrDefault();
                    notaDi.nomeProfessor = prof.nome + " " + prof.apelido;
                    notaDi.nota = String.Format("{0}",nota.nota);
                    laluno.lAlunosNota.Add(notaDi);
                }
                rlAuno.Add(laluno);
            }

            

             
            return Json(rlAuno, JsonRequestBehavior.AllowGet);

        }
        //Para listagem media
        class LCursos{
            
            public string nomeCurso { get; set; }
           
            public ICollection<LCursosSub> lCursosSub = new List<LCursosSub>();

        }
        class LCursosSub
        {

            public string nomeProfessor { get; set; }
            public int idProfessor { get; set; }

            public ICollection<DisciplinaAlunos> lDisciplinaAlunos = new List<DisciplinaAlunos>();

        }
        //Para listagem media


        // Para Lista de disciplinas  
        class LDisciplina
        {

            public string nomeDisciplina { get; set; }
             
            public ICollection<LDisciplinaSub> LDisciplinaSub = new List<LDisciplinaSub>();

        }
        class LDisciplinaSub
        {

            public string nomeProfessor { get; set; }


            public ICollection<LDisciplinaSub1> lDisciplinaSub1 = new List<LDisciplinaSub1>();

        }
        class LDisciplinaSub1
        { 

            public ICollection<DisciplinaAlunos> lAlunosNota = new List<DisciplinaAlunos>();

        }
        // Para Lista de disciplinas  
        //para a listagem de Alunos
        class lAluno {
            public string nomeAluno { get; set; }
            public ICollection<DisciplinaAlunos> lAlunosNota = new List<DisciplinaAlunos>();

        }

    }
}
