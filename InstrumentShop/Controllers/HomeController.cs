using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstrumentShop.Models;
using System.Data;
using System.Data.SqlClient;

namespace InstrumentShop.Controllers
{
    public class HomeController : Controller
    {
      

       

        public ActionResult AdminPage()
        {
            
            return View();
        }
    }
}