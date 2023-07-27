using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class SaloonBookingDetails
    {
        public int Saloon_booking_id { get; set; }
        public int Voyager_id { get; set; }
        public int Saloon_id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string name { get; set; }
        public string saloon { get; set; }
        public string amount { get; set; }
    }
}