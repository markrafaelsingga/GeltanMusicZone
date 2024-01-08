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
        int? role_id, dep_id;
        int id;
        string user;
        string pass;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model)
        {
            try
            {
                string uname = model.Username;
                string pword = model.Password;

                using (var db = new SqlConnection(connString))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT USER_ID, DEP_ID, ROLE_ID, USER_USERNAME, USER_PASSWORD FROM [USERS] WHERE USER_USERNAME = @uname AND USER_PASSWORD = @pword";

                        // Add parameters using Add method
                        cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.NVarChar) { Value = uname });
                        cmd.Parameters.Add(new SqlParameter("@pword", SqlDbType.NVarChar) { Value = pword });

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Rest of your code
                                id = Convert.ToInt32(reader["USER_ID"]);
                                role_id = Convert.ToInt32(reader["ROLE_ID"]);
                                user = Convert.ToString(reader["USER_USERNAME"]);
                                pass = Convert.ToString(reader["USER_PASSWORD"]);
                                dep_id = (reader["DEP_ID"] as int?) ?? 0;

                                Session["user_id"] = id;
                                Session["role_id"] = role_id;

                                if (uname == user && pword == pass)
                                {
                                    if (dep_id == 1 && role_id == 2)
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else if (dep_id == 2 && role_id == 2)
                                    {
                                        return RedirectToAction("Index", "Purchase");
                                    }
                                    else if (role_id == 1)
                                    {
                                        return RedirectToAction("AdminPage", "Home");
                                    }
                                    else
                                    {
                                        TempData["AlertMessage"] = "Invalid Account!";
                                        return View("Login", model);
                                    }
                                }
                                else
                                {
                                    TempData["AlertMessage"] = "Invalid!";
                                    return View("Login", model);
                                }
                            }
                        }
                    }
                }

                TempData["AlertMessage"] = "Invalid Account!";
                return View("Login", model);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                TempData["AlertMessage"] = "An error occurred during login.";
                return View("Login", model);
            }
        }
    }
}