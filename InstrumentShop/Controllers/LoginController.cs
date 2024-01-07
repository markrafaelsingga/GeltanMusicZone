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

        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        int? role_id,dep_id;
        int id;
        string user;
        string pass;
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
                        cmd.CommandText = "SELECT USER_ID, DEP_ID ,ROLE_ID ,USER_USERNAME, USER_PASSWORD FROM [USERS] WHERE USER_USERNAME LIKE '" + uname + "' AND USER_PASSWORD LIKE '" + pword + "'";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            id = Convert.ToInt32(reader["USER_ID"]);
                            role_id = Convert.ToInt32(reader["ROLE_ID"]);
                            user = Convert.ToString(reader["USER_USERNAME"]);
                            pass = Convert.ToString(reader["USER_PASSWORD"]);
                            dep_id = (reader["DEP_ID"] as int?) ?? 0;

                            Session["user_id"] = id;
                            Session["role_id"] = role_id;
                            if (uname == user & pword == pass)
                            {
                                if (dep_id == 1 && role_id == 2)
                                {
                                    return RedirectToAction("Index", "Home");                                    
                                }                              
                                else if (dep_id == 2 && role_id == 2)
                                {
                                    return RedirectToAction("Index", "Purchase");
                                }
                                else
                                {
                                    return RedirectToAction("AdminPage", "Home");
                                }                             
                            }
                            else
                            {
                                return Json(new { success = false, message = "Invalid Account" }, JsonRequestBehavior.AllowGet);
                            }
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