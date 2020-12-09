using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeSplit.Models;

namespace WeSplit.ViewModel
{
    class DetailTripViewModel
    {
        
        public string ImageCount { get; set; }

        public int MemberCount { get; set; } = 0;

        public Trip CurrentTrip { get; set; }

        public List<TripImages> ImageCarousel { get; set; }

        public BindingList<Location> Locations { get; set; }

        public BindingList<Member> Members{ get; set; }

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
        }
    }
}
