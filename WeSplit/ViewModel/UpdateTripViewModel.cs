using System.Collections.Generic;
using System.Collections.ObjectModel;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class UpdateTripViewModel : BaseViewModel
    {
        public UpdateTripViewModel() { }

        public UpdateTripViewModel(Trip trip) 
        {
            TripSelected = trip;
            // Danh sách các loại chi phí trong combo box
            CostComboContent = DataAccess.GetCostsType();
            // Chi phí của chuyến đi
            TripCosts = new ObservableCollection<TripCost>(DataAccess.GetTripCosts(trip.ID));
        }

        public Trip TripSelected { get; set; }

        public List<string> CostComboContent { get; private set; }

        public ObservableCollection<TripCost> TripCosts
        {
            get;
            set;         
        }
    }
}
