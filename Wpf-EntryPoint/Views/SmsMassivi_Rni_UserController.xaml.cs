using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.Models;
using Wpf_EntryPoint.Utility;
using Wpf_EntryPoint.ViewModels;

namespace Wpf_EntryPoint.Views
{
    /// <summary>
    /// Interaction logic for SmsMassivi_Rni_UserController.xaml
    /// </summary>
    public partial class SmsMassivi_Rni_UserController : UserControl
    {
        List<string> _items = new List<string>();

        LookUpCRPS LookUpCRPS { get; set; }
        public SmsMassivi_Rni_UserController()
        {
            InitializeComponent();
            this.DataContext = this.DataContext ?? Application.Current.MainWindow?.DataContext;

            LookUpCRPS readWriteData = new LookUpCRPS();

            LookUpCRPS = readWriteData;

        }

        private void BrowseButton1_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",  // Filtro per file .xlsx
                Title = "Seleziona un file Excel"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                File1InputField1.Text = openFileDialog.FileName;  // Mostra il percorso del file selezionato
            }

        }

        private void BrowseButton2_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",  // Filtro per file .xlsx
                Title = "Seleziona un file Excel"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                File1InputField2.Text = openFileDialog.FileName;  // Mostra il percorso del file selezionato
            }

        }

        private void Elabora_Click(object sender, RoutedEventArgs e)
        {

            string filePath1 = File1InputField1.Text;
            string filePath2 = File1InputField2.Text;

            // Verifica se tutti i campi sono valorizzati
            if (
                string.IsNullOrWhiteSpace(filePath1) ||
                string.IsNullOrWhiteSpace(filePath2) 
                )
            {
                // Mostra un messaggio di errore se qualche campo è vuoto
                MessageBox.Show("Tutti i campi devono essere compilati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ElaborazioneFile(filePath1, filePath2))
            {
                MessageBox.Show("Il file è stato elaborato correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Si è verificato un errore durante l'elaborazione del file. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Funzione xXx che gestisce i dati
        private bool ElaborazioneFile(string filePath1, string filePath2)
        {
            try
            {
                List<TemplateForSmsRNI> templateForSmsRNIs = new List<TemplateForSmsRNI>();
                List<string> numeriTelefono = new List<string>();

                #region Import Primo File
                //Inizio import
                FileInfo fileInfoPrimo = new FileInfo(filePath1);
                using (ExcelPackage package = new ExcelPackage(fileInfoPrimo))
                {
                    // Ottieni il primo foglio di lavoro
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    // Ottieni l'intervallo di celle utilizzate
                    var cellRange = worksheet.Cells[worksheet.Dimension.Address];

                    // Leggi i valori delle celle
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        //genero nuovo oggetto
                        TemplateForSmsRNI entryDataFile = new TemplateForSmsRNI();

                        entryDataFile.IdCampagna = worksheet.Cells[row, 2].Text;
                        entryDataFile.Nome = worksheet.Cells[row, 5].Text;
                        entryDataFile.Cognome = worksheet.Cells[row, 6].Text;
                        entryDataFile.NrTelefono = worksheet.Cells[row, 7].Text;
                        entryDataFile.Stato = worksheet.Cells[row, 9].Text;
                        entryDataFile.Idirizzo = worksheet.Cells[row, 10].Text;
                        entryDataFile.Cap = worksheet.Cells[row, 11].Text;
                        entryDataFile.Citta = worksheet.Cells[row, 12].Text;
                        entryDataFile.Provincia = worksheet.Cells[row, 13].Text;
                        entryDataFile.Regione = worksheet.Cells[row, 14].Text;
                        entryDataFile.NomeSerbatoio = worksheet.Cells[row, 26].Text;
                        entryDataFile.OldIdLista = worksheet.Cells[row, 27].Text;

                        templateForSmsRNIs.Add(entryDataFile);
                    }
                }
                #endregion

                #region Import Secondo File
                FileInfo fileInfoSecondo = new FileInfo(filePath2);
                using (ExcelPackage package = new ExcelPackage(fileInfoSecondo))
                {
                    // Ottieni il primo foglio di lavoro
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                    // Ottieni l'intervallo di celle utilizzate
                    var cellRange = worksheet.Cells[worksheet.Dimension.Address];

                    // Leggi i valori delle celle
                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var NumeroTelefono = worksheet.Cells[row, 2].Text;

                        numeriTelefono.Add(NumeroTelefono);
                    }
                }
                #endregion

                #region Esclusioni di file con match e creazione outputfile
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

                    int j = 2;

                    Dictionary<string, List<string>> qwe = new Dictionary<string, List<string>>();
                    for (int i = 0; i < templateForSmsRNIs.Count; i++)
                    {
                        if (!numeriTelefono.Contains(templateForSmsRNIs[i].NrTelefono) && templateForSmsRNIs[i].Stato == "SENT")
                        {
                            CRPS cRPS = new CRPS();
                            LookUpCRPS.lookUpDict.TryGetValue(templateForSmsRNIs[i].IdCampagna, out cRPS);

                            if (cRPS == null)
                            {
                                continue;
                            }

                            worksheet.Cells[j, 1].Value = templateForSmsRNIs[i].Cognome;
                            worksheet.Cells[j, 2].Value = templateForSmsRNIs[i].Nome;
                            worksheet.Cells[j, 10].Value = templateForSmsRNIs[i].NrTelefono;

                            worksheet.Cells[j, 32].Value = cRPS.Provider;//af provider
                            worksheet.Cells[j, 33].Value = cRPS.Supplier;//ag supplier
                            worksheet.Cells[j, 37].Value = templateForSmsRNIs[i].NomeSerbatoio;//al old id lista
                            worksheet.Cells[j, 38].Value = templateForSmsRNIs[i].OldIdLista;//al old id lista
                            worksheet.Cells[j, 41].Value = cRPS.RNI;//ao campagna
                            j++;
                        }
                    }

                    // Recupero la cartella di output
                    var directoryPath = Directory.GetCurrentDirectory() + "\\DataFile\\OutData\\SMS\\RNI";

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
                #endregion
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

    }
}
