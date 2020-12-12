using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using WeSplit.Utils;

namespace WeSplit.Models
{
    class DataAccess
    {       
        public static List<Trip> SearchTrips(Helpers.SearchInfo info)
        {
            List<Trip> result = new List<Trip>();         
            if(!info.MemberSearchChecked)
            {
                if (info.TripFinishedSearchChecked)
                {
                    result.AddRange(GetFinishedTrips());
                }

                if (info.TripOngoingSearchChecked)
                {
                    result.AddRange(GetOngoingTrips());
                }
                if (info.Keyword == "")
                {
                    return result;
                }
                result = new List<Trip>(result.Where(trip =>
                {
                    string tripName = StringHelper.ConvertToNoSpaceAndUnsigned(trip.Title);
                    string keyword = StringHelper.ConvertToNoSpaceAndUnsigned(info.Keyword);
                    return tripName.Contains(keyword);
                }));
                return result;
            }
            if (info.MemberSearchChecked)
            {
                result = GetTripsBasedOnMemberName(info);
            }
            return result;
        }
        public static List<Trip> GetTripsBasedOnMemberName(Helpers.SearchInfo searchInfo)
        {
            List<Trip> list = new List<Trip>();
            var query = from trip in DatabaseEntity.Entity.DB.TRIPs
                        join tripMember in DatabaseEntity.Entity.DB.TRIP_MEMBERS on trip.TRIP_ID equals tripMember.TRIP_ID
                        select new { trip, tripMember.MEMBER };
            foreach(var row in query)
            {
                if(searchInfo.Keyword != null)
                {
                    string memberName = StringHelper.ConvertToNoSpaceAndUnsigned(row.MEMBER.NAME);
                    string keyword = StringHelper.ConvertToNoSpaceAndUnsigned(searchInfo.Keyword);
                    if (memberName.Contains(keyword))
                    {
                        Trip trip = new Trip(row.trip);
                        list.Add(trip);
                    }
                }    
            }    
            return list;
        }
        public static List<Trip> GetFinishedTrips()
        {
            List<Trip> list = new List<Trip>(DatabaseEntity.Entity.DB.TRIPs.ToList()
                .Where(x => x.ISDONE == false)
                .Select(x => new Trip(x)));
            return list;
        }
        public static List<Trip> GetOngoingTrips()
        {
            List<Trip> list = new List<Trip>(DatabaseEntity.Entity.DB.TRIPs.ToList()
                .Where(x => x.ISDONE == true)
                .Select(x => new Trip(x)));
            return list;
        }
        public static Trip GetTripByID(int tripID)
        {
            TRIP trip = DatabaseEntity.Entity.DB.TRIPs.SingleOrDefault(x => x.TRIP_ID == tripID);
            return new Trip(trip);
        }

        public static BindingList<Location> GetTripLocations(int tripID)
        {
            List<Location> list = new List<Location>(DatabaseEntity.Entity.DB.TRIP_LOCATIONS.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => new Location(x.LOCATION)));

            return new BindingList<Location>(list);
        }

        public static List<TripImages> GetTripImages(int tripID)
        {
            List<TripImages> list = new List<TripImages>(DatabaseEntity.Entity.DB.TRIP_IMAGES.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => new TripImages(x.IMAGE_PATH)));
            return list;
        }
        public static BindingList<Member> GetTripMembers(int tripID)
        {
            List<Member> list = new List<Member>(DatabaseEntity.Entity.DB.TRIP_MEMBERS.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => {
                    Member member = new Member(x.MEMBER)
                    {
                        AmountPaid = x.AMOUNT_PAID
                    };
                    return member;
                }));
            return new BindingList<Member>(list);
        }
        public static List<TripCost> GetTripCosts(int tripID)
        {
            List<TripCost> list = new List<TripCost>(DatabaseEntity.Entity.DB.TRIP_COSTS.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => {
                    TripCost costs = new TripCost(x)
                    {
                        Name = x.COST.NAME
                    };
                    return costs;
                }));
            return list;
        }
        public static List<COST> GetCostsType()
        {
            List<COST> list = new List<COST>(DatabaseEntity.Entity.DB.COSTs.ToList());
            return list;
        }
        public static List<Member> GetAllMembers()
        {
            List<Member> list = new List<Member>(DatabaseEntity.Entity.DB.MEMBERs.ToList().Select(x => new Member(x)));
            return list;
        }
    }
}
