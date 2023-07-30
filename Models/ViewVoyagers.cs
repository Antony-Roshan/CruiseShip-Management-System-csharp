using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class ViewVoyagers
    {
        public int Voyager_id { get; set; }
        public int Login_id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
    }
}