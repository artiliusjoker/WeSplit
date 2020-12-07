using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using WeSplit.Models;

namespace WeSplit.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private CurrentTripsViewModel currentTripsVM = new CurrentTripsViewModel();
        private CurrentTripsViewModel CurrentTripsVM
        {
            get
            {
                return currentTripsVM;
            }
            set { OnPropertyChanged(ref currentTripsVM, value, null); }
        }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                OnPropertyChanged(ref _currentView, value);
            }
        }

        public ICommand CloseAppCommand { get; set; }
      
        public MainViewModel()
        {
            CurrentView = CurrentTripsVM;
            CloseAppCommand = new RelayCommand<Window>((p) => { return true; }, (p) => 
            {
                if (p != null)
                {
                    p.Close();
                }
            });
        }
    }
}
