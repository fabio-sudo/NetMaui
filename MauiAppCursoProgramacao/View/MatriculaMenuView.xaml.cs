using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;

namespace MauiAppCursoProgramacao.View;

public partial class MatriculaMenuView : ContentPage
{
    //Instanciado constantes para utilização API
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApi = App.Current.Resources["urlMatricula"].ToString();
    public MatriculaModelView objMatricula {get;set;}
	public MatriculaMenuView()
	{
		InitializeComponent();

		objMatricula = new MatriculaModelView();
		objMatricula.ListaMatricula = new List<MatriculaModel>();
		objMatricula.matricula = new MatriculaModel();

        BindingContext = this;

		metodoConstrutor();

    }

	//Busca Turmas Cadastradas
	private async void metodoConstrutor() {
		try
		{
			objMatricula.IsRefreshing = true;
            objMatricula.ListaMatricula = await ClientHttp.BuscarLista<MatriculaModel>(urlBase, rotaApi + "/BuscarMatriculaCurso", token);

            //Coloca imagem referente ao curso
            foreach (MatriculaModel matricula in objMatricula.ListaMatricula)
            {
                if (matricula.curso.NomeCurso == "Informática Básica")
                {
                    matricula.imgCurso = "informatica_icone.png";// Definir a imagem correspondente ao Curso 1
                }
                else if (matricula.curso.NomeCurso == "Cabelereiro")
                {
                    matricula.imgCurso = "cabelereiro_icone.png";
                }
                else if (matricula.curso.NomeCurso == "Cozinha")
                {
                    matricula.imgCurso = "cozinha_icone.png";
                }
                else if (matricula.curso.NomeCurso == "Depilação")
                {
                    matricula.imgCurso = "depilacao_icone.png";
                }
                else if (matricula.curso.NomeCurso == "Programação")
                {
                    matricula.imgCurso = "programacao_icone.png";
                }
                else if (matricula.curso.NomeCurso == "Costura")
                {
                    matricula.imgCurso = "costura_icone.png";
                }
                else if (matricula.curso.NomeCurso == "Maquiagem")
                {
                    matricula.imgCurso = "maquiagem_icone.png";
                }
                else { matricula.imgCurso = "matricula_icone.png"; }
            }

            objMatricula.IsRefreshing = false;

		}catch(Exception ex) { throw new Exception(ex.Message); }
	}

    private  async void btnAdicionar_Clicked(object sender, EventArgs e)
    {
        MatriculaView matriculaAdd = new MatriculaView();//(null, "Cadastro"); // Instancie a página do formulário
                                                         //await Navigation.PushAsync(matriculaAdd);
        await Navigation.PushModalAsync(matriculaAdd);

    }
}