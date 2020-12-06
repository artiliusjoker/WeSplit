using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Models
{
    public class TripService
    {
        public static List<Trip> GetOngoingTrips()
        {
            return DataAccess.GetOngoingTrips();
        }
    }
}
