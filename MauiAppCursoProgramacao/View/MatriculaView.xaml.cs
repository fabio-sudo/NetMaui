using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;
using MauiAppPeriodoProgramacao.ModelView;
using MauiAppProfessorProgramacao.ModelView;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiAppCursoProgramacao.View;

public partial class MatriculaView : ContentPage
{
    //Extends appXaml
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApi = App.Current.Resources["urlMatricula"].ToString();

    string rotaApiAluno = App.Current.Resources["urlAluno"].ToString();
    string rotaApiCurso = App.Current.Resources["urlCurso"].ToString();
    string rotaApiPeriodo = App.Current.Resources["urlPeriodo"].ToString();
    string rotaApiProfessor = App.Current.Resources["urlProfessor"].ToString();

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

        metodoGeraListas();

    }

    //Gera listas para preencher os combobox
    private async void metodoGeraListas() {
        try
        {

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
        btnAdicionar.IsEnabled = false;

        MatriculaBuscarView _MatriculaView = new MatriculaBuscarView(objAluno.ListaAluno); // Instancie a página do formulário
        await Navigation.PushModalAsync(_MatriculaView);

        _MatriculaView.OnAlunoAddCompleted += (retorno) =>
        {
            //Espera o retorno para poder realizar atualização
            if (retorno == "ok")
            {

                objMartricula.matricula.listaAlunos = _MatriculaView.ListaCorrente;
                objAluno.ListaAluno = new List<AlunoModel>(objMartricula.matricula.listaAlunos);
            }
        };

        btnAdicionar.IsEnabled = true;

    }

    //Ver o whatsApp pelo aluno 
    private void btnVer_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        objAluno.Aluno = button.BindingContext as AlunoModel; // Substitua "SeuModelo" pelo tipo de objeto na sua coleção

        string numeroCelular = Regex.Replace(objAluno.Aluno.CelularAluno, "[^0-9]", "");
        // Abra o link do WhatsApp
        var uri = new Uri("https://wa.me/" + numeroCelular);
        _ = Launcher.OpenAsync(uri);
    }

    //Remove o aluno da lista
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

    private async void btnAlunoVer_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        objAluno.Aluno = button.BindingContext as AlunoModel; // Substitua "SeuModelo" pelo tipo de objeto na sua coleção

        if (objAluno.Aluno != null)
        {
            MatriculaAlunoView _MatriculaView = new MatriculaAlunoView(objAluno.Aluno); // Instancie a página do formulário
            await Navigation.PushModalAsync(_MatriculaView);
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

                objAluno.ListaAluno = objMartricula.matricula.listaAlunos;//Caso a lista for menor que a lista na memória

                string normalizedSearchText = searchText.Normalize(NormalizationForm.FormD);
                var filteredAlunos = objAluno.ListaAluno.Where(p =>
                    p.NomeAluno != null &&
                    p.NomeAluno.Normalize(NormalizationForm.FormD)
                        .IndexOf(normalizedSearchText, StringComparison.OrdinalIgnoreCase) >= 0
                );

                objAluno.ListaAluno = filteredAlunos.ToList();
            }
            else
            {

                objAluno.ListaAluno = objMartricula.matricula.listaAlunos;

            }
        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }

    }

    //Valida os campos
    private async Task<bool> metodoValidaCadastroAsync()
    {
        if (picDiaSemana.SelectedItem == null)
        {

            await DisplayAlert("Erro Dia Semana", "Selecione o dia da Semana!", "Ok");
            return false;
        }
        else if (picPeriodo.SelectedItem == null)
        {

            await DisplayAlert("Erro Período", "Selecione o Período!", "Ok");
            return false;
        }
        else if (picCurso.SelectedItem == null)
        {

            await DisplayAlert("Erro Curso", "Selecione o Curso!", "Ok");
            return false;
        }
        else if (picProfessor.SelectedItem == null)
        {

            await DisplayAlert("Erro Professor", "Selecione o Professor!", "Ok");
            return false;
        }
        else if (objAluno.ListaAluno.Count <= 0)
        {

            await DisplayAlert("Erro Alunos", "Adicione os Alunos!", "Ok");
            return false;
        }

        return true;
    }

    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool camposValidos = await metodoValidaCadastroAsync();

            if (camposValidos == true)
            {
                // Success
                bool resposta = await DisplayAlert("Matricular Alunos", "Deseja realmenter realizar a matrícula?", "Sim", "Não");

                if (resposta == true)
                {
                    btnSalvar.IsVisible = false;
                    barraProgresso.IsRunning = true;
                    //Criando objeto Matricula
                    objMartricula.matricula.curso = objCurso.Curso;
                    objMartricula.matricula.periodo = objPerido.Periodo;
                    objMartricula.matricula.professor = objProfessor.Professor;
                    objMartricula.matricula.diaSemana = objMartricula.matricula.diaSemana;
                    objMartricula.matricula.statusCurso = "Ativo";

                    objMartricula.matricula.listaAlunos = objAluno.ListaAluno;
           
                    int result = await ClientHttp.Adicionar(urlBase, rotaApi + "/AdicionarMatricula", objMartricula.matricula, token);
                   
                    if (result == 1)
                    {
                        await DisplayAlert("Sucesso", "Matrícula realizada com sucesso!", "Ok");
                        await Navigation.PopAsync();
                    }
                    else {

                        await DisplayAlert("Erro", "Não foi possível realizar a matrícula!", "Ok");
                    }
                    barraProgresso.IsRunning = false;
                    btnSalvar.IsVisible = true;
                }
            }
        }catch (Exception ex) { await DisplayAlert("Erro", ex.Message,"Ok"); }
    }


}