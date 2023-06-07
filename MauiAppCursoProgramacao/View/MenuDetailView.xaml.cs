using MauiAppCursoProgramacao.Model;
using MauiAppCursoProgramacao.ModelView;


namespace MauiAppCursoProgramacao.View;

public partial class MenuDetailView : ContentPage
{
    public MenuViewModel objMenuLista { get; set; }//Encapsulameto
    public MenuDetailView()
	{
		InitializeComponent();


        objMenuLista = new MenuViewModel();
        objMenuLista.Lista = new List<MenuModel>//Adiciona menus a lista
        {
            new MenuModel() { nomeDescricao = "Usuario", nomeIcone = "usericone" },
            new MenuModel() { nomeDescricao = "Alunos", nomeIcone = "estudante_icone" },
            new MenuModel() { nomeDescricao = "Professor", nomeIcone = "professor_icone" },
            new MenuModel() { nomeDescricao = "Período", nomeIcone = "periodo_icone" },
            new MenuModel() { nomeDescricao = "Matrícula", nomeIcone = "matricula_icone" },
            new MenuModel() { nomeDescricao = "Sair", nomeIcone = "sair_icone" }
        }; // Inicializa a lista

        BindingContext = this;

        metodoCarregarUsuario();
    }

    private void metodoCarregarUsuario() {

        Dispatcher.DispatchAsync(() =>
        {
            lbUsuario.Text = Preferences.Get("usuarioLogado", "");
        });

    }

    private void ItemLista_Tapped(object sender, TappedEventArgs e)
    {   
        var frame = (Frame)sender;
        var item = frame.BindingContext;

        lstMenu.SelectedItem = item; // Simula a seleção do item no CollectionView
    }

    private async void lstMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            // Verifique se algum item foi selecionado
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                // Obtenha o aluno selecionado
                MenuModel selectedItem = e.CurrentSelection[0] as MenuModel;


                if (selectedItem.nomeDescricao == "Usuario")
                {
                    _ = App.Navegacao.PushAsync(new UsuarioView());

                }

                else if (selectedItem.nomeDescricao == "Alunos")
                {
                    _ = App.Navegacao.PushAsync(new AlunoBuscarView());

                }
                else if (selectedItem.nomeDescricao == "Sair")
                {
                    string usuarioLogado = Preferences.Get("usuarioLogado", "");

                    await Application.Current.MainPage.DisplayAlert("Logout", "Usuário Desconectado: " + usuarioLogado, "OK");
                    Preferences.Remove("usuarioLogado");
                    Application.Current.MainPage = new LoginView();
                }
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    App.MenuApp.IsPresented = false;
                }
            }

                // Limpe a seleção para evitar que o evento seja acionado novamente acidentalmente
                ((CollectionView)sender).SelectedItem = null;
        }
        catch (Exception ex)
        {

            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
        }
    }

}
