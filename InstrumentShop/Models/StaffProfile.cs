using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class StaffProfile
    {
        public string fname { get; set; }
        public string mi { get; set; }
        public string lname { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime dob { get; set; }
        public string department { get; set; }
        public string role { get; set; }
    }
}