using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class FitnessBookingDetails
    {
        public int Booking_details_id { get; set; }
        public string Booking_type { get; set; }
        public int Booking_for_id { get; set; }
        public int Voyager_id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string Fitname { get; set; }
        public string Place { get; set; }
    }
}