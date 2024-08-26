using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_EntryPoint.Utility
{
    public class GFYF
    { 

        public void fuckfuckfuck(string path)
        {
            try
            {
                // Passo 1: Leggi tutte le righe del file
                var lines = new List<string>();
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                // Funzione per rimuovere prefisso +39 o 39
                string RemovePrefix(string value)
                {
                    if (value.Length > 10)
                    {
                        if (value.StartsWith("+39"))
                        {
                            return value.Substring(3); // Rimuove +39
                        }
                        else if (value.StartsWith("39"))
                        {
                            return value.Substring(2); // Rimuove 39
                        }
                    }
                    else if (value.StartsWith("+39"))
                    {
                        return value.Substring(3); // Rimuove +39
                    }
                    return value; // Restituisce il valore senza modifiche
                }

                // Passo 2: Modifica le righe come necessario
                var updatedLines = new List<string> { lines[0] }; // Inizia con l'intestazione
                for (int i = 1; i < lines.Count; i++) // Inizia da 1 per saltare la riga di intestazione
                {
                    var t = lines[i].Split(';');
                    if (t.Length > 24) // Assicurati che ci siano almeno 25 colonne
                    {
                        t[21] = RemovePrefix(t[21]);
                        t[22] = RemovePrefix(t[22]);
                        t[23] = RemovePrefix(t[23]);
                        t[24] = RemovePrefix(t[24]);

                        // Verifica se il numero di telefono modificato ha più di 10 caratteri
                        if (t[21].Length <= 10 && t[22].Length <= 10 && t[23].Length <= 10 && t[24].Length <= 10)
                        {
                            updatedLines.Add(string.Join(";", t)); // Aggiungi solo righe valide
                        }
                    }
                }

                // Recupero la cartella di output
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "DataFile", "OutData", "Pulizia+39");

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

                // Crea il nome del nuovo file
                var newFileName = $"{prefix}{newNumber}.txt";
                var tempFilePath = Path.Combine(directoryPath, newFileName);

                // Passo 3: Scrivi le righe modificate in un nuovo file
                using (StreamWriter writer = new StreamWriter(tempFilePath))
                {
                    foreach (var line in updatedLines)
                    {
                        writer.WriteLine(line);
                    }
                }

                Console.WriteLine($"File salvato come: {tempFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'elaborazione del file: {ex.Message}");
            }
        }
    }
}
