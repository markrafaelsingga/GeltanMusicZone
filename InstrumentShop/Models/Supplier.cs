using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InstrumentShop.Models
{
    public class Supplier
    {
        
        public int suppid { get; set; }
        [Required(ErrorMessage = "Enter company")]
        public string company { get; set; }
        [Required(ErrorMessage = "Enter Firstname")]
        public string fname { get; set; }
        [Required(ErrorMessage = "Enter Middle Initial")]
        public string mi { get; set; }
        [Required(ErrorMessage = "Enter Lastname")]
        public string lname { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        public string address { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Enter Phone")]
        public string phone { get; set; }
        
        public string status { get; set; }

    }
}