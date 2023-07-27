using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class ViewPaymentDetails
    {
        public int Payment_id { get; set; }
        public Nullable<int> Booking_details_id { get; set; }
        public string Booking_for { get; set; }
        public string Amount { get; set; }
        public string Date { get; set; }
        public string BookingType { get; set; }
        public int VoyagerID { get; set; }
        public string Amnt { get; set; }
        public string Dat { get; set; }
        
    }
}