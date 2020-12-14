
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using WeSplit.Utils;

namespace WeSplit.Models
{
    class DataAccess
    {
        #region QueryData
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
                .Where(x => x.ISDONE == true)
                .Select(x => new Trip(x)));
            return list;
        }
        public static List<Trip> GetOngoingTrips()
        {
            List<Trip> list = new List<Trip>(DatabaseEntity.Entity.DB.TRIPs.ToList()
                .Where(x => x.ISDONE == false)
                .Select(x => new Trip(x)));
            return list;
        }
        public static Trip GetTripByID(int tripID)
        {
            TRIP trip = DatabaseEntity.Entity.DB.TRIPs.SingleOrDefault(x => x.TRIP_ID == tripID);
            return new Trip(trip);
        }

        public static List<Location> GetTripLocations(int tripID)
        {
            List<Location> list = new List<Location>(DatabaseEntity.Entity.DB.TRIP_LOCATIONS.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => new Location(x.LOCATION)));

            return list;
        }

        public static List<TripImages> GetTripImages(int tripID)
        {
            List<TripImages> list = new List<TripImages>(DatabaseEntity.Entity.DB.TRIP_IMAGES.ToList()
                .Where(x => x.TRIP_ID == tripID)
                .Select(x => new TripImages(x)));
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
        public static List<Location> GetAllLocations()
        {
            List<Location> list = new List<Location>(DatabaseEntity.Entity.DB.LOCATIONs.ToList().Select(x => new Location(x)));
            return list;
        }
        #endregion

        #region UpdateData

        public static void UpdateTripInfo(Trip updatedItem)
        {
            TRIP item = new TRIP()
            {
                TRIP_ID = updatedItem.ID,
                DESCRIPTION = updatedItem.Description,
                TITTLE = updatedItem.Title,
                THUMNAIL = updatedItem.ThumnailPath,
                TOGODATE = updatedItem.StartDate,
                RETURNDATE = updatedItem.EndDate,
                ISDONE = (bool?)updatedItem.IsDone
        };
            var entity = DatabaseEntity.Entity.DB.TRIPs.Find(item.TRIP_ID);
            if (entity == null)
            {
                return;
            }
            DatabaseEntity.Entity.DB.Entry(entity).CurrentValues.SetValues(item);
            DatabaseEntity.Entity.DB.SaveChanges();
        }

        #endregion

        #region ListData
        public static void UpdateAddRemoveTripLocations(int tripID, List<Location> tripLocations)
        {
            var currentTripLocations = DatabaseEntity.Entity.DB.TRIP_LOCATIONS
                                        .Where(tl => tl.TRIP_ID == tripID);
            // Delete children
            foreach (var existingElement in currentTripLocations.ToList())
            {
                if (!tripLocations.Any(c => c.ID == existingElement.LOCATION_ID))
                    DatabaseEntity.Entity.DB.TRIP_LOCATIONS.Remove(existingElement);
            }
            // Update and Insert children
            List<TRIP_LOCATIONS> newElements = new List<TRIP_LOCATIONS>();
            foreach (Location location in tripLocations)
            {
                var existingElement = DatabaseEntity.Entity.DB.TRIP_LOCATIONS
                    .Where(element => (element.LOCATION_ID == location.ID) && (element.TRIP_ID == tripID))
                    .SingleOrDefault();

                if (existingElement != null)
                {
                    // Update
                    existingElement.LOCATION_ID = location.ID;
                }                       
                else
                {
                    newElements.Add(location.ToTripLocation(tripID));
                }
            }
            foreach(TRIP_LOCATIONS element in newElements)
            {
                DatabaseEntity.Entity.DB.TRIP_LOCATIONS.Add(element);
            }    
            // Save
            DatabaseEntity.Entity.DB.SaveChanges();
        }
        public static void UpdateAddRemoveTripImages(int tripID, List<TripImages> tripImages)
        {
            var currentTripImages = DatabaseEntity.Entity.DB.TRIP_IMAGES
                                        .Where(tl => tl.TRIP_ID == tripID);
            // Delete children
            foreach (var existingElement in currentTripImages.ToList())
            {
                if (!tripImages.Any(c => c.ImagePath == existingElement.IMAGE_PATH))
                    DatabaseEntity.Entity.DB.TRIP_IMAGES.Remove(existingElement);
            }
            // Update and Insert children
            List<TRIP_IMAGES> newElements = new List<TRIP_IMAGES>();
            foreach (TripImages image in tripImages)
            {
                var existingElement = DatabaseEntity.Entity.DB.TRIP_IMAGES
                    .Where(element => (element.IMAGE_PATH == image.ImagePath) && (element.TRIP_ID == tripID))
                    .SingleOrDefault();

                if (existingElement != null)
                {
                    // Update
                    existingElement.IMAGE_PATH = image.ImagePath;
                }
                else
                {
                    newElements.Add(image.ToTripImage());
                }
            }
            foreach (TRIP_IMAGES element in newElements)
            {
                DatabaseEntity.Entity.DB.TRIP_IMAGES.Add(element);
            }
            // Save
            DatabaseEntity.Entity.DB.SaveChanges();
        }
        public static void UpdateAddRemoveTripMembers(int tripID, List<Member> tripMembers)
        {
            var currentTripMembers = DatabaseEntity.Entity.DB.TRIP_MEMBERS
                                        .Where(tl => tl.TRIP_ID == tripID);
            // Delete children
            foreach (var existingElement in currentTripMembers.ToList())
            {
                if (!tripMembers.Any(c => c.MemberID == existingElement.MEMBER_ID))
                    DatabaseEntity.Entity.DB.TRIP_MEMBERS.Remove(existingElement);
            }
            // Update and Insert children
            List<TRIP_MEMBERS> newElements = new List<TRIP_MEMBERS>();
            foreach (Member member in tripMembers)
            {
                var existingElement = DatabaseEntity.Entity.DB.TRIP_MEMBERS
                    .Where(element => (element.MEMBER_ID == member.MemberID) && (element.TRIP_ID == tripID))
                    .SingleOrDefault();

                if (existingElement != null)
                {
                    // Update
                    existingElement.AMOUNT_PAID = member.AmountPaid;
                }
                else
                {
                    newElements.Add(member.ToTripMember(tripID));
                }
            }
            foreach (TRIP_MEMBERS element in newElements)
            {
                DatabaseEntity.Entity.DB.TRIP_MEMBERS.Add(element);
            }
            // Save
            DatabaseEntity.Entity.DB.SaveChanges();
        }
        public static void UpdateAddRemoveTripCosts(int tripID, List<TripCost> tripCosts)
        {
            var currentTripCosts = DatabaseEntity.Entity.DB.TRIP_COSTS
                                        .Where(tl => tl.TRIP_ID == tripID);
            // Delete children
            foreach (var existingElement in currentTripCosts.ToList())
            {
                if (!tripCosts.Any(c => c.ID == existingElement.COST_ID))
                    DatabaseEntity.Entity.DB.TRIP_COSTS.Remove(existingElement);
            }
            // Update and Insert children
            List<TRIP_COSTS> newElements = new List<TRIP_COSTS>();
            foreach (TripCost cost in tripCosts)
            {
                var existingElement = DatabaseEntity.Entity.DB.TRIP_COSTS
                    .Where(element => (element.COST_ID == cost.ID) && (element.TRIP_ID == cost.Trip_ID))
                    .SingleOrDefault();

                if (existingElement != null)
                {
                    // Update
                    existingElement.AMOUNT = cost.Amount;
                }
                else
                {
                    newElements.Add(cost.ToTripCost());
                }
            }
            foreach (TRIP_COSTS element in newElements)
            {
                DatabaseEntity.Entity.DB.TRIP_COSTS.Add(element);
            }
            // Save
            DatabaseEntity.Entity.DB.SaveChanges();
        }
        #endregion
    }
}
