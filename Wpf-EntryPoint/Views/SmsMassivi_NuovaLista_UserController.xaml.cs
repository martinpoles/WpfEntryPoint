using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_EntryPoint.Utility;
using Wpf_EntryPoint.ViewModels;
using Wpf_EntryPoint.Windows;
namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for SmsMassivi_NuovaLista_UserController.xaml
    /// </summary>
    public partial class SmsMassivi_NuovaLista_UserController : UserControl
    {
        private CheckBox _currentlyChecked;

        public SmsMassivi_NuovaLista_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;

           ReadWriteData readWriteData = new ReadWriteData();

            // Genera una CheckBox per ogni stringa nella lista
            foreach (var item in readWriteData.lookUpDict)
            {
                OptionsComboBox.Items.Add(item.Key);
            }

        }
        private void NumericInputField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // accetta solo numeri (0-9)
            e.Handled = regex.IsMatch(e.Text); // se l'input non è numerico, lo blocca
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void OpenInsertNewCampagnaWindow_Click(object sender, RoutedEventArgs e)
        {
            InsertNewCampagna newWindow = new InsertNewCampagna();
            newWindow.ShowDialog();
        }

        private void Elabora_Click(object sender, RoutedEventArgs e)
        {
            // Ottieni i valori dei campi di input
            string selectedOption = OptionsComboBox.SelectedItem?.ToString();
            string numericInput = NumericInputField.Text;
            string filePath = FilePathTextBox.Text;

            // Verifica se tutti i campi sono valorizzati
            if (string.IsNullOrEmpty(selectedOption) ||
                string.IsNullOrWhiteSpace(numericInput) ||
                string.IsNullOrWhiteSpace(filePath))
            {
                // Mostra un messaggio di errore se qualche campo è vuoto
                MessageBox.Show("Tutti i campi devono essere compilati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Chiama la funzione xXx con i dati dei campi
            ElaborazioneFile(selectedOption, numericInput, filePath);
        }

        // Funzione xXx che gestisce i dati
        private void ElaborazioneFile(string selectedOption, string numericInput, string filePath)
        {



            // Logica per gestire i dati
            MessageBox.Show($"Funzione xXx chiamata con:\n" +
                            $"Opzione Selezionata: {selectedOption}\n" +
                            $"Input Numerico: {numericInput}\n" +
                            $"Percorso File: {filePath}", "Funzione xXx");
        }

    }
}
