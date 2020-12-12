using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class UpdateTripViewModel
    {
        public UpdateTripViewModel() { }

        public UpdateTripViewModel(Trip trip) 
        {
            TripSelected = trip;
        }

        public Trip TripSelected { get; set; }
    }
}
