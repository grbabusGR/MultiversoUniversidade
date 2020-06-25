using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MultiversoUniversidade.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MultiversoUniversidade.Models.DAL
{
    public class MultiversoContext : DbContext
    {
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<DisciplinaProfessor> DisciplinasProfessores { get; set; }
        public DbSet<DisciplinaCurso> DisciplinasCursos { get; set; }
        public DbSet<AlunosCurso> AlunosCurso { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }


}