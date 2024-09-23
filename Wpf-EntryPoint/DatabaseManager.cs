using System;
using System.IO;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient; // Namespace per MySQL

namespace Wpf_EntryPoint
{
    public sealed class DatabaseManager
    {
        private static readonly Lazy<DatabaseManager> _instance =
            new Lazy<DatabaseManager>(() => new DatabaseManager());

        private MySqlConnection _connection;
        private string _connectionString;

        private DatabaseManager()
        {
            try
            {
                LoadConnectionString();
                OpenConnection();
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'inizializzazione di DatabaseManager: {ex.Message}");
            }
        }

        public static DatabaseManager Instance => _instance.Value;

        public MySqlConnection Connection => _connection;

        private void LoadConnectionString()
        {
            try
            {
                string jsonFilePath =  Directory.GetCurrentDirectory() +  "\\appSetting.json"; // Modifica con il percorso corretto
                if (System.IO.File.Exists(jsonFilePath))
                {
                    string jsonString = System.IO.File.ReadAllText(jsonFilePath);
                    var config = System.Text.Json.JsonSerializer.Deserialize<DatabaseConfig>(jsonString);
                    _connectionString = config.DatabaseSettings.ConnectionString;
                }
                else
                {
                    throw new Exception("Il file di configurazione del database non è stato trovato.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore nel caricamento della connection string: {ex.Message}");
            }
        }

        private void OpenConnection()
        {
            try
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante l'apertura della connessione: {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }

    public class DatabaseConfig
    {
        public DatabaseSettings DatabaseSettings { get; set; }
    }

    public class DatabaseSettings
    {
        public string ConnectionString { get; set; }
    }
}
