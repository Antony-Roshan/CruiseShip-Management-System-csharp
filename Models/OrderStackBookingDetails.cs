using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class OrderStackBookingDetails
    {
        public int Order_id { get; set; }
        public string Voyager_id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
        public string Status { get; set; }

    }
}