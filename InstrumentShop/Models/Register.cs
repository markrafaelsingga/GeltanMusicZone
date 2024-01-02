using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace InstrumentShop.Models
{
    public class Register
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string fname { get; set; }
        public string mi { get; set; }
        public string lname { get; set; }
        public int Department { get; set; }
        public DateTime DateofBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
       
        public string imagePath { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }


    }
}