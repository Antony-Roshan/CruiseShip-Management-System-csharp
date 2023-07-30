using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CruiseshipApp.Models
{
    public class ViewEmployeeDetails
    {
        public int Employee_id { get; set; }
        public int Login_id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
    }
}