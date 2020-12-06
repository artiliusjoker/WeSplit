using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

using WeSplit.Models;

namespace WeSplit.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand CloseAppCommand { get; set; }

        // danh sach danh muc
        private List<Trip> _trips = null;
        public List<Trip> Trips
        {
            get
            {
                if (_trips == null)
                {
                    _trips = TripService.GetOngoingTrips();
                }
                return _trips;
            }
            set { OnPropertyChanged(ref _trips, value); }
        }

        public MainViewModel()
        {          
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
