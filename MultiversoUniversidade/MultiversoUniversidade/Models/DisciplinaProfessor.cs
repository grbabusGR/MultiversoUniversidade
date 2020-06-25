using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class DisciplinaProfessor
    {
        public int id { get; set; }
        public int idProfessor { get; set; }
        public int idDisciplina { get; set; }

        [NotMapped]
        public string Descricao { get; set; }
        [NotMapped]
        public string nota { get; set; }

        [NotMapped]
        public int idAluno { get; set; }
    }
}