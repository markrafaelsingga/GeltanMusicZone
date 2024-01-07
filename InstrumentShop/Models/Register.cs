using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InstrumentShop.Models
{
    public class Register
    {
        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter your username.")]
        public string Username { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter your first name.")]
        public string fname { get; set; }
        
        public string mi { get; set; }
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter your last name.")]
        public string lname { get; set; }
        [DisplayName("Department")]
        [Required(ErrorMessage = "Please select a department.")]
        public int Department { get; set; }
        [DisplayName("Date of Birth")]
        [Required(ErrorMessage = "Please enter your birthdate.")]
        public DateTime DateofBirth { get; set; }
        [DisplayName("Phone")]
        [Required(ErrorMessage = "Please enter your phone.")]
        public string Phone { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "Please enter your address.")]
        public string Address { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Please enter your email.")]
        public string Email { get; set; }
       
        public string imagePath { get; set; }
        //public HttpPostedFileBase ImageFile { get; set; }
        [DisplayName("Image")]
        [Required(ErrorMessage = "Image Required")]
        public string Uimg { get; set; }


    }
}