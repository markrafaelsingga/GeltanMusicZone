using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class Staff
    {
        public string fname { get; set; }
        public string mi { get; set; }
        public string lname { get; set; }
        public int Department { get; set; }
        public string dep { get; set; }
        public string status { get; set; }
        public int userId { get; set; }
        public DateTime dob { get; set; }

        public string phone { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int depId { get; set; }
        public string depName { get; set; }
        public string uname { get; set; }
        public string pword { get; set; }
        public string uimg { get; set; }
       /* public List<Department> Departments { get; set; } = new List<Department>();*/


        //@staff.userId, '@staff.fname', '@staff.mi', '@staff.lname', '@staff.department','@staff.dob','@staff.phone','@staff.address','@staff.email
    }
}