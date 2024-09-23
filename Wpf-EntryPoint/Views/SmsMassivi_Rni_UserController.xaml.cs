using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wpf_EntryPoint.Models;
using Wpf_EntryPoint.Utility;
using Wpf_EntryPoint;

namespace Wpf_EntryPoint.Views
{
    public partial class SmsMassivi_Rni_UserController : UserControl
    {
        // Liste di suggerimenti
        private List<string> availableSuggestions1 = new List<string> { "Suggerimento 1", "Suggerimento 2", "Suggerimento 3" };
        private List<string> allSuggestions1 = new List<string> { "Suggerimento 1", "Suggerimento 2", "Suggerimento 3" };

        private List<string> availableSuggestions2 = new List<string> { "Opzione A", "Opzione B", "Opzione C" };
        private List<string> allSuggestions2 = new List<string> { "Opzione A", "Opzione B", "Opzione C" };

        public SmsMassivi_Rni_UserController()
        {
            InitializeComponent();

            // Imposta le sorgenti dati per i ListBox
            suggestionListBox1.ItemsSource = availableSuggestions1;
            suggestionListBox2.ItemsSource = availableSuggestions2;
        }

        // Metodo per filtrare i suggerimenti in base al testo inserito per TextBox 1
        private void SearchTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox1.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                popup1.IsOpen = false;
                return;
            }

            var filteredSuggestions = allSuggestions1.Where(s => s.ToLower().Contains(searchText)).ToList();
            suggestionListBox1.ItemsSource = filteredSuggestions;

            popup1.IsOpen = filteredSuggestions.Any();
        }

        // Metodo per filtrare i suggerimenti in base al testo inserito per TextBox 2
        private void SearchTextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox2.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                popup2.IsOpen = false;
                return;
            }

            var filteredSuggestions = allSuggestions2.Where(s => s.ToLower().Contains(searchText)).ToList();
            suggestionListBox2.ItemsSource = filteredSuggestions;

            popup2.IsOpen = filteredSuggestions.Any();
        }

        // Evento di selezione per ListBox 1
        private void SuggestionListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionListBox1.SelectedItem != null)
            {
                string selectedItem = suggestionListBox1.SelectedItem.ToString();

                // Verifica se l'elemento è già presente nel pannello
                if (selectedItemsPanel1.Children.OfType<StackPanel>().Any(p => ((TextBlock)p.Children[0]).Text == selectedItem))
                {
                    MessageBox.Show("Questo elemento è già stato selezionato.");
                    suggestionListBox1.SelectedItem = null; // Reset della selezione
                    return;
                }

                // Rimuovi l'elemento dalla lista di suggerimenti e aggiorna il campo di testo
                availableSuggestions1.Remove(selectedItem);
                SearchTextBox1.Text = string.Empty;
                popup1.IsOpen = false;

                // Aggiungi l'elemento al pannello sottostante
                AddItemToPanel(selectedItem, selectedItemsPanel1, availableSuggestions1, allSuggestions1);
                suggestionListBox1.SelectedItem = null; // Reset della selezione
            }
        }

        // Evento di selezione per ListBox 2
        private void SuggestionListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (suggestionListBox2.SelectedItem != null)
            {
                string selectedItem = suggestionListBox2.SelectedItem.ToString();

                // Verifica se l'elemento è già presente nel pannello
                if (selectedItemsPanel2.Children.OfType<StackPanel>().Any(p => ((TextBlock)p.Children[0]).Text == selectedItem))
                {
                    MessageBox.Show("Questo elemento è già stato selezionato.");
                    suggestionListBox2.SelectedItem = null; // Reset della selezione
                    return;
                }

                // Rimuovi l'elemento dalla lista di suggerimenti e aggiorna il campo di testo
                availableSuggestions2.Remove(selectedItem);
                SearchTextBox2.Text = string.Empty;
                popup2.IsOpen = false;

                // Aggiungi l'elemento al pannello sottostante
                AddItemToPanel(selectedItem, selectedItemsPanel2, availableSuggestions2, allSuggestions2);
                suggestionListBox2.SelectedItem = null; // Reset della selezione
            }
        }

        // Metodo per aggiungere l'elemento selezionato al pannello e generare il pulsante per rimuoverlo
        private void AddItemToPanel(string selectedItem, StackPanel panel, List<string> availableSuggestions, List<string> allSuggestions)
        {
            // Creiamo un StackPanel orizzontale per contenere l'elemento e il pulsante di rimozione
            StackPanel itemPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5)
            };

            // Etichetta per visualizzare l'elemento selezionato
            TextBlock textBlock = new TextBlock
            {
                Text = selectedItem,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Pulsante per rimuovere l'elemento selezionato
            Button removeButton = new Button
            {
                Content = "Rimuovi",
                Tag = selectedItem,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };
            removeButton.Click += (s, e) =>
            {
                // Quando il pulsante viene cliccato, rimuoviamo l'elemento dal pannello
                panel.Children.Remove(itemPanel);
                // E lo rendiamo nuovamente disponibile nella lista di suggerimenti
                availableSuggestions.Add(selectedItem);

                // Forziamo l'aggiornamento del ListBox per riflettere le modifiche
                suggestionListBox1.ItemsSource = null;
                suggestionListBox1.ItemsSource = availableSuggestions1;

                suggestionListBox2.ItemsSource = null;
                suggestionListBox2.ItemsSource = availableSuggestions2;
            };

            // Aggiungi la label e il pulsante al pannello orizzontale
            itemPanel.Children.Add(textBlock);
            itemPanel.Children.Add(removeButton);

            // Aggiungi l'intero pannello al pannello verticale principale
            panel.Children.Add(itemPanel);
        }

        // Gestione del focus per il campo di testo 1
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

        // Gestione del focus per il campo di testo 2
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

        // Evento per il click del bottone "Elabora"
        private void Elabora_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Elaborazione avviata!");
        }
    }
}

#region

//private void Elabora_Click(object sender, RoutedEventArgs e)
//{
//    // Recupera le date selezionate dai DatePicker
//    DateTime? startDate = StartDatePicker.SelectedDate;
//    DateTime? endDate = EndDatePicker.SelectedDate;

//    // Verifica se le date sono state selezionate
//    if (startDate == null || endDate == null)
//    {
//        // Mostra un messaggio di errore se qualche data non è selezionata
//        MessageBox.Show("Tutte le date devono essere selezionate.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
//        return;
//    }

//    // Converti le date in stringa per ulteriori elaborazioni se necessario
//    string startDateString = startDate.Value.ToString("yyyy-MM-dd");
//    string endDateString = endDate.Value.ToString("yyyy-MM-dd");

//    // Esegui l'elaborazione (sostituisci con la tua logica di elaborazione)
//    if (ElaborazioneFile(startDateString, endDateString/*, opzione1Selezionata, opzione2Selezionata, opzione3Selezionata, numeroInserito*/))
//    {
//        MessageBox.Show("L'elaborazione è stata completata correttamente.", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
//    }
//    else
//    {
//        MessageBox.Show("Si è verificato un errore durante l'elaborazione. Per favore, riprova.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
//    }
//}

//// Funzione xXx che gestisce i dati
//private bool ElaborazioneFile(string startDate, string endDate/*, bool opzione1, bool opzione2, bool opzione3, int numero*/)
//{
//    try
//    {
//        // Assicurati che le date siano nel formato corretto
//        if (string.IsNullOrWhiteSpace(startDate) || string.IsNullOrWhiteSpace(endDate))
//        {
//            throw new ArgumentException("Le date di inizio e fine, insieme al lasso di tempo, devono essere fornite.");
//        }

//        // Verifica che le date siano in formato valido
//        if (!DateTime.TryParse(startDate, out DateTime startDateTime) ||
//            !DateTime.TryParse(endDate, out DateTime endDateTime))
//        {
//            throw new ArgumentException("Le date devono essere nel formato corretto.");
//        }

//        // Crea la query SQL con parametri
//        string query = @"SELECT contacts.first_name, contacts.last_name, contacts.phone_number, sms_messages.campaign_id, COLUMN_GET(dynamic_cols, ""old_id_lista"" as CHAR(100)) as old_id_lista
//                                 FROM sms_messages
//                                 JOIN contacts ON sms_messages.contact_id = contacts.contact_id
//                                 WHERE sms_messages.sent_datetime BETWEEN @StartDate AND @EndDate";


//        #region Crea il pacchetto
//        List<ModelRniVolpi> THEDUMPTRUCK = new List<ModelRniVolpi>();
//        using (MySqlCommand command = new MySqlCommand(query, DatabaseManager.Instance.Connection))
//        {
//            //Aggiungi i parametri alla query
//            command.Parameters.AddWithValue("@StartDate", startDateTime.ToString("yyyy-MM-dd"));
//            command.Parameters.AddWithValue("@EndDate", endDateTime.ToString("yyyy-MM-dd"));

//            // Esegui la query
//            using (MySqlDataReader reader = command.ExecuteReader())
//            {
//                // Elabora i risultati
//                while (reader.Read())
//                {

//                    string provider = "";
//                    string supplier = "";
//                    string rni = "";

//                    CRPS record = new CRPS();
//                    if (LookUpCRPS.lookUpDict.TryGetValue(reader["campaign_id"].ToString(), out record))
//                    {
//                        provider = record.Provider;
//                        supplier = record.Supplier;
//                        rni = record.RNI;

//                    }
//                    else
//                    {
//                        Console.WriteLine($"campagna non trovata: {reader["campaign_id"].ToString()}");

//                    }

//                    ModelRniVolpi modelRniVolpi = new ModelRniVolpi();

//                    modelRniVolpi.first_name = reader["first_name"].ToString();
//                    modelRniVolpi.last_name = reader["last_name"].ToString();
//                    modelRniVolpi.phone_number = reader["phone_number"].ToString();
//                    modelRniVolpi.provider = provider;
//                    modelRniVolpi.supplier = supplier;
//                    modelRniVolpi.old_id_lista = reader["old_id_lista"].ToString();
//                    modelRniVolpi.campagna = rni;
//                    THEDUMPTRUCK.Add(modelRniVolpi);
//                }
//            }
//        }
//        #endregion

//        #region Scrivi
//        string NomeFile = "MEGASERBATOIO AC RNI " + DateTime.Now.ToString("yyyyMMdd") + ".txt";
//        string PathFile = "I:\\GESTIONE SERBATOI NEW\\";
//        CreaFileTesto((PathFile + NomeFile), THEDUMPTRUCK);
//        #endregion

//    }
//    catch (Exception ex)
//    {
//        // Log dell'eccezione e restituzione di false
//        MessageBox.Show($"Errore durante l'elaborazione del file: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
//        return false;
//    }

//    return true; // Restituisci true se tutto è andato a buon fine
//}

//private void CreaFileTesto(string filePath, List<ModelRniVolpi> dataList)
//{
//    // Definisci i nomi delle colonne (headers)
//    string[] headers = new string[]
//    {
//        "last_name", "first_name", "gender", "address", "street_nr", "city", "postal_code", "country",
//        "province", "phone_number", "tax_code", "vat", "date_of_birth", "place_of_birth", "phone_number_2",
//        "phone_number_3", "company_name", "email", "address_2", "address_3", "business_category", "attribute_1",
//        "attribute_2", "attribute_3", "attribute_4", "attribute_5", "attribute_6", "attribute_7", "attribute_8",
//        "attribute_9", "attribute_10", "provider", "supplier", "expiration_datetime", "data_scadenza_privacy",
//        "cpl", "nome_serbatoio", "old_id_lista", "stato_utilizzo", "data_utilizzo", "campagna", "lista"
//    };

//    // Utilizza StreamWriter che creerà il file se non esiste o lo sovrascriverà se esiste
//    using (StreamWriter writer = new StreamWriter(filePath, false)) // 'false' per sovrascrivere se esiste
//    {
//        // Scrivi la prima riga con gli headers, separati da tabulazioni
//        writer.WriteLine(string.Join("\t", headers));

//        // Cicla sui dati e scrivi ogni riga successiva
//        foreach (var data in dataList)
//        {
//            // Crea una lista di valori che verranno scritti in ogni riga
//            List<string> rowValues = new List<string>
//            {
//                data.last_name,
//                data.first_name,
//                "",  // Gender (aggiungi i tuoi valori)
//                "",  // Address
//                "",  // Street number
//                "",  // City
//                "",  // Postal code
//                "",  // Country
//                "",  // Province
//                data.phone_number,
//                "",  // Tax code
//                "",  // VAT
//                "",  // Date of birth
//                "",  // Place of birth
//                "",  // Phone number 2
//                "",  // Phone number 3
//                "",  // Company name
//                "",  // Email
//                "",  // Address 2
//                "",  // Address 3
//                "",  // Business category
//                "",  // Attribute_1
//                "",  // Attribute_2
//                "",  // Attribute_3
//                "",  // Attribute_4
//                "",  // Attribute_5
//                "",  // Attribute_6
//                "",  // Attribute_7
//                "",  // Attribute_8
//                "",  // Attribute_9
//                "",  // Attribute_10
//                data.provider,
//                data.supplier,
//                "",  // Expiration datetime
//                "",  // Data scadenza privacy
//                "",  // CPL
//                "",  // Nome serbatoio
//                data.old_id_lista,
//                "",  // Stato utilizzo
//                "",  // Data utilizzo
//                data.campagna,
//                ""   // Lista
//            };

//            // Scrivi la riga nel file di testo, separando i valori con tabulazioni
//            writer.WriteLine(string.Join("\t", rowValues));
//        }
//    }
//}
#endregion