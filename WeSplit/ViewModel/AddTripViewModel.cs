using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class AddTripViewModel : BaseViewModel
    {
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

        public AddTripViewModel()
        {
            TripSelected = new Trip(0);
            ResetView();
            // Danh sách các loại chi phí trong combo box
            AllCostTypes = DataAccess.GetCostsType();
            // Tất cả thành viên trong nhóm
            AllMembers = DataAccess.GetAllMembers();
            // Tất cả địa điểm có thể đi
            AllLocations = DataAccess.GetAllLocations();

            // Delete commands
            DeleteTripImageCommand = new RelayCommand<object>((p) => { return p != null; }, (p) =>
            {
                TripImages selectedItem = (TripImages)p;
                foreach (TripImages element in AllTripImages)
                {
                    if (element.ImagePath == selectedItem.ImagePath)
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

            // Add commands
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
                CustomDialog.ShowDialog("Địa điểm đã có trong danh sách", CustomDialog.Buttons.OK);
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
                    CustomDialog.ShowDialog("Thành viên đã có trong danh sách, thay đổi tiền trong bảng", CustomDialog.Buttons.OK);
                    return;
                }
                CustomDialog.ShowDialog("Nhập tiền không đúng định dạng", CustomDialog.Buttons.OK);
            });
            AddCostCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                if (int.TryParse(CostAmountInput, out int amount))
                {
                    double doubleAmount = (double)amount;
                    // Những chi phí đã có trong DB
                    var existingCosts = new HashSet<int>(from cost in TripCosts select cost.ID);
                    // Kiểm tra chi phí mới có trong DB chưa
                    bool isExisted = existingCosts.Any(costID => costID == CostCBBSelected.COST_ID);
                    if (!isExisted)
                    {
                        // Thêm vào chi phí mới lên UI
                        TripCosts.Add(new TripCost()
                        {
                            Name = CostCBBSelected.NAME,
                            ID = CostCBBSelected.COST_ID,
                            Trip_ID = TripSelected.ID,
                            Amount = doubleAmount
                        });
                        return;
                    }
                    CustomDialog.ShowDialog("Chi phí đã có trong danh sách, thay đổi tiền trong bảng", CustomDialog.Buttons.OK);
                    return;
                }
                CustomDialog.ShowDialog("Nhập tiền không đúng định dạng", CustomDialog.Buttons.OK);
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
                        });
                    }
                }
            });
            AddTripThumbnailCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
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
            // Other commands          
            AddNewTripCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                TripSelected = TripBinding.Clone();
                if (TripSelected.IsAnyFieldNull())
                {
                    CustomDialog.ShowDialog("Không thể thêm mới: có thông tin bỏ trống !", CustomDialog.Buttons.OK);
                    return; 
                }
                // Copy hình thumbnail mới vào folder của chương trình và lưu record vào DB          
                string newThumbnail = Utils.StringHelper.CopyFile(TripBinding.ThumnailPath, TripSelected.ID, true);
                TripSelected.ThumnailPath = newThumbnail;

                int newID = DataAccess.AddNewTrip(TripSelected);
                if ( newID < 0)
                {
                    CustomDialog.ShowDialog("Không thể thêm mới: lỗi nghiêm trọng !", CustomDialog.Buttons.OK);
                    return; 
                }
                TripSelected.ID = newID;
                // Địa điểm             
                DataAccess.UpdateAddRemoveTripLocations(TripSelected.ID, TripLocations.ToList());
                // Hình ảnh
                foreach (TripImages image in AllTripImages)
                {
                    image.Trip_ID = newID;
                    if (image.IsNew)
                    {
                        string newImage = Utils.StringHelper.CopyFile(image.ImagePath, TripSelected.ID, false);
                        image.ImagePath = newImage;
                        image.IsNew = false;
                    }
                }
                DataAccess.UpdateAddRemoveTripImages(TripSelected.ID, AllTripImages.ToList());
                // Thành viên
                DataAccess.UpdateAddRemoveTripMembers(TripSelected.ID, TripMembers.ToList());
                // Chi phí
                foreach(TripCost tripCost in TripCosts)
                {
                    tripCost.Trip_ID = newID;
                }    
                DataAccess.UpdateAddRemoveTripCosts(TripSelected.ID, TripCosts.ToList());
                CustomDialog.ShowDialog("Thêm mới thành công", CustomDialog.Buttons.OK);
            });
            DiscardChangesAndReload = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ResetView();           
            });
        }

        #region Commands      
        public ICommand DiscardChangesAndReload { get; set; }

        public ICommand DeleteTripLocationCommand { get; set; }
        public ICommand DeleteTripCostCommand { get; set; }
        public ICommand DeleteTripMemberCommand { get; set; }
        public ICommand DeleteTripImageCommand { get; set; }

        public ICommand AddCostCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand AddTripImageCommand { get; set; }
        public ICommand AddTripThumbnailCommand { get; set; }
        public ICommand AddNewTripCommand { get; set; }
        #endregion

        #region Lists
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
        #endregion

        #region InputBindings
        public string CostAmountInput { get; set; }
        public string MemberCostAmountInput { get; set; }
        public Member MemberCBBSelected { get; set; }
        public Location LocationCBBSelected { get; set; }
        public COST CostCBBSelected { get; set; }
        #endregion

        #region Functions
        private void ResetView()
        {
            TripBinding = TripSelected.Clone();
            // Chi phí của chuyến đi
            TripCosts = new ObservableCollection<TripCost>();
            // Thành viên của chuyến đi
            TripMembers = new ObservableCollection<Member>();
            // Tất cả hình ảnh của chuyến đi
            AllTripImages = new ObservableCollection<TripImages>();
            // Tất cả địa điểm của chuyến đi
            TripLocations = new ObservableCollection<Location>();
        }
        #endregion
    }
}
