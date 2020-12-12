using System;

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
        private bool isDone;

        public Trip(TRIP trip)
        {
            ID = trip.TRIP_ID;
            Description = trip.DESCRIPTION;
            Title = trip.TITTLE;
            thumnailPath = trip.THUMNAIL;
            StartDate = (DateTime)trip.TOGODATE;
            EndDate = (DateTime)trip.RETURNDATE;
            isDone = (bool)trip.ISDONE;
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
        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }
    }

    public class Location : ViewModel.BaseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public Location(LOCATION location)
        {
            this.ID = location.LOCATION_ID;
            this.Name = location.NAME;
            this.Address = location.ADDRESS;
            this.Description = location.DESCRIPTION;
        }

    }

    public class Member : ViewModel.BaseViewModel
    {
        public int MemberID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }

        public double AmountPaid { get; set; }

        public Member(MEMBER member)
        {
            this.MemberID = member.MEMBER_ID;
            this.Name = member.NAME;
            this.PhoneNumber = member.PHONENUMBER;
            this.Avatar = member.AVATAR;
        }
        public Member(Member member)
        {
            this.MemberID = member.MemberID;
            this.Name = member.Name;
            this.PhoneNumber = member.PhoneNumber;
            this.Avatar = member.Avatar;
            this.AmountPaid = member.AmountPaid;
        }

    }

    public class TripCost : ViewModel.BaseViewModel
    {
        public int ID { get; set; }
        public int Trip_ID { get; set; }

        public string Name { get; set; }

        public double Amount { get; set; }

        public TripCost(TRIP_COSTS costs)
        {
            this.ID = costs.COST_ID;
            this.Trip_ID = costs.TRIP_ID;
            this.Amount = (double)costs.AMOUNT;
        }

    }
    public class TripMember : ViewModel.BaseViewModel
    {

    }
    public class TripLocation : ViewModel.BaseViewModel
    {

    }
    public class TripImages : ViewModel.BaseViewModel
    {
        public string ImagePath { get; set; }

        public TripImages(string imgPath)
        {
            this.ImagePath = imgPath;
        }
    }

}
