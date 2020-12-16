using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class LocationViewModel : BaseViewModel
    {
        private Location newLocation;
        public Location NewLocation
        {
            get
            {
                return newLocation;
            }
            set
            {
                OnPropertyChanged(ref newLocation, value);
            }
        }
        private ObservableCollection<Location> locations;
        public ObservableCollection<Location> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                OnPropertyChanged(ref locations, value);
            }
        }
        public ICommand AddLocationCommand { get; set; }
        public ICommand ResetViewCommand { get; set; }
        public ICommand SaveLocationsCommand { get; set; }

        public LocationViewModel()
        {
            AddLocationCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (NewLocation.IsAnyFieldNull())
                {
                    CustomDialog.ShowDialog("Có thông tin bỏ trống !", CustomDialog.Buttons.OK);
                    return;
                }
                // Những địa điểm đã có trong DB
                var existingLocations = new HashSet<string>(from location in Locations select location.Name);
                // Kiểm tra địa điểm được thêm vào có trong DB chưa
                bool isExisted = existingLocations.Any(locationName => locationName == NewLocation.Name);
                if (!isExisted)
                {
                    // Thêm vào địa điểm mới lên UI
                    Locations.Add(NewLocation.Clone());
                    NewLocation = new Location();
                    return;
                }
                NewLocation = new Location();
                CustomDialog.ShowDialog("Địa điểm đã có !", CustomDialog.Buttons.OK);
            });
            ResetViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ResetView();
            });
            SaveLocationsCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                
            });
            ResetView();
        }
        private void ResetView()
        {
            NewLocation = new Location();
            Locations = new ObservableCollection<Location>(DataAccess.GetAllLocations());
        }
    }
}
