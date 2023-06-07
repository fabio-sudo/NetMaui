using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Metodos;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;
using MauiAppPeriodoProgramacao.ModelView;
using MauiAppProfessorProgramacao.ModelView;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace MauiAppCursoProgramacao.View;

public partial class MatriculaView : ContentPage
{

    //Extends appXaml
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApiCurso = App.Current.Resources["urlCurso"].ToString();
    string rotaApiPeriodo = App.Current.Resources["urlPeriodo"].ToString();
    string rotaApiProfessor = App.Current.Resources["urlProfessor"].ToString();

    //Realiza Animação
    AnimacaoBotao animacaoBotao = new AnimacaoBotao();

    public MatriculaModelView objMartricula { get; set; }
    public AlunoModelView objAluno { get; set; }
    public CursoModelView objCurso { get; set; }
    public PeriodoModelView objPerido { get; set; }
    public ProfessorModelView objProfessor { get; set; }
    public MatriculaView()
	{

        InitializeComponent();

        objMartricula = new MatriculaModelView();
        objMartricula.matricula = new MatriculaModel();
        
        //Aluno
        objAluno = new AlunoModelView(); // Inicialize objAluno aqui
        objAluno.Aluno = new AlunoModel();
        objAluno.ListaAluno = new List<AlunoModel>();
        //Curso
        objCurso = new CursoModelView();
        objCurso.Curso = new CursoModel();
        objCurso.ListaCurso = new List<CursoModel>();
        //Periodo
        objPerido = new PeriodoModelView();
        objPerido.Periodo = new PeriodoModel();
        objPerido.ListaPeriodo = new List<PeriodoModel>();
        //Professor
        objProfessor = new ProfessorModelView();
        objProfessor.Professor = new ProfessorModel();
        objProfessor.ListaProfessor = new List<ProfessorModel>();



        BindingContext = this;
        metodoGeraAlunos();

    }

	private async void metodoGeraAlunos() {
        try
        {
            for (int i = 1; i <= 10; i++)
        {
            objAluno.Aluno = new AlunoModel();

            objAluno.Aluno.IdAluno = i;
            objAluno.Aluno.NomeAluno = "Nome " + i.ToString();
            objAluno.Aluno.SobrenomeAluno = "Sobrenome " + i.ToString();
            objAluno.Aluno.CpfAluno = "123.456.789-0" + i.ToString();
            objAluno.Aluno.CelularAluno = "(00) 9 9999-999" + i.ToString();
            objAluno.Aluno.EnderecoAluno = "Endereço " + i.ToString();
            objAluno.Aluno.DataNascimentoAlunoStr =  i.ToString();
            objAluno.Aluno.DataNascimentoAluno = DateTime.Now;


            objAluno.ListaAluno.Add(objAluno.Aluno);
        }

        objMartricula.matricula.listaAlunos = objAluno.ListaAluno;

        //Consumindo API   
        objCurso.ListaCurso =
        await ClientHttp.BuscarLista<CursoModel>(urlBase, rotaApiCurso, token);

        objPerido.ListaPeriodo =
        await ClientHttp.BuscarLista<PeriodoModel>(urlBase, rotaApiPeriodo, token);

        objProfessor.ListaProfessor =
        await ClientHttp.BuscarLista<ProfessorModel>(urlBase, rotaApiProfessor, token);

        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }
    }

    private async void btnAdicionar_Clicked(object sender, EventArgs e)
    {
        animacaoBotao.metodoAnimacaoBotao(this.btnAdicionar);

        MatriculaBuscarView _MatriculaView = new MatriculaBuscarView(objAluno.ListaAluno); // Instancie a página do formulário
        await Navigation.PushModalAsync(_MatriculaView);    
    }

    private void btnVer_Clicked(object sender, EventArgs e)
    {
        //var btn = (ImageButton)sender; // Obtém o botão clicado
        //AnimacaoBotao animacaoBotao = new AnimacaoBotao();
        //animacaoBotao.metodoAnimacaoBotao(btn);

        // Success
        //bool resposta = await DisplayAlert("Remover ?", "Deseja realmenter remover o Aluno?", "Sim", "Não");

        //if (resposta == true)
        //{
        //}


    }

    private void btnRemover_Clicked(object sender, EventArgs e)
    {

        var button = (ImageButton)sender;
            objAluno.Aluno = button.BindingContext as AlunoModel; // Substitua "SeuModelo" pelo tipo de objeto na sua coleção

            if (objAluno.Aluno != null)
            {
                objMartricula.matricula.listaAlunos.Remove(objAluno.Aluno);
                objAluno.ListaAluno = new List<AlunoModel>(objMartricula.matricula.listaAlunos);
            }
    }

    //Selecionar objetos
    private void picCurso_SelectedIndexChanged(object sender, EventArgs e)
    {
        objCurso.Curso = picCurso.SelectedItem as CursoModel;
    }

    private void picPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        objPerido.Periodo = picPeriodo.SelectedItem as PeriodoModel;
    }

    private void picProfessor_SelectedIndexChanged(object sender, EventArgs e)
    {
        objProfessor.Professor = picProfessor.SelectedItem as ProfessorModel;
    }

    //Buscar alunos na lista memória
    private async void txtBuscarAluno_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string searchText = e.NewTextValue;

            if (!string.IsNullOrEmpty(searchText))
            {
                if (objAluno.ListaAluno.Count < objMartricula.matricula.listaAlunos.Count)
                {
                    objAluno.ListaAluno = objMartricula.matricula.listaAlunos;//Caso a lista for menor que a lista na memória
                }

                string normalizedSearchText = searchText.Normalize(NormalizationForm.FormD);
                var filteredAlunos = objAluno.ListaAluno.Where(p =>
                    p.NomeAluno != null &&
                    p.NomeAluno.Normalize(NormalizationForm.FormD)
                        .IndexOf(normalizedSearchText, StringComparison.OrdinalIgnoreCase) >= 0
                );

                objAluno.ListaAluno = filteredAlunos.ToList();
            }
        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }

    }

    private  void btnSalvar_Clicked(object sender, EventArgs e)
    {

    }

}