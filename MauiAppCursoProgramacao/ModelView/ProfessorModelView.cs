using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppProfessorProgramacao.ModelView
{
    public class ProfessorModelView : BaseBinding
    {

        private ProfessorModel _ProfessorModel;
        private List<ProfessorModel> _ProfessorLista;

        public List<ProfessorModel> ListaProfessor
        {
            get { return _ProfessorLista; }
            set => SetValue(ref _ProfessorLista, value);
        }

        public ProfessorModel Professor
        {
            get { return _ProfessorModel; }
            set => SetValue(ref _ProfessorModel, value);
        }



    }
}
