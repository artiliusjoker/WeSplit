//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WeSplit.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TRIP_LOCATIONS
    {
        public int TRIP_ID { get; set; }
        public int LOCATION_ID { get; set; }
        public double COSTS { get; set; }
    
        public virtual LOCATION LOCATION { get; set; }
        public virtual TRIP TRIP { get; set; }
    }
}
