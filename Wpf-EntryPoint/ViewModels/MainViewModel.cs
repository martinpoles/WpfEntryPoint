﻿using System.ComponentModel;
using System.Windows.Input;

namespace Wpf_EntryPoint.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainViewModel()
        {
            // Imposta la vista iniziale se necessario
            CurrentView = new LandingPage_ViewModel();

            ShowNuovaListaCommand = new RelayCommand(ShowNuovaLista);
            ShowNRICommand = new RelayCommand(ShowNRI);
            ShowPulizia39Command = new RelayCommand(ShowPulizia39);
            ShowLandingPageCommand = new RelayCommand(ShowLandinPage);
            

        }

        public ICommand ShowNuovaListaCommand { get; }
        public ICommand ShowNRICommand { get; }
        public ICommand ShowPulizia39Command { get; }
        public ICommand ShowLandingPageCommand { get; }

        private void ShowNuovaLista()
        {
            CurrentView = new SmsMassivi_NuovaLista_ViewModel();
        }
        private void ShowNRI()
        {
            CurrentView = new SmsMassivi_Rni_ViewModel();
        }
        private void ShowPulizia39()
        {
            CurrentView = new PuliziaNumeri_39_ViewModel();
        }

        private void ShowLandinPage()
        {
            CurrentView = new LandingPage_ViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}