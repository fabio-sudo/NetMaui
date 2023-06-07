namespace MauiAppCursoProgramacao.Metodos
{
   public class AnimacaoBotao
    {
        public async void metodoAnimacaoBotao(ImageButton btn)
        {
            Animation animation = new Animation();

            // Defina as propriedades de animação desejadas, por exemplo, Opacity e Scale
            animation.Add(0, 0.5, new Animation(v => btn.Opacity = v, 1, 0));
            //animation.Add(0, 1, new Animation(v => btn.Scale = v, 1, 0.8, Easing.SpringOut));

            // Defina a duração da animação
            animation.Commit(btn, "ButtonClickAnimation", 16, 500);

            // Adicione qualquer lógica adicional que você precisa executar quando o botão é clicado

            // Aguarde o término da animação antes de prosseguir
            await Task.Delay(500);

            // Restaure as propriedades para os valores originais ou execute outras ações necessárias
            btn.Opacity = 1;
            btn.Scale = 1;

        }
    }
}
