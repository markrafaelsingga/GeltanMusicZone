using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InstrumentShop.Controllers
{
    public class AdminDepartmentController : Controller
    {
        // GET: AdminDepartment
        public ActionResult AdminDepartment()
        {
            string name = Session["uname"].ToString();
            ViewBag.uname = name;
            return View();
        }
    }
}