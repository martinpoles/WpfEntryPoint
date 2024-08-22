using System.Windows;

namespace Wpf_EntryPoint.Windows
{
    public partial class InsertNewCampagna : Window
    {
        public InsertNewCampagna()
        {
            InitializeComponent();
        }
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // Logica per gestire i dati inseriti nei campi di input
            string input1 = InputField1.Text;
            string input2 = InputField2.Text;
            string input3 = InputField3.Text;
            string input4 = InputField4.Text;

            // Verifica se tutti i campi sono valorizzati
            if (string.IsNullOrWhiteSpace(input1) ||
                string.IsNullOrWhiteSpace(input2) ||
                string.IsNullOrWhiteSpace(input3) ||
                string.IsNullOrWhiteSpace(input4))
            {
                // Mostra un messaggio di errore se qualche campo è vuoto
                MessageBox.Show("Tutti i campi devono essere compilati.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Chiudi la finestra dopo la conferma
            this.Close();
        }

    }
}
