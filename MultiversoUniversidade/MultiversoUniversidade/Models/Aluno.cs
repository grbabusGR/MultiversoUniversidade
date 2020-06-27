using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class Aluno
    {
        public int id { get; set; }
        public int ativo { get; set; }
        public int numeroFicha { get; set; }
        public string nome { get; set; }
        public string apelido { get; set; }
        public string email { get; set; }
        public DateTime  dataNascimento { get; set; }
       // public Curso cursoAluno { get; set; }
    }
}