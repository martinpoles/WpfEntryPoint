using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.Models;
using Wpf_EntryPoint.Utility;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.ComponentModel;

namespace Wpf_EntryPoint.Views
{
    public partial class SmsMassivi_Rni_UserController : UserControl
    {
        private List<string> availableCampaigns; // Changed naming for clarity
        private List<string> allCampaigns;

        private Dictionary<string, List<string>> availableLists; // Changed naming for clarity
        private Dictionary<string, List<string>> allLists;

        private Dictionary<string, CRPS> cRPs;

        public SmsMassivi_Rni_UserController()
        {
            InitializeComponent();

            cRPs = CreaDictSuplierProvider();

            // Initialize campaign and list data from the database
            availableCampaigns = GetCampaignsFromDB();
            allCampaigns = availableCampaigns;

            availableLists = GetListsFromDB();
            allLists = availableLists;

            // Update the initial counts
            UpdateCampaignCount();
            UpdateListCount();

            // Set the data sources for the ListBox
            suggestionListBox1.ItemsSource = availableCampaigns;
            suggestionListBox2.ItemsSource = availableLists.SelectMany(kvp => kvp.Value).ToList(); // Flatten the dictionary for display
        }

        private void SearchTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox1.Text.ToUpper();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                popup1.IsOpen = false;
                return;
            }

            var filteredSuggestions = allCampaigns.Where(s => s.ToUpper().Contains(searchText)).ToList();
            suggestionListBox1.ItemsSource = filteredSuggestions;

            popup1.IsOpen = filteredSuggestions.Any();
            UpdateCampaignCount(); // Update count after filtering
        }

        private void SearchTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox2.Text.ToUpper();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                popup2.IsOpen = false;
                return;
            }

            var selectedCampaigns = GetSelectedCampaings();

            var filteredLists = selectedCampaigns
                .Where(campaign => availableLists.ContainsKey(campaign))
                .SelectMany(campaign => availableLists[campaign])
                .ToList();

            var filteredSuggestions = filteredLists
                .Where(s => s.ToUpper().Contains(searchText))
                .ToList();

            suggestionListBox2.ItemsSource = filteredSuggestions;
            popup2.IsOpen = filteredSuggestions.Any();
            UpdateListCount(); // Update count after filtering
        }

        private void SuggestionListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionListBox1.SelectedItem != null)
            {
                string selectedItem = suggestionListBox1.SelectedItem.ToString();

                if (selectedItemsPanel1.Children.OfType<StackPanel>().Any(p => ((TextBlock)p.Children[0]).Text == selectedItem))
                {
                    MessageBox.Show("Questo elemento è già stato selezionato.");
                    suggestionListBox1.SelectedItem = null; // Reset della selezione
                    return;
                }

                availableCampaigns.Remove(selectedItem);
                SearchTextBox1.Text = string.Empty;
                popup1.IsOpen = false;

                AddItemToPanel(selectedItem, selectedItemsPanel1, availableCampaigns, allCampaigns);
                suggestionListBox1.SelectedItem = null; // Reset della selezione

                UpdateCampaignCount(); // Aggiorna il conteggio delle campagne dopo la selezione
                UpdateListCount(); // Aggiorna il conteggio delle liste in base alle campagne selezionate
            }
        }

        private void SuggestionListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionListBox2.SelectedItem != null)
            {
                string selectedItem = suggestionListBox2.SelectedItem.ToString();

                if (selectedItemsPanel2.Children.OfType<StackPanel>().Any(p => ((TextBlock)p.Children[0]).Text == selectedItem))
                {
                    MessageBox.Show("Questo elemento è già stato selezionato.");
                    suggestionListBox2.SelectedItem = null; // Reset della selezione
                    return;
                }

                string associatedCampaign = availableLists.FirstOrDefault(kvp => kvp.Value.Contains(selectedItem)).Key;

                if (associatedCampaign == null)
                {
                    MessageBox.Show("Errore: campagna non trovata per la lista selezionata.");
                    suggestionListBox2.SelectedItem = null;
                    return;
                }

                availableLists[associatedCampaign].Remove(selectedItem);
                SearchTextBox2.Text = string.Empty;
                popup2.IsOpen = false;

                AddItemToPanel(selectedItem, selectedItemsPanel2, availableLists.SelectMany(kvp => kvp.Value).ToList(), allLists.SelectMany(kvp => kvp.Value).ToList());
                suggestionListBox2.SelectedItem = null; // Reset della selezione

                UpdateListCount(); // Update count after selection
            }
        }

        private void AddItemToPanel(string selectedItem, StackPanel panel, List<string> availableSuggestions, List<string> allSuggestions)
        {
            StackPanel itemPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            TextBlock textBlock = new TextBlock
            {
                Text = selectedItem,
                VerticalAlignment = VerticalAlignment.Center
            };

            Button removeButton = new Button
            {
                Content = "Rimuovi",
                Tag = selectedItem,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            removeButton.Click += (s, e) =>
            {
                panel.Children.Remove(itemPanel);
                availableSuggestions.Add(selectedItem);

                suggestionListBox1.ItemsSource = null;
                suggestionListBox1.ItemsSource = availableCampaigns;

                suggestionListBox2.ItemsSource = null;
                suggestionListBox2.ItemsSource = availableLists.SelectMany(kvp => kvp.Value).ToList(); // Update with available lists
                UpdateListCount(); // Update count after removing
            };

            itemPanel.Children.Add(textBlock);
            itemPanel.Children.Add(removeButton);
            panel.Children.Add(itemPanel);
        }

        private void SearchTextBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchTextBox1.Text))
            {
                popup1.IsOpen = true;
            }
        }

        private void SearchTextBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            popup1.IsOpen = false;
        }

        private void SearchTextBox2_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SearchTextBox2.Text))
            {
                popup2.IsOpen = true;
            }
        }

        private void SearchTextBox2_LostFocus(object sender, RoutedEventArgs e)
        {
            popup2.IsOpen = false;
        }
        private void UpdateCampaignCount()
        {
            CampaignCountLabel.Content = $"{availableCampaigns.Count} Campagne disponibili";
        }

        private void UpdateListCount()
        {
            // Ottieni le campagne selezionate
            var selectedCampaings = GetSelectedCampaings();

            // Se non ci sono campagne selezionate, mostra tutte le liste
            if (selectedCampaings.Count == 0)
            {
                // Somma il numero totale di liste disponibili nel dizionario
                int listCount = availableLists.Values.Sum(lists => lists.Count);
                ListCountLabel.Content = $"{listCount} Liste disponibili";
            }
            else
            {
                // Conta le liste solo per le campagne selezionate
                int listCount = selectedCampaings
                    .Where(campaign => availableLists.ContainsKey(campaign))
                    .Sum(campaign => availableLists[campaign].Count);

                ListCountLabel.Content = $"{listCount} Liste disponibili per {selectedCampaings.Count} campagne selezionate";
            }
        }
        private List<string> GetSelectedCampaings()
        {
            return selectedItemsPanel1.Children.OfType<StackPanel>()
                .Select(p => ((TextBlock)p.Children[0]).Text)
                .ToList();
        }

        private List<string> GetCampaignsFromDB()
        {
            List<string> campaigns = new List<string>();
            string query = @"select distinct campaignId from campaigns";

            using (MySqlCommand command = new MySqlCommand(query, DatabaseManager.Instance.Connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        campaigns.Add(reader["campaignId"].ToString());
                    }
                }
            }
            return campaigns;
        }

        private Dictionary<string, List<string>> GetListsFromDB()
        {
            Dictionary<string, List<string>> lists = new Dictionary<string, List<string>>();
            string query = @"select list_name, campaign_id from lists";

            using (MySqlCommand command = new MySqlCommand(query, DatabaseManager.Instance.Connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var campaignId = reader["campaign_id"].ToString();
                        var listName = reader["list_name"].ToString();

                        if (lists.ContainsKey(campaignId))
                        {
                            lists[campaignId].Add(listName);
                        }
                        else
                        {
                            lists[campaignId] = new List<string> { listName };
                        }
                    }
                }
            }
            return lists;
        }
        private void Elabora_Click(object sender, RoutedEventArgs e)
        {
            // Recupera le date selezionate dai DatePicker
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            // Verifica se le date sono state selezionate
            if (startDate == null || endDate == null)
            {
                // Mostra un messaggio di errore se qualche data non è selezionata
                MessageBox.Show("Tutte le date devono essere selezionate.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Converti le date in stringa per ulteriori elaborazioni se necessario
            string startDateString = startDate.Value.ToString("yyyy-MM-dd");
            string endDateString = endDate.Value.ToString("yyyy-MM-dd");

            // Recupera i valori dai pannelli
            var campagneSelezionate = new List<string>();
            var listeSelezionate = new List<string>();

            // Recupera i valori dal primo pannello
            foreach (Panel child in selectedItemsPanel1.Children)
            {
                foreach (var item in child.Children)
                {
                    if (item is TextBlock textBox)
                    {
                        campagneSelezionate.Add(textBox.Text);
                    }
                }
            }

            // Recupera i valori dal secondo pannello
            foreach (Panel child in selectedItemsPanel2.Children)
            {
                foreach (var item in child.Children)
                {
                    if (item is TextBlock textBox)
                    {
                        listeSelezionate.Add(textBox.Text);
                    }
                }
            }

            // Esegui l'elaborazione con i valori raccolti
            if (ElaborazioneFile(startDateString, endDateString, campagneSelezionate, listeSelezionate))
            {
                MessageBox.Show("L'elaborazione è stata completata correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Si è verificato un errore durante l'elaborazione. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ElaborazioneFile(string startDate, string endDate, List<string> campagneSelezionate, List<string> listeSelezionate)
        {
            try
            {
                // Assicurati che le date siano nel formato corretto
                if (string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(endDate) || campagneSelezionate.Count < 1 || listeSelezionate.Count < 1)
                {
                    throw new ArgumentException("Dati in ingresso non conformi");
                }

                // Verifica che le date siano in formato valido
                if (!DateTime.TryParse(startDate, out DateTime startDateTime) ||
                    !DateTime.TryParse(endDate, out DateTime endDateTime))
                {
                    throw new ArgumentException("Le date devono essere nel formato corretto.");
                }

                // Trasformiamo le campagne e le liste selezionate in formato corretto per la query
                string campagneParam = string.Join(",", campagneSelezionate.Select(c => $"'{c}'"));
                string listeParam = string.Join(",", listeSelezionate.Select(l => $"'{l}'"));

                // Imposta l'ora del primo giorno a 00:00:01
                DateTime startOfDayDateTime = startDateTime.Date.AddSeconds(1); // 00:00:01

                // Imposta l'ora dell'ultimo giorno a 23:59:59
                DateTime endOfDayDateTime = endDateTime.Date.AddDays(1).AddSeconds(-1); // 23:59:59

                // Crea la query SQL con i valori inseriti
                string queryConParametri = $@"SELECT DISTINCT
                            contacts.first_name, 
                            contacts.last_name, 
                            contacts.phone_number, 
                            sms_messages.campaign_id, 
                            COLUMN_GET(dynamic_cols, 'old_id_lista' AS CHAR(100)) AS old_id_lista
                        FROM 
                            sms_messages
                        INNER JOIN contacts ON sms_messages.contact_id = contacts.contact_id
                        INNER JOIN lists on sms_messages.campaign_id = lists.campaign_id 
                        WHERE sms_messages.sent_datetime BETWEEN '{startOfDayDateTime:yyyy-MM-dd HH:mm:ss}' AND '{endOfDayDateTime:yyyy-MM-dd HH:mm:ss}'
                        AND sms_messages.campaign_id IN ({campagneParam}) 
                        AND lists.list_name IN ({listeParam})";

                // Opzionalmente puoi loggare la query per verificare che sia corretta
                Console.WriteLine($"Query SQL completa: {queryConParametri}");

                #region Esegui la query
                List<ModelRniVolpi> THEDUMPTRUCK = new List<ModelRniVolpi>();
                Dictionary<string, ModelRniVolpi> test = new Dictionary<string, ModelRniVolpi>();

                using (MySqlCommand command = new MySqlCommand(queryConParametri, DatabaseManager.Instance.Connection))
                {
                    command.CommandTimeout = 300; // Aumenta il timeout a 300 secondi, ad esempio
                    // Esegui la query
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Elabora i risultati
                        while (reader.Read())
                        {
                            // Recupero il valore della campagna dal reader
                            string campagna = reader["campaign_id"].ToString();

                            var crpsRecord = new CRPS();
                            var supplier = "";
                            var provider = "";
                            var campagnarni = "";

                            // Cerca il valore nel dizionario cRPs
                            if (cRPs.TryGetValue(campagna, out crpsRecord))
                            {
                                supplier = crpsRecord.Supplier;
                                provider = crpsRecord.Provider;
                                campagnarni = crpsRecord.RNI;
                            }

                            // Crea un nuovo record di ModelRniVolpi
                            var record = new ModelRniVolpi
                            {
                                first_name = reader["first_name"].ToString(),
                                last_name = reader["last_name"].ToString(),
                                phone_number = reader["phone_number"].ToString(),
                                campagna = campagnarni,
                                old_id_lista = reader["old_id_lista"].ToString(),
                                supplier = supplier,  // Valorizza supplier dal dizionario
                                provider = provider    // Valorizza provider dal dizionario
                            };

                            // Aggiungi il record alla lista
                            THEDUMPTRUCK.Add(record);

                            // Aggiungi il record anche al dizionario 'test', utilizzando 'campagna' come chiave
                            if (!test.ContainsKey(campagna))
                            {
                                test.Add(campagna, record);
                            }
                            else
                            {
                                // Se necessario, gestisci i casi in cui la chiave esiste già (opzionale)
                                test[campagna] = record; // Sovrascrivi l'elemento esistente
                            }
                        }
                    }
                }
                #endregion


                #region Scrivi
                string NomeFile = "MEGASERBATOIO AC RNI " + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                string PathFile = @"C:\Users\ssoko\Desktop\Nuova cartella";
                //string PathFile = "I:\\GESTIONE SERBATOI NEW\\";//prod
                CreaFileTesto((PathFile + NomeFile), THEDUMPTRUCK);
                #endregion

            }
            catch (Exception ex)
            {
                // Log dell'eccezione e restituzione di false
                MessageBox.Show($"Errore durante l'elaborazione del file: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true; // Restituisci true se tutto è andato a buon fine
        }


        private void CreaFileTesto(string filePath, List<ModelRniVolpi> dataList)
        {
            // Definisci i nomi delle colonne (headers)
            string[] headers = new string[]
    {
        "last_name", "first_name", "gender", "address", "street_nr", "city", "postal_code", "country",
        "province", "phone_number", "tax_code", "vat", "date_of_birth", "place_of_birth", "phone_number_2",
        "phone_number_3", "company_name", "email", "address_2", "address_3", "business_category", "attribute_1",
        "attribute_2", "attribute_3", "attribute_4", "attribute_5", "attribute_6", "attribute_7", "attribute_8",
        "attribute_9", "attribute_10", "provider", "supplier", "expiration_datetime", "data_scadenza_privacy",
        "cpl", "nome_serbatoio", "old_id_lista", "stato_utilizzo", "data_utilizzo", "campagna", "lista"
    };

            // Utilizza StreamWriter che creerà il file se non esiste o lo sovrascriverà se esiste
            using (StreamWriter writer = new StreamWriter(filePath, false)) // 'false' per sovrascrivere se esiste
            {
                // Scrivi la prima riga con gli headers, separati da tabulazioni
                writer.WriteLine(string.Join("\t", headers));

                // Cicla sui dati e scrivi ogni riga successiva
                foreach (var data in dataList)
                {
                    // Crea una lista di valori che verranno scritti in ogni riga
                    List<string> rowValues = new List<string>
                    {
                        data.last_name,
                        data.first_name,
                        "",  // Gender (aggiungi i tuoi valori)
                        "",  // Address
                        "",  // Street number
                        "",  // City
                        "",  // Postal code
                        "",  // Country
                        "",  // Province
                        data.phone_number,
                        "",  // Tax code
                        "",  // VAT
                        "",  // Date of birth
                        "",  // Place of birth
                        "",  // Phone number 2
                        "",  // Phone number 3
                        "",  // Company name
                        "",  // Email
                        "",  // Address 2
                        "",  // Address 3
                        "",  // Business category
                        "",  // Attribute_1
                        "",  // Attribute_2
                        "",  // Attribute_3
                        "",  // Attribute_4
                        "",  // Attribute_5
                        "",  // Attribute_6
                        "",  // Attribute_7
                        "",  // Attribute_8
                        "",  // Attribute_9
                        "",  // Attribute_10
                        data.provider,
                        data.supplier,
                        "",  // Expiration datetime
                        "",  // Data scadenza privacy
                        "",  // CPL
                        "",  // Nome serbatoio
                        data.old_id_lista,
                        "",  // Stato utilizzo
                        "",  // Data utilizzo
                        data.campagna,
                        ""   // Lista
                    };

                    // Scrivi la riga nel file di testo, separando i valori con tabulazioni
                    writer.WriteLine(string.Join("\t", rowValues));
                }
            }
        }

        public Dictionary<string, CRPS> CreaDictSuplierProvider()
        {
            string solutionPath = @"C:\Users\ssoko\Desktop\Personal\Code\Wpf-EntryPoint\Wpf-EntryPoint.sln";
            string solutionDirectory = Path.GetDirectoryName(solutionPath);

            // Costruisci il percorso relativo
            string relativePath = @"Wpf-EntryPoint\bin\Debug\net8.0-windows\DataFile\Define\CRPS.xlsx";

            // Combina i percorsi
            string path = Path.Combine(solutionDirectory, relativePath);

            var records = new Dictionary<string, CRPS>();
            string lastRNI = null;       // Per memorizzare l'ultimo valore di RNI
            string lastProvider = null;  // Per memorizzare l'ultimo valore di Provider
            string lastSupplier = null;  // Per memorizzare l'ultimo valore di Supplier

            // Assicurati che il file esista
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Il file {path} non è stato trovato.");
            }

            // Inizializza il pacchetto Excel
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                // Assumiamo che il primo foglio contenga i dati
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                // Iniziamo dalla riga 2 per saltare l'intestazione
                for (int row = 2; row <= rowCount; row++)
                {
                    // Leggi i valori dalle colonne, gestendo celle unite
                    string nuovoRiutilizzo = worksheet.Cells[row, 1].Text; // Colonna A
                    string rni = GetCellValue(worksheet.Cells[row, 2]);      // Colonna B
                    string provider = GetCellValue(worksheet.Cells[row, 3]); // Colonna C
                    string supplier = GetCellValue(worksheet.Cells[row, 4]); // Colonna D

                    // Ignora il record se NuovoRiutilizzo è vuoto
                    if (string.IsNullOrWhiteSpace(nuovoRiutilizzo))
                    {
                        continue; // Salta questo ciclo se NuovoRiutilizzo è vuoto
                    }

                    // Se B, C e D sono vuoti e A è valorizzato, utilizziamo i valori precedentemente salvati
                    if (string.IsNullOrWhiteSpace(rni) && string.IsNullOrWhiteSpace(provider) && string.IsNullOrWhiteSpace(supplier))
                    {
                        // Se abbiamo già salvato valori precedenti, usali
                        rni = lastRNI;
                        provider = lastProvider;
                        supplier = lastSupplier;
                    }
                    else
                    {
                        // Altrimenti, salva i nuovi valori
                        lastRNI = rni;
                        lastProvider = provider;
                        lastSupplier = supplier;
                    }

                    // Crea il nuovo oggetto CRPS
                    var crps = new CRPS
                    {
                        NuovoRiutilizzo = nuovoRiutilizzo,
                        RNI = rni,
                        Provider = provider,
                        Supplier = supplier
                    };

                    // Aggiungi il record al dizionario, utilizzando NuovoRiutilizzo come chiave
                    if (!records.ContainsKey(nuovoRiutilizzo))
                    {
                        records.Add(nuovoRiutilizzo, crps);
                    }
                }
            }

            return records;
        }

        private string GetCellValue(ExcelRange cell)
        {
            // Se la cella è parte di un intervallo unito, restituisci il valore della prima cella dell'unione
            if (cell.Merge)
            {
                return cell.Worksheet.Cells[cell.Start.Row, cell.Start.Column].Text;
            }
            return cell.Text; // Altrimenti restituisci il valore normale
        }

    }
}
