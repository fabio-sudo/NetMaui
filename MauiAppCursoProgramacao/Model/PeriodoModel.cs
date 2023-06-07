using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppCursoProgramacao.Model
{
  public class PeriodoModel
    {
        public int IdPeriodo { get; set; }
        public string NomePeriodo { get; set; } = "";
        public DateTime? HorarioInicial { get; set; } = null!;
        public DateTime? HorarioFinal { get; set; } = null!;
        public string? HorarioInicialStr { get; set; } = "";
        public string? HorarioFinalStr { get; set; } = "";

        public string HorarioFormatado
        {
            get
            {
                if (HorarioInicial.HasValue && HorarioFinal.HasValue)
                {
                    return $"{NomePeriodo}: {HorarioInicial.Value.ToString("HH:mm")} - {HorarioFinal.Value.ToString("HH:mm")}";
                }
                return string.Empty;
            }
        }
    }
}
