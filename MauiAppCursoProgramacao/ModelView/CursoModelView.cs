using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;


namespace MauiAppCursoProgramacao.ModelView
{
   public class CursoModelView: BaseBinding
    {
        private CursoModel _CursoModel;
        private List<CursoModel> _CursoLista;

        public List<CursoModel> ListaCurso
        {
            get { return _CursoLista; }
            set => SetValue(ref _CursoLista, value);
        }

        public CursoModel Curso
        {
            get { return _CursoModel; }
            set => SetValue(ref _CursoModel, value);
        }


    }
}
