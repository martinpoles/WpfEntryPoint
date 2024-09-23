using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
