using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class MovieBookingDetails
    {

        public int Movie_bookings_id { get; set; }
        public int Voyager_id { get; set; }
        public int Movie_id { get; set; }
        public Nullable<int> seat { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
        public string Status { get; set; }
        public string name { get; set; }
        public string movie { get; set; }
    }
}