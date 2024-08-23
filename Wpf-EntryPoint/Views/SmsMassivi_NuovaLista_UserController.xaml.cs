using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_EntryPoint.Models;
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

        List<string> _items = new List<string>();

        LookUpCRPS LookUpCRPS { get; set; }

        public SmsMassivi_NuovaLista_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;

            LookUpCRPS readWriteData = new LookUpCRPS();

            LookUpCRPS = readWriteData;

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

        private void AutoCompleteComboBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string filterText = OptionsComboBox.Text + e.Text;
            var filteredItems = _items.Where(item => item.StartsWith(filterText, System.StringComparison.OrdinalIgnoreCase)).ToList();

            OptionsComboBox.ItemsSource = filteredItems;
            OptionsComboBox.IsDropDownOpen = true;
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
                FilePathTextBox.Text = openFileDialog.FileName;  // Mostra il percorso del file selezionato
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
            if (ElaborazioneFile(selectedOption, numericInput, filePath))
            {
                MessageBox.Show("Il file è stato elaborato correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Si è verificato un errore durante l'elaborazione del file. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Funzione xXx che gestisce i dati
        private bool ElaborazioneFile(string campagna, string nomeSerbatoio, string filePath)
        {
            try
            {
                // Ho recuperato i dati del file da processare
                List<TemplateForSms> dataPassage = new List<TemplateForSms>();
                FileInfo fileInfo = new FileInfo(filePath);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    // Ottieni il primo foglio di lavoro
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // Ottieni l'intervallo di celle utilizzate
                    var cellRange = worksheet.Cells[worksheet.Dimension.Address];

                    // Leggi i valori delle celle
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        // Genera nuovo oggetto
                        TemplateForSms entryDataFile = new TemplateForSms
                        {
                            FirstName = worksheet.Cells[row, 3].Text,
                            LastName = worksheet.Cells[row, 4].Text,
                            Attribute3 = worksheet.Cells[row, 24].Text,
                            Attribute4 = worksheet.Cells[row, 25].Text,
                            PhoneNumber = worksheet.Cells[row, 33].Text,
                            OldIdLista = worksheet.Cells[row, 38].Text
                        };

                        dataPassage.Add(entryDataFile);
                    }
                }

                // Recupera i dati della campagna 
                CRPS cRPS = new CRPS();
                LookUpCRPS.lookUpDict.TryGetValue(campagna, out cRPS);

                using (var package = new ExcelPackage())
                {
                    // Aggiungi un foglio di lavoro
                    var worksheet = package.Workbook.Worksheets.Add("da_utilizzare_pulito");

                    #region Intestazione
                    worksheet.Cells[1, 1].Value = "last_name";
                    worksheet.Cells[1, 2].Value = "first_name";
                    worksheet.Cells[1, 3].Value = "gender";
                    worksheet.Cells[1, 4].Value = "address";
                    worksheet.Cells[1, 5].Value = "street_nr";
                    worksheet.Cells[1, 6].Value = "city";
                    worksheet.Cells[1, 7].Value = "postal_code";
                    worksheet.Cells[1, 8].Value = "country";
                    worksheet.Cells[1, 9].Value = "province";
                    worksheet.Cells[1, 10].Value = "phone_number";
                    worksheet.Cells[1, 11].Value = "tax_code";
                    worksheet.Cells[1, 12].Value = "vat";
                    worksheet.Cells[1, 13].Value = "date_of_birth";
                    worksheet.Cells[1, 14].Value = "place_of_birth";
                    worksheet.Cells[1, 15].Value = "phone_number_2";
                    worksheet.Cells[1, 16].Value = "phone_number_3";
                    worksheet.Cells[1, 17].Value = "company_name";
                    worksheet.Cells[1, 18].Value = "email";
                    worksheet.Cells[1, 19].Value = "address_2";
                    worksheet.Cells[1, 20].Value = "address_3";
                    worksheet.Cells[1, 21].Value = "business_category";
                    worksheet.Cells[1, 22].Value = "attribute_1";
                    worksheet.Cells[1, 23].Value = "attribute_2";
                    worksheet.Cells[1, 24].Value = "attribute_3";
                    worksheet.Cells[1, 25].Value = "attribute_4";
                    worksheet.Cells[1, 26].Value = "attribute_5";
                    worksheet.Cells[1, 27].Value = "attribute_6";
                    worksheet.Cells[1, 28].Value = "attribute_7";
                    worksheet.Cells[1, 29].Value = "attribute_8";
                    worksheet.Cells[1, 30].Value = "attribute_9";
                    worksheet.Cells[1, 31].Value = "attribute_10";
                    worksheet.Cells[1, 32].Value = "provider";
                    worksheet.Cells[1, 33].Value = "supplier";
                    worksheet.Cells[1, 34].Value = "expiration_datetime";
                    worksheet.Cells[1, 35].Value = "data_scadenza_privacy";
                    worksheet.Cells[1, 36].Value = "cpl";
                    worksheet.Cells[1, 37].Value = "nome_serbatoio";
                    worksheet.Cells[1, 38].Value = "old_id_lista";
                    worksheet.Cells[1, 39].Value = "stato_utilizzo";
                    worksheet.Cells[1, 40].Value = "data_utilizzo";
                    worksheet.Cells[1, 41].Value = "campagna";
                    worksheet.Cells[1, 42].Value = "lista";
                    #endregion

                    int row = 2;
                    foreach (var data in dataPassage)
                    {
                        worksheet.Cells[row, 1].Value = data.LastName;
                        worksheet.Cells[row, 2].Value = data.FirstName;
                        worksheet.Cells[row, 10].Value = data.PhoneNumber;
                        worksheet.Cells[row, 24].Value = data.Attribute3;
                        worksheet.Cells[row, 25].Value = data.Attribute4;
                        worksheet.Cells[row, 32].Value = cRPS.Provider;
                        worksheet.Cells[row, 33].Value = cRPS.Supplier;
                        worksheet.Cells[row, 37].Value = nomeSerbatoio;
                        worksheet.Cells[row, 38].Value = data.OldIdLista;
                        worksheet.Cells[row, 41].Value = campagna;
                        row++;
                    }

                    // Recupero la cartella di output
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "DataFile", "Output");

                    // Prefisso da rimuovere dal nome del file
                    var prefix = "ExportData_";

                    // Recupera tutti i file .xlsx nella directory specificata
                    var files = Directory.GetFiles(directoryPath, "*.xlsx");

                    // Trova il valore numerico più alto dai nomi dei file
                    var maxNumber = files
                        .Select(file => Path.GetFileNameWithoutExtension(file)) // Ottieni solo il nome del file senza estensione
                        .Select(name => name.StartsWith(prefix) ? name.Substring(prefix.Length) : null) // Rimuovi il prefisso
                        .Where(name => !string.IsNullOrEmpty(name) && int.TryParse(name, out _)) // Filtra solo i nomi validi
                        .Select(name => int.Parse(name)) // Converte i nomi in numeri
                        .DefaultIfEmpty(0) // Imposta 0 se non ci sono file validi
                        .Max(); // Trova il valore massimo

                    // Incrementa il valore massimo di 1
                    var newNumber = maxNumber + 1;

                    // Salva il file Excel
                    FileInfo fileInfoOutput = new FileInfo(Path.Combine(directoryPath, $"ExportData_{newNumber}.xlsx"));
                    package.SaveAs(fileInfoOutput);
                }

                // Se tutto è andato bene, ritorna true
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
