using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppCursoProgramacao.ModelView
{
  public class MatriculaModelView: BaseBinding
    {
        private MatriculaModel _matricula;
        private List<MatriculaModel> _listaMatricula;


        public MatriculaModel matricula
        {
            get { return _matricula; }
            set => SetValue(ref _matricula, value);
        }

        public List<MatriculaModel> ListaMatricula
        {
            get { return _listaMatricula; }
            set => SetValue(ref _listaMatricula, value);
        }

        //----------------------------Atualizando Grid
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set => SetValue(ref isRefreshing, value);
        }


        //--------------------------Command
        private Command _RefreshCommand;
        public Command RefreshCommand
        {

            get { return _RefreshCommand; }
            set => SetValue(ref _RefreshCommand, value);
        }

    }
}
