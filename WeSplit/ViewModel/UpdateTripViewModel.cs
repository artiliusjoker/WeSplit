using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class UpdateTripViewModel : BaseViewModel
    {
        public UpdateTripViewModel() { }

        public UpdateTripViewModel(Trip trip) 
        {
            AddCostCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                var x = CostAmountInput;
                var y = CostSelected;
            });
            ChooseTripThumbnailCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog open = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Image Files(*.jpg; *.png; *.jpeg; *.gif; *.bmp)|*.jpg; *.png; *.jpeg; *.gif; *.bmp"
                };
                bool? result = open.ShowDialog();
                if (result == true)
                {
                    // Thay đổi UI
                    var newThumbnailOpened = open.FileName;
                    TripBinding.ThumnailPath = newThumbnailOpened;                
                }
            });
            SaveDetailsCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                bool isChanged = false;
                // Title
                if (TripBinding.Title != TripSelected.Title)
                {
                    TripSelected.Title = TripBinding.Title;
                    isChanged = true;
                }
                // Description
                if (TripBinding.Description != TripSelected.Description)
                {
                    TripSelected.Description = TripBinding.Description;
                    isChanged = true;
                }
                // Copy hình thumbnail mới vào folder của chương trình và lưu record vào DB
                if (TripBinding.ThumnailPath != TripSelected.ThumnailPath)
                {
                    string newThumbnailFileName = Path.GetFileName(TripBinding.ThumnailPath);
                    string newThumbnail = $"Assets\\trips\\{TripBinding.ID}\\{newThumbnailFileName}";
                    string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string newThumbnailDestination = $"{currentFolder}{newThumbnail}";
                    File.Copy(TripBinding.ThumnailPath, newThumbnailDestination, true);
                    TripSelected.ThumnailPath = newThumbnail;
                    isChanged = true;
                }
                // StartDate
                if (TripBinding.StartDate != TripSelected.StartDate)
                {
                    TripSelected.StartDate = TripBinding.StartDate;
                    isChanged = true;
                }
                // EndDate
                if (TripBinding.EndDate != TripSelected.EndDate)
                {
                    TripSelected.EndDate = TripBinding.EndDate;
                    isChanged = true;
                }
                if(isChanged)
                {
                    DataAccess.UpdateTripInfo(TripSelected);
                }    
            });
            DiscardChangesAndReload = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                PopulateData();             
            });
            // Get selected Trip
            TripSelected = trip;
            // Danh sách các loại chi phí trong combo box
            AllCostTypes = DataAccess.GetCostsType();
            // Tất cả thành viên trong nhóm
            AllMembers = DataAccess.GetAllMembers();
            // Tất cả địa điểm có thể đi
            AllLocations = DataAccess.GetAllLocations();
            // Populate dynamic data
            PopulateData();
        }

        private void PopulateData()
        {
            TripBinding = TripSelected.Clone();
            
            // Chi phí của chuyến đi
            TripCosts = new ObservableCollection<TripCost>(DataAccess.GetTripCosts(TripSelected.ID));
            // Thành viên của chuyến đi
            TripMembers = DataAccess.GetTripMembers(TripSelected.ID);
            // Tất cả hình ảnh của chuyến đi
            TripImages = new ObservableCollection<TripImages>(DataAccess.GetTripImages(TripSelected.ID));
            // Tất cả địa điểm của chuyến đi
            TripLocations = new ObservableCollection<Location>(DataAccess.GetTripLocations(TripSelected.ID));
        }

        public Trip TripSelected { get; set; }

        public Trip tripBinding;
        public Trip TripBinding 
        {
            get
            {
                return tripBinding;
            }
            set
            {
                OnPropertyChanged(ref tripBinding, value);
            }
        }

        public COST costSelected;
        public COST CostSelected
        {
            get
            {
                return costSelected;
            }
            set
            {
                OnPropertyChanged(ref costSelected, value);
            }
        }

        public string costAmountInput;
        public string CostAmountInput
        {
            get
            {
                return costAmountInput;
            }
            set
            {
                OnPropertyChanged(ref costAmountInput, value);
            }
        }

        public List<COST> AllCostTypes { get; private set; }

        public List<Member> AllMembers { get; set; }

        public List<Location> AllLocations { get; set; }

        public ObservableCollection<TripCost> TripCosts { get; set; }

        public BindingList<Member> TripMembers { get; set; }

        public ObservableCollection<TripImages> TripImages { get; set; }

        public ObservableCollection<Location> TripLocations { get; set; }

        public ICommand ChooseTripThumbnailCommand { get; set; }
        public ICommand SaveDetailsCommand { get; set; }
        public ICommand DiscardChangesAndReload { get; set; }
        public ICommand AddCostCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
    }
}
