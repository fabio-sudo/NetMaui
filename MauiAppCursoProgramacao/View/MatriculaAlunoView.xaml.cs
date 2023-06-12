using MauiAppCursoProgramacao.ModelView;
using MauiAppCursoProgramacao.Generico;
using MauiAppCursoProgramacao.Model;

namespace MauiAppCursoProgramacao.View;

public partial class MatriculaAlunoView : ContentPage
{
    string urlBase = App.Current.Resources["urlBase"].ToString();
    string token = App.Current.Resources["token"].ToString();
    string rotaApiAluno = App.Current.Resources["urlAluno"].ToString();
    public AlunoModelView objAluno { get; set; }

    public MatriculaAlunoView(AlunoModel aluno)
    {

        InitializeComponent();

        objAluno = new AlunoModelView();
        objAluno.Aluno = new AlunoModel();
        objAluno.Aluno = aluno;

        BindingContext = this;

        metodoConstrutorAsync();
    }
    private async void metodoConstrutorAsync() {

        try
        {
            objAluno.IsRefreshing = true;
            var alunoSelecionado = await ClientHttp.Buscar<AlunoModel>(urlBase, rotaApiAluno + "/BuscarPorID" + objAluno.Aluno.IdAluno.ToString(), token);
            
            if (string.IsNullOrEmpty(alunoSelecionado.imgStr))
            {
                alunoSelecionado.imgStr = "photo_icone.jpg";
            }

            objAluno.Aluno = alunoSelecionado;
            objAluno.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();

    }
}