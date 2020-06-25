using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class Curso
    {
        public int id { get; set; }
        public int ativo { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        
        public   ICollection<DisciplinaCurso>disciplinaCurso { get; set; }
        public   ICollection<AlunosCurso> alunosCurso { get; set; }


    }
}