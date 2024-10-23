using System.Windows;
using System.Windows.Forms;
using System.IO;
using Wpf_EntryPoint.ViewModels;

namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for SmsMassivi_RniReduct_UserController.xaml
    /// </summary>
    public partial class SmsMassivi_RniReduct_UserController : Window
    {
        public SmsMassivi_RniReduct_UserController()
        {
            InitializeComponent();

            // Imposta il DataContext al ViewModel
            this.DataContext = new SmsMassivi_RniReduct_ViewModel();

            // Per il debug, controlla se il DataContext è stato impostato
            if (this.DataContext == null)
            {
                System.Windows.MessageBox.Show("DataContext non impostato!");
            }
        }

        // Evento per il bottone Sfoglia
        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Usa OpenFileDialog di Windows Forms
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            // Mostra la finestra di dialogo e, se l'utente seleziona un file, valorizza la TextBox
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePathTextBox.Text = openFileDialog.FileName;
            }
        }

        // Evento per il bottone Elabora
        private void ProcessButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = filePathTextBox.Text;

            // Controllo se il percorso è vuoto
            if (string.IsNullOrEmpty(filePath))
            {
                System.Windows.MessageBox.Show("Seleziona un file Excel prima di procedere.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Controllo se il file ha estensione .xlsx
            if (Path.GetExtension(filePath).ToLower() != ".xlsx")
            {
                System.Windows.MessageBox.Show("Il file deve avere estensione .xlsx.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Logica di elaborazione del file
            System.Windows.MessageBox.Show($"Elaborazione del file {filePath}", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            // Qui puoi aggiungere il codice per processare il file .xlsx
        }
    }

}
