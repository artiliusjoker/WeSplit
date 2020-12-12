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
                    string newThumbnailFileName = Path.GetFileName(newThumbnailOpened);
                    string newThumbnail = $"Assets\\trips\\{TripSelected.ID}\\{newThumbnailFileName}";

                    // Copy hình thumbnail mới vào folder của chương trình
                    string currentFolder = AppDomain.CurrentDomain.BaseDirectory;
                    string thumbnailDestination = $"{currentFolder}{newThumbnail}";
                    File.Copy(newThumbnailOpened, thumbnailDestination, true);

                    TripSelected.ThumnailPath = newThumbnail;
                }
            });

            TripSelected = trip;
            // Danh sách các loại chi phí trong combo box
            AllCostTypes = DataAccess.GetCostsType();
            // Chi phí của chuyến đi
            TripCosts = new ObservableCollection<TripCost>(DataAccess.GetTripCosts(trip.ID));
            // Thành viên của chuyến đi
            TripMembers = DataAccess.GetTripMembers(TripSelected.ID);
            // Tất cả thành viên trong nhóm
            AllMembers = new ObservableCollection<Member>(DataAccess.GetAllMembers());
            // Tất cả hình ảnh của chuyến đi
            TripImages = new ObservableCollection<TripImages>(DataAccess.GetTripImages(TripSelected.ID));
        }

        public Trip TripSelected { get; set; }

        public List<COST> AllCostTypes { get; private set; }

        public ObservableCollection<TripCost> TripCosts { get; set; }

        public BindingList<Member> TripMembers { get; set; }

        public ObservableCollection<Member> AllMembers { get; set; }

        public ObservableCollection<TripImages> TripImages { get; set; }

        public ICommand ChooseTripThumbnailCommand { get; set; }
    }
}
