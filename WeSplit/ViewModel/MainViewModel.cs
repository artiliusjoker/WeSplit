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

        public ICommand UpdateTrip { get; set; }

        public ICommand DiscardChangesAndGoBack { get; set; }

        public ICommand OpenHomeView { get; set; }

        public ICommand OpenAddTripView { get; set; }

        public ICommand OpenLocationsView { get; set; }

        public ICommand OpenMembersView { get; set; }

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

            UpdateTrip = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                Trip tripSelected = (Trip)p;
                CurrentView = new UpdateTripViewModel(tripSelected);
            });

            DiscardChangesAndGoBack = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = CurrentTripsVM;
            });
            OpenHomeView = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = CurrentTripsVM;
            });
            OpenAddTripView = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = CurrentTripsVM;
            });
            OpenLocationsView = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = CurrentTripsVM;
            });
            OpenMembersView = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = CurrentTripsVM;
            });
        }
    }
}
