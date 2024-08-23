using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_EntryPoint.Models;

namespace Wpf_EntryPoint.Utility
{
    public class LookUpCRPS
    {

        public Dictionary<string, CRPS> lookUpDict { get; set; }

        public LookUpCRPS()
        {
            lookUpDict = new Dictionary<string, CRPS>();

            string pathLookUpFile = Directory.GetCurrentDirectory() + "\\DataFile\\Define\\CRPS.xlsx";

            FileInfo fileInfoLookUp = new FileInfo(pathLookUpFile);
            using (ExcelPackage package = new ExcelPackage(fileInfoLookUp))
            {
                // Ottieni il primo foglio di lavoro
                ExcelWorksheet worksheet = package.Workbook.Worksheets.First();

                // Ottieni l'intervallo di celle utilizzate
                var cellRange = worksheet.Cells[worksheet.Dimension.Address];

                // Leggi i valori delle celle
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {

                    if (string.IsNullOrEmpty(worksheet.Cells[row, 1].Text))
                    {
                        continue;
                    }
                    CRPS crps = new CRPS();

                    crps.NuovoRiutilizzo = worksheet.Cells[row, 1].Text;

                    //controllo se la cella è mergata o meno e se lo è prendi il valore della prima cella della lista.
                    #region NRI
                    var cellNri = worksheet.Cells[row, 2];
                    if (cellNri.Merge)
                    {
                        // Trova la cella iniziale dell'intervallo unito
                        var mergedRangeRni = worksheet.MergedCells[row, 2];
                        var startCellRni = worksheet.Cells[mergedRangeRni];

                        // Ottieni il valore della prima cella dell'intervallo unito
                        var firstCellValueRni = startCellRni.First().Text;
                        crps.RNI = firstCellValueRni;
                    }
                    else
                    {
                        // La cella non è unita, puoi ottenere direttamente il suo valore
                        var cellValueRni = cellNri.Text;
                        crps.RNI = cellValueRni;
                    }
                    #endregion

                    #region Provider
                    var cellProvider = worksheet.Cells[row, 3];
                    if (cellProvider.Merge)
                    {
                        // Trova la cella iniziale dell'intervallo unito
                        var mergedRangeProvider = worksheet.MergedCells[row, 3];
                        var startCellProvider = worksheet.Cells[mergedRangeProvider];

                        // Ottieni il valore della prima cella dell'intervallo unito
                        var firstCellValueProvider = startCellProvider.First().Text;
                        crps.Provider = firstCellValueProvider;
                    }
                    else
                    {
                        // La cella non è unita, puoi ottenere direttamente il suo valore
                        var cellValueProvider = cellProvider.Text;
                        crps.Provider = cellValueProvider;
                    }
                    #endregion

                    #region Supplier

                    var cellSupplier = worksheet.Cells[row, 4];
                    if (cellNri.Merge)
                    {
                        // Trova la cella iniziale dell'intervallo unito
                        var mergedRangeSupplier = worksheet.MergedCells[row, 4];
                        var startCellSupplier = worksheet.Cells[mergedRangeSupplier];

                        // Ottieni il valore della prima cella dell'intervallo unito
                        var firstCellValueSupplier = startCellSupplier.First().Text;
                        crps.Supplier = firstCellValueSupplier;
                    }
                    else
                    {
                        // La cella non è unita, puoi ottenere direttamente il suo valore
                        var cellValueSupplier = cellSupplier.Text;
                        crps.Supplier = cellValueSupplier;
                    }
                    #endregion

                    lookUpDict.Add(worksheet.Cells[row, 1].Text, crps);
                }
            }

            var sortedDict = lookUpDict.OrderBy(entry => entry.Key)
                                   .ToDictionary(entry => entry.Key, entry => entry.Value);

            lookUpDict = sortedDict;
        }


    }

    public class TemplateForSmsNuovo 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public string PhoneNumber { get; set; }
        public string OldIdLista { get; set; }

    }

    public class TemplateForSmsRNI
    {
        public string IdCampagna { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string NrTelefono { get; set; }
        public string Stato { get; set; }
        public string Idirizzo { get; set; }
        public string Cap { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string Regione { get; set; }
        public string NomeSerbatoio { get; set; }
        public string OldIdLista { get; set; }
    }

}
