﻿using System;

using WeSplit.ViewModel;
namespace WeSplit.Models
{
    public class Trip : BaseViewModel
    {
        private string description;
        private string title;
        private string thumnailPath;
        private int _ID;
        private System.DateTime startDate;
        private System.DateTime endDate;
        private bool isDone;
        private string tripStatus;

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

        public Trip(Trip trip)
        {
            ID = trip.ID;
            Description = trip.Description;
            Title = trip.Title;
            thumnailPath = trip.ThumnailPath;
            StartDate = trip.StartDate;
            EndDate = trip.EndDate;
            isDone = trip.IsDone;
        }

        public Trip (int newID)
        {
            ID = newID;
            Description = "";
            Title = "";
            thumnailPath = "";
            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
            isDone = false;
        }

        public Trip Clone()
        {
            return new Trip(this);
        }

        public bool IsAnyFieldNull()
        {
            // Title
            if (Title == "")
            {
                return true;
            }
            // Description
            if (Description == "")
            {
                return true;
            }
            // Thumbnail
            if (thumnailPath == "")
            {
                return true;
            }
            return false;
        }

        public TRIP ToTrip()
        {
            return new TRIP
            {
                TRIP_ID = ID,
                DESCRIPTION = Description,
                TITTLE = Title,
                THUMNAIL = ThumnailPath,
                TOGODATE = StartDate,
                RETURNDATE = EndDate,
                ISDONE = (bool?)IsDone
            };
        }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Title
        {
            get { return title; }
            set { OnPropertyChanged(ref title, value); }
        }
        public string Description
        {
            get { return description; }
            set { OnPropertyChanged(ref description, value); }
        }
        public string ThumnailPath
        {
            get { return thumnailPath; }
            set { OnPropertyChanged(ref thumnailPath, value); }
        }
        public System.DateTime StartDate
        {
            get { return startDate; }
            set { OnPropertyChanged(ref startDate, value); }
        }
        public System.DateTime EndDate
        {
            get { return endDate; }
            set { OnPropertyChanged(ref endDate, value); }
        }
        public bool IsDone
        {
            get { return isDone; }
            set { OnPropertyChanged(ref isDone, value); }
        }
        public string TripStatus
        {
            get
            {
                if (isDone)
                    return $"Đã kết thúc vào ngày {EndDate:dd/MM/yyyy}";
                return $"Dự kiến kết thúc vào ngày {EndDate:dd/MM/yyyy}";
            }
            set { OnPropertyChanged(ref tripStatus, value); }
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
        public Location(Location location)
        {
            this.ID = location.ID;
            this.Name = location.Name;
            this.Address = location.Address;
            this.Description = location.Description;
        }
        public Location()
        {
            this.ID = 0;
            this.Name = "";
            this.Address = "";
            this.Description = "";
        }
        public Location Clone()
        {
            return new Location(this);
        }
        public bool IsAnyFieldNull()
        {
            return Name == "" || Address == "" || Description == "";
        }
        public override string ToString()
        {
            return this.Name;
        }
        public TRIP_LOCATIONS ToTripLocation(int tripID)
        {
            return new TRIP_LOCATIONS()
            {
                TRIP_ID = tripID,
                COSTS = 0,
                LOCATION_ID = ID
            };
        }
        public LOCATION ToLOCATION()
        {
            return new LOCATION()
            {
                NAME = Name,
                ADDRESS = Address,
                DESCRIPTION = Description,
                LOCATION_ID = ID
            };
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
        public Member()
        {
            this.MemberID = 0;
            this.Name = "";
            this.PhoneNumber = "";
            this.Avatar = "NO";
            this.AmountPaid = 0;
        }
        public Member Clone()
        {
            return new Member(this);
        }
        public bool IsAnyFieldNull()
        {
            return Name == "" || PhoneNumber == "";
        }
        public override string ToString()
        {
            return this.Name;
        }

        public TRIP_MEMBERS ToTripMember(int tripID)
        {
            return new TRIP_MEMBERS()
            {
                TRIP_ID = tripID,
                MEMBER_ID = MemberID,
                AMOUNT_PAID = AmountPaid
            };
        }
        public MEMBER ToMEMBER()
        {
            return new MEMBER()
            {
                MEMBER_ID = MemberID,
                NAME = Name,
                PHONENUMBER = PhoneNumber,
                AVATAR = "NO"
            };
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
        public TripCost() { }

        public TRIP_COSTS ToTripCost()
        {
            return new TRIP_COSTS()
            {
                COST_ID = ID,
                TRIP_ID = Trip_ID,
                AMOUNT = Amount
            };
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
        public bool IsNew { get; set; }
        public int Trip_ID { get; set; }

        public TripImages(TRIP_IMAGES trip_image)
        {
            this.ImagePath = trip_image.IMAGE_PATH;
            this.Trip_ID = trip_image.TRIP_ID;
            IsNew = false;
        }
        public TripImages()
        {
            IsNew = true;
        }
        public TRIP_IMAGES ToTripImage()
        {
            return new TRIP_IMAGES()
            {
                IMAGE_PATH = ImagePath,
                TRIP_ID = Trip_ID
            };
        }
    }

}
