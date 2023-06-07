using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppPeriodoProgramacao.ModelView
{
   public class PeriodoModelView : BaseBinding
    {
        private PeriodoModel _PeriodoModel;
        private List<PeriodoModel> _PeriodoLista;

        public List<PeriodoModel> ListaPeriodo
        {
            get { return _PeriodoLista; }
            set => SetValue(ref _PeriodoLista, value);
        }

        public PeriodoModel Periodo
        {
            get { return _PeriodoModel; }
            set => SetValue(ref _PeriodoModel, value);
        }
    
    }
}
