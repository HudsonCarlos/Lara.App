using Lara.Data;
using Lara.Util;
using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lara
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow instanciaLara;

        public MainWindow()
        {
            Constantes.INSTANCIA_LARA = instanciaLara = this;

            InitializeComponent();

            Constantes.CarregarConstantes();
            Constantes.LogLara.EnfileirarLogExecucao($"Inciando {Constantes.USUARIO_SISTEMA}.");
        }

        private void IniciandoLara(object sender, RoutedEventArgs e)
        {
            using (var db = new DBContexto())
            {
                var pessoa = db.BpmPessoa.FirstOrDefault();
                if (pessoa == null)
                {
                    db.BpmPessoa.Add(new Model.Pessoa
                    {
                        IdPessoa = 1,
                        Nome = "Hudson da Silva Carlos",
                        Apelido = "Dudu",
                        EstadoCivil = Model.EnumEstadoCivil.Casado,
                        Idade = 32,
                        Sexo = Model.EnumSexo.Masculino
                    });
                    db.SaveChanges();
                }

                pessoa = db.BpmPessoa.FirstOrDefault();
                MessageBox.Show($"Nome: {pessoa.Nome}");
            }

            Constantes.LogLara.EnfileirarLogExecucao($"IniciandoLara.");
        }

        public static Dispatcher RetDispatcher()
        {
            return instanciaLara.Dispatcher;
        }

        private void ClickBotao(object sender, RoutedEventArgs e)
        {
            if (sender == btnMaximizar)
            {
                if (WindowState == WindowState.Maximized)
                {
                    //Constantes.LogLara.EnfileirarLogExecucao($"Maximizada.");
                    btnMaximizarIcon.Kind = PackIconKind.WindowMaximize;
                    WindowState = WindowState.Normal;
                    BorderBrush = new SolidColorBrush(Colors.Orange);
                    BorderThickness = new Thickness(1, 1, 1, 1);
                }
                else
                {
                    //Constantes.LogLara.EnfileirarLogExecucao($"Restaurado.");

                    MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
                    MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
                    btnMaximizarIcon.Kind = PackIconKind.WindowRestore;
                    WindowState = WindowState.Maximized;
                    BorderBrush = new SolidColorBrush();
                    BorderThickness = new Thickness(0);
                }
            }
            else if (sender == btnMinimizar)
            {
                WindowState = WindowState.Minimized;
            }
            else if (sender == btnFechar)
            {
                Visibility = Visibility.Visible;
                Constantes.LogLara.Dispose();
                this.Close();
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
