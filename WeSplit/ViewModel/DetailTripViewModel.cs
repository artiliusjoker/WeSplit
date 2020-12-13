using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class DetailTripViewModel : BaseViewModel
    {
        #region Toggle
        bool togglePieChart;
        public bool TogglePieChart
        {
            get
            {
                return togglePieChart;
            }

            set
            {
                OnPropertyChanged(ref togglePieChart, value);
            }
        }

        public ICommand ToggleCommand { get; set; }

        #endregion
        public string ImageCount { get; set; }

        public int MemberCount { get; set; } = 0;

        public double TotalExpenses { get; set; } = 0;

        public double AmountSplit { get; set; } = 0;

        public Trip CurrentTrip { get; set; }

        public List<TripImages> ImageCarousel { get; set; }

        public List<TripCost> TripCosts { get; set; }

        public List<Member> MoneySplit { get; set; }

        public List<Location> Locations { get; set; }

        public BindingList<Member> Members{ get; set; }

        public SeriesCollection chartData;

        public SeriesCollection ChartData
        {
            get
            {
                return chartData;
            }
            set
            {
                OnPropertyChanged(ref chartData, value);
            }
         }

        public SeriesCollection ChartTripCosts { get; set; }

        public SeriesCollection ChartMemberPaid { get; set; }

        public DetailTripViewModel(Models.Trip trip)
        {

        }

        public DetailTripViewModel(int tripID)
        {
            TogglePieChart = true;
            ToggleCommand = new RelayCommand<Label>((p) => { return p != null; }, (p) =>
            {
                if (TogglePieChart)
                {
                    ChartData = ChartMemberPaid;
                    togglePieChart = false;
                    p.Content = "Biểu đồ tiền đã chi";
                }
                else
                {
                    ChartData = ChartTripCosts;
                    togglePieChart = true;
                    p.Content = "Biểu đồ tổng chi phí";
                }
            });
            CurrentTrip = DataAccess.GetTripByID(tripID);

            // Các địa điểm của chuyến đi
            Locations = DataAccess.GetTripLocations(tripID);

            // Hình ảnh của chuyển đi
            ImageCarousel = DataAccess.GetTripImages(tripID);
            ImageCount = ImageCarousel.Count.ToString();

            // Thành viên của chuyến đi
            Members = DataAccess.GetTripMembers(tripID);
            MemberCount = Members.Count;
            
            // Chi phí của chuyến đi
            TripCosts = DataAccess.GetTripCosts(tripID);
            // Tạo biểu đồ bánh
            ChartTripCosts = new SeriesCollection();
            ChartMemberPaid = new SeriesCollection();
            // Đưa dữ liệu vào biểu đồ bánh chi phí chuyến đi
            foreach (TripCost element in TripCosts)
            {
                TotalExpenses += element.Amount;

                ChartValues<double> cost = new ChartValues<double>();
                if (element.Amount > 0)
                {
                    cost.Add(element.Amount);
                    PieSeries series = new PieSeries
                    {
                        Values = cost,
                        Title = element.Name
                    };
                    ChartTripCosts.Add(series);
                }
            }
            // Đưa dữ liệu vào biểu đồ bánh tiền đã trả
            foreach (Member element in Members)
            {
                ChartValues<double> amount = new ChartValues<double>();
                if (element.AmountPaid > 0)
                {
                    amount.Add(element.AmountPaid);
                    PieSeries series = new PieSeries
                    {
                        Values = amount,
                        Title = element.Name
                    };
                    ChartMemberPaid.Add(series);
                }
            }
            // Tính toán số tiền mỗi thành viên trả
            AmountSplit = 1.0 * TotalExpenses / MemberCount;
            MoneySplit = new List<Member>();
            foreach(Member member in Members)
            {
                Member newElement = new Member(member)
                {
                    AmountPaid = AmountSplit - member.AmountPaid
                };
                MoneySplit.Add(newElement);
            }
            ChartData = ChartTripCosts;
        }
    }
}
