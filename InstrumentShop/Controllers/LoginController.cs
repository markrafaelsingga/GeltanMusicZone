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
    public class LoginController : Controller
    {
        // GET: Login
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
       
        public ActionResult Login(Login model)
        {
            string uname = model.Username;
            string pword = model.Password;

            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.CommandText = "SELECT USER_ID, ROLE_ID ,USER_USERNAME, USER_PASSWORD FROM [USERS] WHERE USER_USERNAME LIKE '" + uname + "' AND USER_PASSWORD LIKE '" + pword + "'";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int role_id = Convert.ToInt32(reader["ROLE_ID"]);
                            Session["user"] = uname;
                            Session["pass"] = pword;
                            Session["role_id"] = role_id;
                            return RedirectToAction("AdminPage", "Home");

                        }
                        else
                        {
                            return View();
                        }
                    }
                }
            }
        }

    }
}