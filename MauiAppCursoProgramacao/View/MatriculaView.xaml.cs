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
    //Retorno conclusão
    public event Action<string> OnAlunoAddCompleted;//Evento para Dialog result

    //Construtor
    public MatriculaModel _matriculaSelecionada = new MatriculaModel();
    string tipoPage = "";

    //Extends appXaml
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApi = App.Current.Resources["urlMatricula"].ToString();

    //string rotaApiAluno = App.Current.Resources["urlAluno"].ToString();
    string rotaApiCurso = App.Current.Resources["urlCurso"].ToString();
    string rotaApiPeriodo = App.Current.Resources["urlPeriodo"].ToString();
    string rotaApiProfessor = App.Current.Resources["urlProfessor"].ToString();

    public MatriculaModelView objMartricula { get; set; }
    public AlunoModelView objAluno { get; set; }
    public CursoModelView objCurso { get; set; }
    public PeriodoModelView objPerido { get; set; }
    public ProfessorModelView objProfessor { get; set; }
    public MatriculaView(string tipoPageSelecionada, MatriculaModel matriculaSelecionada)
    {
        //Passar o objeto matricula em vez de somente a ordem selecionada
        //Para realizar as devidas atualizações ou exclusões

        InitializeComponent();

        //Pagina Genérica
        tipoPage = tipoPageSelecionada;
        _matriculaSelecionada = matriculaSelecionada;

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


    //Preenche o formulário quando tipoPage for Alterar ou Excluir
    private async void metodoConstrutor() {

        try
        {
            if (tipoPage.Equals("Cadastro"))
            {
                btnSalvar.BackgroundColor = Colors.DarkCyan;

            }

            else if (tipoPage.Equals("Alterar"))
            {

                objMartricula.matricula.listaAlunos = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi + "/BuscarMatriculaAlunos?ordemMatricula=" + _matriculaSelecionada.ordemMatricula, token);
                objMartricula.matricula.ordemMatricula = _matriculaSelecionada.ordemMatricula;
                objAluno.ListaAluno = new List<AlunoModel>(objMartricula.matricula.listaAlunos);

                //Preenche os dataPicer
                metodoPreencherListas();

                btnSalvar.BackgroundColor = Color.FromArgb("#2196F3");
                btnSalvar.Text = "Alterar";
            }

            else if (tipoPage.Equals("Excluir"))
            {
                objMartricula.matricula.listaAlunos = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi + "/BuscarMatriculaAlunos?ordemMatricula=" + _matriculaSelecionada.ordemMatricula, token);
                objAluno.ListaAluno = new List<AlunoModel>(objMartricula.matricula.listaAlunos);
                //Preenche os dataPicer
                metodoPreencherListas();

                btnSalvar.BackgroundColor = Colors.Red;

                picCurso.IsEnabled = false;
                picDiaSemana.IsEnabled = false;
                picProfessor.IsEnabled = false;
                picPeriodo.IsEnabled = false;

                btnAdicionar.IsEnabled = false;
                btnSalvar.Text = "Excluir";
            }

            else if (tipoPage.Equals("Ver"))
            {
                objMartricula.matricula.listaAlunos = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApi + "/BuscarMatriculaAlunos?ordemMatricula=" + _matriculaSelecionada.ordemMatricula, token);
                objAluno.ListaAluno = new List<AlunoModel>(objMartricula.matricula.listaAlunos);
                //Preenche os dataPicer
                metodoPreencherListas();

                btnVoltar.IsVisible = false;
                btnSalvar.BackgroundColor = Colors.DarkCyan;
                btnSalvar.Text = "Voltar";
            }
        }
        catch (Exception ex) { throw new Exception(ex.Message); }
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

            metodoConstrutor();

        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }
    }

    private void metodoPreencherListas() {
      
        // Encontre o objeto do curso desejado na lista de cursos
        var cursoSelecionado = objCurso.ListaCurso.FirstOrDefault(p => p.NomeCurso == _matriculaSelecionada.curso.NomeCurso);
        picCurso.SelectedItem = cursoSelecionado;

        var professorSelecionado = objProfessor.ListaProfessor.FirstOrDefault(p => p.NomeProfessor == _matriculaSelecionada.professor.NomeProfessor);
        picProfessor.SelectedItem = professorSelecionado;

        var periodoSelecionado = objPerido.ListaPeriodo.FirstOrDefault(p => p.NomePeriodo == _matriculaSelecionada.periodo.NomePeriodo);
        picPeriodo.SelectedItem = periodoSelecionado;

        picDiaSemana.SelectedItem = _matriculaSelecionada.diaSemana;
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

    //Botão voltar
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        try
        {
            //---------------------------------Cadastrar Matricula
            if (btnSalvar.Text == "Salvar")
            {

                bool camposValidos = await metodoValidaCadastroAsync();

                if (camposValidos == true)
                {
                    // Success
                    bool resposta = await DisplayAlert("Matricular Alunos", "Deseja realmenter realizar a matrícula?", "Sim", "Não");

                    if (resposta == true)
                    {
                        btnSalvar.IsVisible = false;
                        btnVoltar.IsVisible = false;
                        barraProgresso.IsRunning = true;
                        //Criando objeto Matricula
                        objMartricula.matricula.curso = objCurso.Curso;
                        objMartricula.matricula.periodo = objPerido.Periodo;
                        objMartricula.matricula.professor = objProfessor.Professor;
                        objMartricula.matricula.diaSemana = objMartricula.matricula.diaSemana;
                        objMartricula.matricula.statusCurso = "Ativo";

                        objMartricula.matricula.listaAlunos = objAluno.ListaAluno;

                        int result = await ClientHttp.Adicionar(urlBase, rotaApi + "/AdicionarMatricula", objMartricula.matricula, token);
                        if (result != 0)
                        {
                            objMartricula.matricula.ordemMatricula = result;//API retorna Matricula como retorno
                            ConcluirAcao("CadastroOK");//Cadastro Realizado
                            await DisplayAlert("Sucesso", "Matrícula realizada com sucesso!", "Ok");
                            await Navigation.PopModalAsync();
                        }
                        else
                        {

                            await DisplayAlert("Erro", "Não foi possível realizar a matrícula!", "Ok");
                        }
                        barraProgresso.IsRunning = false;
                        btnVoltar.IsVisible = true;
                        btnSalvar.IsVisible = true;
                    }
                }
            }

            else if (btnSalvar.Text == "Alterar") {

                bool camposValidos = await metodoValidaCadastroAsync();

                if (camposValidos == true)
                {
                    // Success
                    bool resposta = await DisplayAlert("Alterar Matricula", "Deseja realmenter altualizar a matrícula?", "Sim", "Não");

                    if (resposta == true)
                    {
                        btnSalvar.IsVisible = false;
                        barraProgresso.IsRunning = true;
                        btnVoltar.IsVisible = false;

                        //Criando objeto Matricula
                        objMartricula.matricula.curso = objCurso.Curso;
                        objMartricula.matricula.periodo = objPerido.Periodo;
                        objMartricula.matricula.professor = objProfessor.Professor;
                        objMartricula.matricula.diaSemana = objMartricula.matricula.diaSemana;
                        objMartricula.matricula.statusCurso = "Ativo";

                        objMartricula.matricula.listaAlunos = objAluno.ListaAluno;

                        /*string BaseAddress =
                         DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:7175" : "https://localhost:7175";

                        ClientHttp client = new ClientHttp();*/

                        bool result = await ClientHttp.Alterar(urlBase, rotaApi + "/AlterarMatricula", objMartricula.matricula, token);
                        if (result == true)
                            {
                                ConcluirAcao("AlterarOK");//Cadastro Realizado
                                await DisplayAlert("Sucesso", "Matrícula atualizada com sucesso!", "Ok");
                                await Navigation.PopModalAsync();
                            }
                            else
                            {
                                await DisplayAlert("Erro", "Não foi possível realizar a matrícula!", "Ok");
                            }
                        barraProgresso.IsRunning = false;
                        btnSalvar.IsVisible = true;
                        btnVoltar.IsVisible = true;

                    }

                    }
               }

            //---------------------------------Ver Dados Matriculados
            else if (btnSalvar.Text == "Voltar")
            {
                ConcluirAcao("");
                await Navigation.PopModalAsync();
            }

            //------------------------------------Excluir Matricula completa
            else if (btnSalvar.Text == "Excluir")
            {
                // Success
                bool resposta = await DisplayAlert("Excluir?", "Deseja realmenter remover as Matriculas?", "Sim", "Não");

                if (resposta == true)
                {
                    barraProgresso.IsRunning = true;
                    btnSalvar.IsVisible = false;
                    btnVoltar.IsVisible = false;
                    //Consumindo API
                    int result = await ClientHttp.Excluir(urlBase, rotaApi + "/ExcluirAlunosMatriculados/?Matricula=" + _matriculaSelecionada.ordemMatricula, token);

                    if (result == 1)
                    {
                        // Success
                        await DisplayAlert("Sucesso", "Matrículas excluidas com sucesso!", "OK");
                        //Retorno para formulario anterior Para o mesmo atualizar
                        ConcluirAcao("ExcluirOK");
                        await Navigation.PopModalAsync();

                    }
                    else
                    {
                        await DisplayAlert("Erro", "Erro ao excluir Aluno!", "OK");

                    }
                    barraProgresso.IsRunning = false;
                    btnSalvar.IsVisible = true;
                    btnVoltar.IsVisible = true;
                }
            }
        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message,"Ok"); }
    }

    // Método ou evento que aciona a conclusão da ação na página AlunoView
    private void ConcluirAcao(string retorno)
    {
        OnAlunoAddCompleted?.Invoke(retorno);
    }


}