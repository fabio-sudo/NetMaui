using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppCursoProgramacao.ModelView
{
  public class MatriculaModelView: BaseBinding
    {
        private MatriculaModel _matricula;

        public MatriculaModel matricula
        {
            get { return _matricula; }
            set => SetValue(ref _matricula, value);
        }




    }
}
