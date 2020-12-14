using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Linq;
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
                if (int.TryParse(CostAmountInput, out int amount))
                {
                }
            });
            AddTripImageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                OpenFileDialog open = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "Image Files(*.jpg; *.png; *.jpeg; *.gif; *.bmp)|*.jpg; *.png; *.jpeg; *.gif; *.bmp"
                };
                bool? result = open.ShowDialog();
                if (result == true)
                {
                    // Những hình đã có trong DB
                    var existingImgs = new HashSet<string>(from img in AllTripImages select img.ImagePath);
                    // Những hình đã chọn, lọc ra hình chưa có
                    var newItems = open.FileNames.Where(item => !existingImgs.Contains(item));
                    // Thêm vào nhũng hình chưa có
                    foreach (var item in newItems)
                    {
                        // Thay đổi UI
                        AllTripImages.Add(new TripImages()
                        {
                            Trip_ID = TripSelected.ID,
                            ImagePath = item
                        }) ;
                    }                 
                }
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
            DeleteTripImageCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                TripImages selectedItem = (TripImages)p;
                foreach(TripImages element in AllTripImages)
                {
                    if(element.ImagePath == selectedItem.ImagePath)
                    {
                        AllTripImages.Remove(element);
                        ImagesDeleted.Add(element);
                        break;
                    }    
                }    
            });
            // List deleted
            ImagesDeleted = new List<TripImages>();

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
            AllTripImages = new ObservableCollection<TripImages>(DataAccess.GetTripImages(TripSelected.ID));
            // Tất cả địa điểm của chuyến đi
            TripLocations = new ObservableCollection<Location>(DataAccess.GetTripLocations(TripSelected.ID));           
        }

        public Trip TripSelected { get; set; }

        private Trip tripBinding;
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

        private COST costSelected;
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

        private string costAmountInput;
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

        private ObservableCollection<TripImages> _allTripImages;
        public ObservableCollection<TripImages> AllTripImages
        {
            get
            {
                return _allTripImages;
            }
            set { OnPropertyChanged(ref _allTripImages, value); }
        }

        public ObservableCollection<Location> TripLocations { get; set; }

        public ICommand ChooseTripThumbnailCommand { get; set; }
        public ICommand SaveDetailsCommand { get; set; }
        public ICommand DiscardChangesAndReload { get; set; }

        public ICommand DeleteTripLocationCommand { get; set; }
        public ICommand DeleteTripCostCommand { get; set; }
        public ICommand DeleteTripMemberCommand { get; set; }
        public ICommand DeleteTripImageCommand { get; set; }

        public ICommand AddCostCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand AddTripImageCommand { get; set; }


        #region ADD
        #endregion

        #region DELETE
        List<TripImages> ImagesDeleted { get; set; }
        List<Location> LocationsDeleted { get; set; }
        List<TripCost> MembersDeleted { get; set; }
        List<TripCost> CostsDeleted { get; set; }
        private void ClearDeletedLists()
        {
        }
        #endregion
    }
}
