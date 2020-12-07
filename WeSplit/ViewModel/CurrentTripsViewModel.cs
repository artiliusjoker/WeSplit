using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class CurrentTripsViewModel : BaseViewModel
    {
        // danh sach chuyen di
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
    }
}
