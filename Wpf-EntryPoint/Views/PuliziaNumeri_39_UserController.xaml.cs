using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.Models;
using Wpf_EntryPoint.Utility;

namespace Wpf_EntryPoint.Views
{

    public partial class PuliziaNumeri_39_UserController : UserControl
    {
        public PuliziaNumeri_39_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",  // Filtro per file .xlsx
                Title = "Seleziona un file Excel"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FolderPath.Text = openFileDialog.FileName;  // Mostra il percorso del file selezionato
            }

        }
        private void Elabora_Click(object sender, RoutedEventArgs e)
        {
            //// Ottieni i valori dei campi di input
            //string selectedOption = OptionsComboBox.SelectedItem?.ToString();
            //string numericInput = NumericInputField.Text;
            //string filePath = FilePathTextBox.Text;

            //// Verifica se tutti i campi sono valorizzati
            //if (string.IsNullOrEmpty(selectedOption) ||
            //    string.IsNullOrWhiteSpace(numericInput) ||
            //    string.IsNullOrWhiteSpace(filePath))
            //{
            //    // Mostra un messaggio di errore se qualche campo è vuoto
            //    MessageBox.Show("Tutti i campi devono essere compilati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //if (ElaborazioneFile(selectedOption, numericInput, filePath))
            //{
            //    MessageBox.Show("Il file è stato elaborato correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    MessageBox.Show("Si è verificato un errore durante l'elaborazione del file. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        // Funzione xXx che gestisce i dati
        private bool ElaborazioneFile(string campagna, string nomeSerbatoio, string filePath)
        {
            try
            {
             
                return true;
            }
            catch (Exception ex)
            {
                // Ritorna false in caso di eccezione
                return false;
            }
        }
    }
}
