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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public partial class Party_hall
    {
        [Key]
        public int Hall_id { get; set; }
        public string Hall_name { get; set; }
        public string Time { get; set; }
        public string Occasion { get; set; }
        [DisplayName("Upload File")]
        public string Image { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string Price { get; set; }
        public string Status { get; set; }
    }
}
