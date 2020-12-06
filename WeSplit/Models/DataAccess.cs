using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
