using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeSplit.Models
{
    public class Trip
    {
        private string description;
        private string title;
        private string thumnailPath;
        private int _ID;
        private System.DateTime startDate;
        private System.DateTime endDate;

        public Trip(TRIP trip)
        {
            Description = trip.DESCRIPTION;
            Title = trip.TITTLE;
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            thumnailPath = folder + trip.THUMNAIL;
            StartDate = (DateTime)trip.TOGODATE;
            EndDate = (DateTime)trip.RETURNDATE;
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string ThumnailPath
        {
            get { return thumnailPath; }
            set { thumnailPath = value; }
        }
        public System.DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        public System.DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

    }
}
