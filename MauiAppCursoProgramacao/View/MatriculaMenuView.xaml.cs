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
        MatriculaView matriculaAdd = new MatriculaView("Cadastrar",0);//(null, "Cadastro"); // Instancie a página do formulário
                                                                     //await Navigation.PushAsync(matriculaAdd);
        await Navigation.PushModalAsync(matriculaAdd);

    }

    //Selecionar item
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var frame = (Frame)sender;
        var item = frame.BindingContext;

        lstListaMatriculas.SelectedItem = item; // Simula a seleção do item no CollectionView
    }

    private async void lstListaMatriculas_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Verifique se algum item foi selecionado
        if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
        {
            // Obtenha o aluno selecionado
            MatriculaModel matriculaSelecionada = e.CurrentSelection[0] as MatriculaModel;

            string acaoSelecionada = await Application.Current.MainPage.DisplayActionSheet("Opções", "Cancelar", null, "Alterar", "Excluir");

            // Verifique a ação selecionada
            if (acaoSelecionada == "Excluir")
            {
                MatriculaView _matricula = new MatriculaView("Excluir",matriculaSelecionada.ordemMatricula); // Instancie a página do formulário
                await Navigation.PushModalAsync(_matricula);

                //metodoAtulizaFormulario(alunoAdd);
            }
            else if (acaoSelecionada == "Alterar")
            {
                MatriculaView _matricula = new MatriculaView("Alterar", matriculaSelecionada.ordemMatricula); // Instancie a página do formulário
                await Navigation.PushModalAsync(_matricula);

                //metodoAtulizaFormulario(alunoAdd);
            }
        }

    // Limpe a seleção para evitar que o evento seja acionado novamente acidentalmente
    ((CollectionView)sender).SelectedItem = null;
    }
}