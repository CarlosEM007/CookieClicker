using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;

namespace CookieClicker
{
    public partial class MainWindow : Window
    {
        // Jogador
        private double poderClicks = 1;
        private double totalCookies = 0;
        private double cookiesPorSegundo = 0;

        
        // Preço melhorias
        private double precoPonteiros = 10;
        private double precoIdosa = 20;


        // Informações Extras
        private int contador = 0;
        private DispatcherTimer timer;
        private int totalclicks = 0;

        public MainWindow()
        {
            InitializeComponent();
            IniciarTimer();
        }

        private void IniciarTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); 
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            contador++;
            AtualizaValores();
            CookiesPorSegundo();

        }

        private void AtualizaValores()
        {
            PoderClick.Text = $"Poder de Click: {poderClicks.ToString("f1")}";
            CookieSegundos.Text = $"CPS: {cookiesPorSegundo.ToString("f1")}";
            TotalClicks.Text = $"Total de clicks: {totalclicks.ToString("f1")}";
            TempoJogado.Text = $"Tempo Jogado em  segundos: {contador.ToString("f1")}";
            TotalCookies.Text = $"{totalCookies.ToString("f1")} Cookies";
            CustoPonteiro.Text = $"Custo: {precoPonteiros.ToString("f1")}";
            CustoIdosa.Text = $"Custo: {precoIdosa.ToString("f1")}";
        }

        private async void Clicker_Click(object sender, RoutedEventArgs e)
        {
            totalCookies += poderClicks;
            totalclicks++;
            TotalCookies.Text = $"{totalCookies.ToString("f1")} Cookies";
            AnimateCookie();
        }

        private void Pointer_Click(object sender, RoutedEventArgs e)
        {           
            if (totalCookies >= precoPonteiros)
            {
                totalCookies -= precoPonteiros;
                precoPonteiros += (precoPonteiros * 0.3);
                if(cookiesPorSegundo == 0)
                {
                    cookiesPorSegundo += (1 * 0.2);
                }
                cookiesPorSegundo += (cookiesPorSegundo * 0.2);
            }
        }

        private void Btn_Avo_Click(object sender, RoutedEventArgs e)
        {
            if(totalCookies >= precoIdosa)
            {
                totalCookies -= precoIdosa;
                precoIdosa += (precoIdosa * 0.4);
                if (cookiesPorSegundo == 0)
                {
                    cookiesPorSegundo += (1 * 0.4);
                }
                cookiesPorSegundo += (cookiesPorSegundo * 0.4);
            }

        }

        private void CookiesPorSegundo()
        {
            totalCookies += cookiesPorSegundo;
        }

        private void AnimateCookie()
        {
            // Animação para o efeito esbranquiçado (alteração da opacidade)
            ColorAnimation colorAnimation = new ColorAnimation();
            colorAnimation.From = Colors.White;
            colorAnimation.To = Colors.Transparent;
            colorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            colorAnimation.AutoReverse = true; // Para voltar ao normal após o efeito

            // Criar um efeito de esbranquiçado ao clicar (usando um Brush)
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = Cookie.Source;
            Cookie.OpacityMask = new SolidColorBrush(Colors.White);

            SolidColorBrush whiteBrush = new SolidColorBrush(Colors.White);
            whiteBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

            // Efeito de tremor (animação de deslocamento)
            DoubleAnimationUsingKeyFrames shakeAnimationX = new DoubleAnimationUsingKeyFrames();
            shakeAnimationX.Duration = TimeSpan.FromMilliseconds(1000);
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))));
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(50))));
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100))));
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(-5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(150))));
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(5, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))));
            shakeAnimationX.KeyFrames.Add(new EasingDoubleKeyFrame(0, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(300))));

            // Aplicar a animação de tremor
            TranslateTransform translateTransform = new TranslateTransform();
            Cookie.RenderTransform = translateTransform;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, shakeAnimationX);
        }
    }
}
