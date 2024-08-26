using System.ComponentModel;

namespace Wpf_EntryPoint.ViewModels
{
    class PuliziaNumeri_39_ViewModel : INotifyPropertyChanged
    {
        private string _message = "Benvenuto Nel Pulizia +39";
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
