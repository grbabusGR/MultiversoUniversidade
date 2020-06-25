using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiversoUniversidade.Models
{
    public class Disciplina
    {
        public int id { get; set; }
        public int ativo { get; set; }
        public ICollection<DisciplinaProfessor> disciplinaProfessor { get; set; }
        public int ciclo { get; set; }
        public string nome { get; set; }
        public string descricao { get; set; }
        public   ICollection<DisciplinaCurso> disciplinaCurso { get; set; }
        public   ICollection<Nota> Notas { get; set; }


    }
}