//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CruiseshipApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order_details
    {
        public int Orderdetails_id { get; set; }
        public int Order_id { get; set; }
        public int Item_id { get; set; }
        public string Quantity { get; set; }
        public string Amount { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
