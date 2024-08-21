using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf_EntryPoint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClick_NuovaLista(object sender, RoutedEventArgs e)
        {
            // Azione da eseguire quando il pulsante viene cliccato
            MessageBox.Show("Pulsante Nuova cliccato!");
        }

        private void OnClick_NRI(object sender, RoutedEventArgs e)
        {
            // Azione da eseguire quando il pulsante viene cliccato
            MessageBox.Show("Pulsante NRI cliccato!");
        }
    }
}