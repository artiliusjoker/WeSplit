using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;

using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class DetailTripViewModel
    {
        
        public string ImageCount { get; set; }

        public int MemberCount { get; set; } = 0;

        public double TotalExpenses { get; set; } = 0;

        public double AmountSplit { get; set; } = 0;

        public Trip CurrentTrip { get; set; }

        public List<TripImages> ImageCarousel { get; set; }

        public List<TripCost> TripCosts { get; set; }

        public BindingList<Location> Locations { get; set; }

        public BindingList<Member> Members{ get; set; }

        public SeriesCollection ChartData { get; set; }

        public DetailTripViewModel(Models.Trip trip)
        {

        }

        public DetailTripViewModel(int tripID)
        {
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
            // Create Pie chart
            ChartData = new SeriesCollection();
            // populate expenses in piechart
            foreach (TripCost element in TripCosts)
            {
                TotalExpenses += element.Amount;

                ChartValues<double> cost = new ChartValues<double>();
                if (element.Amount > 0)
                {
                    cost.Add(System.Convert.ToDouble(element.Amount));
                    PieSeries series = new PieSeries
                    {
                        Values = cost,
                        Title = element.Name
                    };
                    ChartData.Add(series);
                }
            }
        }
    }
}
