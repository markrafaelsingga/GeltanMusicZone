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
    public class AdminProfileController : Controller
    {
        // GET: AdminProfile
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Source\Repos\GeltanMusicZone\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        int id;
        public ActionResult AdminProfile()
        {
            id = (int)Session["user_id"];
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM USERS WHERE USER_ID = @ID";
                    cmd.Parameters.AddWithValue("@ID",id);
                    
                    using(var reader = cmd.ExecuteReader())
                    {
                        
                        if(reader.Read())
                        {
                            var model = new AdminProfile
                            {
                                firstName = reader["user_fname"].ToString(),
                                mi = reader["user_mi"].ToString(),
                                lastName = reader["user_lname"].ToString(),
                                Phone = reader["user_phone"].ToString(),
                                Email = reader["user_email"].ToString(),
                                Address = reader["user_address"].ToString(),

                            };
                            return View(model);
                        }
                        
                    }
                }
            }
            return Json(new { success = false, message = "Failed to save data" });
        }

        public ActionResult EditProfile(AdminProfile model)
        {
            id = (int)Session["user_id"];
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "update users set user_fname = @fname, user_mi = @mi, user_lname = @lname, user_address = @address, user_phone = @phone, user_email = @email where user_id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@fname", model.firstName);
                    cmd.Parameters.AddWithValue("@mi", model.mi);
                    cmd.Parameters.AddWithValue("@lname", model.lastName);
                    cmd.Parameters.AddWithValue("@address", model.Address);
                    cmd.Parameters.AddWithValue("@phone", model.Phone);
                    cmd.Parameters.AddWithValue("@email", model.Email);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
                    {
                        return Json(new { success = true, message = "Updated successfully" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Failed to save data" });
                    }
                }
            }
           
        }
    }
}