﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class CurrentTripsViewModel : BaseViewModel
    {

        private ObservableCollection<Trip> bindingItems;
        public ObservableCollection<Trip> BindingItems
        {
            get
            {
                if (bindingItems == null)
                {
                    bindingItems = new ObservableCollection<Trip>(Trips.ToList());
                }
                return bindingItems;
            }
            set
            {
                OnPropertyChanged(ref bindingItems, value);
            }
        }
        // danh sach chuyen di
        private List<Trip> _trips = null;
        public List<Trip> Trips
        {
            get
            {             
                return _trips;
            }
            set { OnPropertyChanged(ref _trips, value); }
        }

        public CurrentTripsViewModel()
        {
            NextPageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (this.CurrentIndex < this.paging.TotalPages - 1)
                {
                    this.CurrentIndex += 1;
                }
            });
            PreviousPageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (this.CurrentIndex > 0)
                {
                    this.CurrentIndex -= 1;
                }
            });

            SelectTripCommand = new RelayCommand<object>((selectedItem) => { return selectedItem != null; }, (selectedItem) =>
            {
                Trip tripSelected = (Trip)selectedItem;
                View.DetailTripWindow childWindow = new View.DetailTripWindow(tripSelected.ID);
                childWindow.ShowDialog();
            });

            SearchCommand = new RelayCommand<object>((x) => { return x != null; }, (x) =>
            {
                Search.Keyword = (string)x;
                Trips = DataAccess.SearchTrips(Search);
                bindingItems = new ObservableCollection<Trip>(Trips.ToList());
                CalculatePagingInfo();
                Info = $"Tìm thấy {Trips.Count} kết quả";
            });

            Trips = DataAccess.GetOngoingTrips();

            CalculatePagingInfo();
            Search = new Helpers.SearchInfo();
            Info = "";
        }

        #region Pagination

        private int currentIndex;
        public int CurrentIndex
        {
            get => this.currentIndex;
            set
            {
                OnPropertyChanged(ref currentIndex, value);
                CollectTrips();
            }
        }

        private Helpers.Pagination paging = new Helpers.Pagination();

        private void CollectTrips()
        {
            int page = this.CurrentIndex + 1;
            int skip = (page - 1) * this.paging.RowsPerPage;
            int take = this.paging.RowsPerPage;

            BindingItems = new ObservableCollection<Trip>(this.Trips.Skip(skip)
                                                                   .Take(take)
                                                                   .ToList());
        }

        public ICommand NextPageCommand { get; set; }

        public ICommand PreviousPageCommand { get; set; }

        void CalculatePagingInfo()
        {
            int count = this._trips.Count();
            int rowsPerPage = this.paging.RowsPerPage;

            // Calculate paging info
            paging = new Helpers.Pagination()
            {
                CurrentPage = 1,
                TotalPages = count / rowsPerPage +
                    (((count % rowsPerPage) == 0) ? 0 : 1)
            };
            this.CurrentIndex = 0;
        }

        
        #endregion

        #region ViewTripDetails

        public ICommand SelectTripCommand { get; set; }

        #endregion

        #region Searching
        private string _info;
        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                OnPropertyChanged(ref _info, value);
            }
        }
        public ICommand SearchCommand { get; set; }
        public Helpers.SearchInfo Search { get; set; } 

        #endregion
    }
}
