﻿using Microsoft.Win32;
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
            AddLocationCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                // Những địa điểm đã có trong DB
                var existingLocations = new HashSet<int>(from location in TripLocations select location.ID);
                // Kiểm tra thành viên được thêm vào có trong DB chưa
                bool isExisted = existingLocations.Any(locationID => locationID == LocationCBBSelected.ID);
                if (!isExisted)
                {
                    // Thêm vào thành viên mới lên UI
                    TripLocations.Add(new Location(LocationCBBSelected));
                    return;
                }
                CustomDialog.ShowDialog("Địa điểm đã có !", CustomDialog.Buttons.OK);
            });
            AddMemberCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (int.TryParse(MemberCostAmountInput, out int amount))
                {
                    double doubleAmount = (double)amount;
                    // Những thành viên đã có trong DB
                    var existingMembers = new HashSet<int>(from member in TripMembers select member.MemberID);
                    // Kiểm tra thành viên được thêm vào có trong DB chưa
                    bool isExisted = existingMembers.Any(memberID => memberID == MemberCBBSelected.MemberID);
                    if (!isExisted)
                    {
                        // Thêm vào thành viên mới lên UI
                        TripMembers.Add(new Member(MemberCBBSelected)
                        {
                            AmountPaid = doubleAmount
                        });
                        return;
                    }
                    CustomDialog.ShowDialog("Thành viên đã có, thay đổi số tiền trên bảng", CustomDialog.Buttons.OK);
                }
            });
            AddCostCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (int.TryParse(CostAmountInput, out int amount))
                {
                    double doubleAmount = (double)amount;
                    // Những chi phí đã có trong DB
                    var existingCosts = new HashSet<int>(from cost in TripCosts select cost.ID);
                    // Kiểm tra chi phí mới có trong DB chưa
                    bool isExisted = existingCosts.Any(costID => costID == CostSelected.COST_ID);
                    if(!isExisted)
                    {
                        // Thêm vào chi phí mới lên UI
                        TripCosts.Add(new TripCost()
                        {
                            Name = CostSelected.NAME,
                            ID = CostSelected.COST_ID,
                            Trip_ID = TripSelected.ID,
                            Amount = doubleAmount
                        });
                        return;
                    }
                    CustomDialog.ShowDialog("Chi phí đã có, thay đổi số tiền trên bảng", CustomDialog.Buttons.OK);
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
                // Kiểm tra có bỏ trống trường nào không
                if (TripBinding.IsAnyFieldNull())
                {
                    CustomDialog.ShowDialog("Có thông tin bị bỏ trống, xin mời nhập lại !", CustomDialog.Buttons.OK);
                    return;
                }
                // Kiểm tra ngày về có sớm hơn ngày đi hay không
                if (DateTime.Compare(TripBinding.EndDate, TripBinding.StartDate) < 0)
                {
                    CustomDialog.ShowDialog("Nhập sai ngày đi hoặc ngày về !", CustomDialog.Buttons.OK);
                    return;
                }
                // Kiểm tra tiền
                double totalMemberPaid = 0;
                double totalCosts = 0;
                foreach(Member member in TripMembers)
                {
                    totalMemberPaid += member.AmountPaid;
                }
                foreach (TripCost cost in TripCosts)
                {
                    totalCosts += cost.Amount;
                }
                if(totalMemberPaid > totalCosts)
                {
                    CustomDialog.ShowDialog("Tiền các thành viên nhiều hơn chi phí chuyến đi, xin hãy nhập lại !", CustomDialog.Buttons.OK);
                    return;
                }    
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
                    string newThumbnail = Utils.StringHelper.CopyFile(TripBinding.ThumnailPath, TripSelected.ID, true);
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
                // Kiểm tra ngày về với ngày hiện tại, nếu sớm hơn thì đã đi xong rồi, ngược lại thì đang đi
                if (DateTime.Compare(TripSelected.EndDate, DateTime.Today) < 0)
                {
                    TripSelected.IsDone = true;
                    isChanged = true;
                }
                else TripSelected.IsDone = false;
                // Lưu dữ liệu vào DB
                if (isChanged)
                {
                    DataAccess.UpdateTripInfo(TripSelected);
                }
                // Địa điểm
                DataAccess.UpdateAddRemoveTripLocations(TripSelected.ID, TripLocations.ToList());
                // Hình ảnh
                foreach (TripImages image in AllTripImages)
                {
                    if (image.IsNew)
                    {
                        string newThumbnail = Utils.StringHelper.CopyFile(image.ImagePath, TripSelected.ID, false);
                        image.ImagePath = newThumbnail;
                        image.IsNew = false;
                    }
                }
                DataAccess.UpdateAddRemoveTripImages(TripSelected.ID, AllTripImages.ToList());
                // Thành viên
                DataAccess.UpdateAddRemoveTripMembers(TripSelected.ID, TripMembers.ToList());
                // Chi phí
                DataAccess.UpdateAddRemoveTripCosts(TripSelected.ID, TripCosts.ToList());

                CustomDialog.ShowDialog("Chỉnh sửa chuyến đi thành công", CustomDialog.Buttons.OK);
            });
            DiscardChangesAndReload = new RelayCommand<object>((p) => { return true; }, (p) =>
            {               
                Reset();             
            });
            DeleteTripImageCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                TripImages selectedItem = (TripImages)p;
                foreach(TripImages element in AllTripImages)
                {
                    if(element.ImagePath == selectedItem.ImagePath)
                    {
                        AllTripImages.Remove(element);
                        break;
                    }    
                }    
            });
            DeleteTripCostCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                TripCost selectedItem = (TripCost)p;
                TripCosts.Remove(selectedItem);
            });
            DeleteTripLocationCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                Location selectedItem = (Location)p;
                TripLocations.Remove(selectedItem);
            });
            DeleteTripMemberCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                Member selectedItem = (Member)p;
                TripMembers.Remove(selectedItem);  
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
            Reset();
        }

        private void Reset()
        {
            TripBinding = TripSelected.Clone();
            // Chi phí của chuyến đi
            TripCosts = new ObservableCollection<TripCost>(DataAccess.GetTripCosts(TripSelected.ID));
            // Thành viên của chuyến đi
            TripMembers = new ObservableCollection<Member>(DataAccess.GetTripMembers(TripSelected.ID));
            // Tất cả hình ảnh của chuyến đi
            AllTripImages = new ObservableCollection<TripImages>(DataAccess.GetTripImages(TripSelected.ID));
            // Tất cả địa điểm của chuyến đi
            TripLocations = new ObservableCollection<Location>(DataAccess.GetTripLocations(TripSelected.ID));      
        }

        private Trip TripSelected { get; set; }

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

        public List<COST> AllCostTypes { get; private set; }

        public List<Member> AllMembers { get; set; }

        public List<Location> AllLocations { get; set; }

        private ObservableCollection<TripCost> _tripCosts;
        public ObservableCollection<TripCost> TripCosts 
        {
            get
            {
                return _tripCosts;
            }
            set
            {
                OnPropertyChanged(ref _tripCosts, value);
            }
        }
        private ObservableCollection<Member> _tripMembers;
        public ObservableCollection<Member> TripMembers
        {
            get
            {
                return _tripMembers;
            }
            set
            {
                OnPropertyChanged(ref _tripMembers, value);
            }
        }
        private ObservableCollection<TripImages> _allTripImages;
        public ObservableCollection<TripImages> AllTripImages
        {
            get
            {
                return _allTripImages;
            }
            set { OnPropertyChanged(ref _allTripImages, value); }
        }
        private ObservableCollection<Location> _tripLocations;
        public ObservableCollection<Location> TripLocations
        {
            get
            {
                return _tripLocations;
            }
            set
            {
                OnPropertyChanged(ref _tripLocations, value);
            }
        }

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
        public COST CostSelected { get; set; }
        public string CostAmountInput { get; set; }    
        public string MemberCostAmountInput { get; set; }
        public Member MemberCBBSelected { get; set; }
        public Location LocationCBBSelected { get; set; }
        #endregion

        #region DELETE       
        #endregion
    }
}
