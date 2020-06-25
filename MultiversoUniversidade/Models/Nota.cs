using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class Nota
    {
        public int id { get; set; }
        public int idAluno { get; set; }
        public int idDisciplina { get; set; }
        public int idProfessor { get; set; }
        public int nota { get; set; }
        public DateTime data { get; set; }
    }
}