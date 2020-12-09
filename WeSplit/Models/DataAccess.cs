using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace WeSplit.Models
{
    class DataAccess
    {
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
    }
}
