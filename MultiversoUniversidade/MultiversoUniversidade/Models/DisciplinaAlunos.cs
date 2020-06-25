using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class DisciplinaAlunos
    {

        public string nota { get; set; }
        public int idAluno { get; set; }
        public int idDisciplina { get; set; }
        public string descricaoDisciplina { get; set; }
        public string nomeAluno { get; set; }
        public string data { get; set; }
        public string nomeProfessor { get; set; }
    }
}