using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Wpf_EntryPoint.Models;
using Wpf_EntryPoint.Utility;

namespace Wpf_EntryPoint.Views
{

    public partial class PuliziaNumeri_39_UserController : System.Windows.Controls.UserControl
    {
        public PuliziaNumeri_39_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? System.Windows.Application.Current.MainWindow?.DataContext;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Usa VistaFolderBrowserDialog per selezionare una cartella
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
            {
                Description = "Seleziona una cartella",  // Descrizione per il dialogo
                UseDescriptionForTitle = true            // Usa la descrizione anche come titolo (solo per Windows Vista e versioni successive)
            };

            // Mostra il dialogo di selezione cartella
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                FolderPath.Text = dialog.SelectedPath;  // Mostra il percorso della cartella selezionata
            }
        }
        private void Elabora_Click(object sender, RoutedEventArgs e)
        {
            //// Ottieni i valori dei campi di input
            string folderPath = FolderPath.Text;


            //// Verifica se tutti i campi sono valorizzati
            if (string.IsNullOrEmpty(folderPath))
            {
                // Mostra un messaggio di errore se qualche campo è vuoto
                System.Windows.MessageBox.Show("Tutti i campi devono essere compilati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ElaborazioneFile(folderPath))
            {
                System.Windows.MessageBox.Show("Il file è stato elaborato correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Si è verificato un errore durante l'elaborazione del file. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Funzione xXx che gestisce i dati
        private bool ElaborazioneFile(string folderPath)
        {
            try
            {
                // Ottiene tutti i file .csv nel percorso specificato e nelle sottocartelle
                string[] csvFiles = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);

                foreach (var csvFile in csvFiles)
                {
                    // Ho recuperato i dati del file da processare
                    List<string> dataPassage = new List<string>();
                    FileInfo fileInfo = new FileInfo(csvFile);

                    var csvData = new Dictionary<string, string>();

                    GFYF gFYF = new GFYF();

                    gFYF.fuckfuckfuck(csvFile);

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

            return false;

        }
    }
}
