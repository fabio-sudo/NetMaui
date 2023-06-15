using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;
using System.Text;
using System.Text.RegularExpressions;

namespace MauiAppCursoProgramacao.View;

public partial class MatriculaBuscarView : ContentPage
{
    int quantidadeLimitador = 10; // Define a quantidade máxima de itens desejada
    int quantidadeMaximaItem = 0;

    public event Action<string> OnAlunoAddCompleted;//Evento para Dialog result

    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApiAluno = App.Current.Resources["urlAlunoFK"].ToString();

    public AlunoModelView objAluno { get; set; }

    //Lista de itens JÁ ADD
    public List<AlunoModel> ListaCorrente { get; set; }//Lista está vinculada entre os dois formularios
    public List<AlunoModel> Lista { get; set; }//Lista 

    public MatriculaBuscarView(List<AlunoModel> listaAdd)
    {
        //Aluno
        objAluno = new AlunoModelView(); // Inicialize objAluno aqui
        objAluno.Aluno = new AlunoModel();
        objAluno.ListaAluno = new List<AlunoModel>();

        //Itens já adicionados
        ListaCorrente = listaAdd;

        InitializeComponent();

        BindingContext = this;
        _=metodoConstrutor("");
    }

    private void ConcluirAcao(string retorno)
    {
        OnAlunoAddCompleted?.Invoke(retorno);
    }

    private async Task metodoConstrutor(string nomeAluno)
    {
        try
        {
            // Consumindo API
            if (nomeAluno == "")
            {
                objAluno.ListaAluno = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApiAluno + "/BuscarAlunoFK", token);
            }
            else
            {
                objAluno.ListaAluno = await ClientHttp.BuscarLista<AlunoModel>(urlBase, rotaApiAluno + "/BuscarAlunoPorNomeFK" + nomeAluno, token);
            }

            if (ListaCorrente.Count > 0)
            {
                // Remover os valores iguais entre as listas
                objAluno.ListaAluno.RemoveAll(aluno => ListaCorrente.Any(alunoCorrente => alunoCorrente.IdAluno == aluno.IdAluno));
            }

            Lista = objAluno.ListaAluno;
            quantidadeMaximaItem = objAluno.ListaAluno.Count();
            objAluno.ListaAluno = objAluno.ListaAluno.Take(quantidadeLimitador).ToList();
        }
        catch (HttpRequestException ex)
        {
            // Tratar o erro de rede
            throw new Exception("Ocorreu um erro de rede: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Tratar outras exceções
            throw new Exception("Ocorreu um erro: " + ex.Message);
        }
    }

    //Adiciona os itens a lista
    private async void btnAdicionar_Clicked(object sender, EventArgs e)
    {
        //ConcluirAcao("ok");
        foreach (var aluno in objAluno.ListaAluno)
        {
            if (aluno.IsSelected == true)
            {
                ListaCorrente.Add(aluno);
            }
        }
        if (ListaCorrente.Count > 0)
        {
            ConcluirAcao("ok");
        }

        await Navigation.PopModalAsync();
    }

    private async void txtBuscarAluno_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string searchText = e.NewTextValue;

            if (!string.IsNullOrEmpty(searchText))
            {

                objAluno.ListaAluno = Lista;//Caso a lista for menor que a lista na memória

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

                objAluno.ListaAluno = Lista;

            }
        }
        catch (Exception ex) { await DisplayAlert("Erro", ex.Message, "OK"); }
    }

    //Ve o aluno selecionado
    private async void btnVer_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        objAluno.Aluno = button.BindingContext as AlunoModel; // Substitua "SeuModelo" pelo tipo de objeto na sua coleção

        if (objAluno.Aluno != null)
        {
            MatriculaAlunoView _MatriculaView = new MatriculaAlunoView(objAluno.Aluno); // Instancie a página do formulário
            await Navigation.PushModalAsync(_MatriculaView);
        }
    }

    //Abre whats aap
    private void btnWhats_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        objAluno.Aluno = button.BindingContext as AlunoModel; // Substitua "SeuModelo" pelo tipo de objeto na sua coleção

        string numeroCelular = Regex.Replace(objAluno.Aluno.CelularAluno, "[^0-9]", "");
        // Abra o link do WhatsApp
        var uri = new Uri("https://wa.me/" + numeroCelular);
        _ = Launcher.OpenAsync(uri);
    }

    //Volta ao formulário anterior
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    //Carrega mais itens 
    private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
    {
        double scrollY = scvScroll.ContentSize.Height - scvScroll.Height;

        if (e.ScrollY >= scrollY - 40) // Define um valor de margem inferior para acionar a páginação (40 no exemplo)
        {
            if (quantidadeMaximaItem > quantidadeLimitador)
            {
                objAluno.ListaAluno.AddRange(Lista.Skip(quantidadeLimitador));
                quantidadeLimitador += 10;
                
                objAluno.ListaAluno = objAluno.ListaAluno.Take(quantidadeLimitador).ToList();
            }
        }

    }



}