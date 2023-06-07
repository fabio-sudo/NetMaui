using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppCursoProgramacao.Model
{
    public class ProfessorModel
    {
        public int IdProfessor { get; set; }
        public string NomeProfessor { get; set; } = "";
        public string SobrenomeProfessor { get; set; } = "";
        public string? CpfProfessor { get; set; } = "";
        public string? CelularProfessor { get; set; } = "";
        public string? EnderecoProfessor { get; set; } = "";
        public DateTime? DataNascimentoProfessor { get; set; } = null!;
        public string? DataNascimentoProfessorStr { get; set; } = "";

        public string? NomeProfessorCompleto { get { return NomeProfessor + " "+SobrenomeProfessor; } }
    }
}
