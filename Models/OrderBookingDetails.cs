using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class OrderBookingDetails
    {
        public int Orderdetails_id { get; set; }
        public int Order_id { get; set; }
        public int Item_id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
        

    }
}