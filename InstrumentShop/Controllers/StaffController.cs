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
    public class StaffController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

        public ActionResult Staff(Staff model)
        {
            string name = Session["uname"].ToString();
            ViewBag.uname = name;
            List<Staff> staffList = new List<Staff>();
            List<Department> departments = new List<Department>();
  
            using (var db1 = new SqlConnection(connString))
            {
                db1.Open();
                using (var cmd1 = db1.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT dep_id, dep_name FROM DEPARTMENT";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd1);
                    sda.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Department department = new Department
                        {
                            DepartmentId = Convert.ToInt32(row["dep_id"]),
                            DepartmentName = row["dep_name"].ToString()
                        };
                        departments.Add(department);

                    }
                    ViewBag.DepList = new SelectList(departments, "DepartmentId", "DepartmentName");
                 
                }
            }

            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select users.user_id, users.user_fname, users.user_mi, users.user_lname,department.dep_name,users.user_isActive,users.user_phone,users.user_address,users.user_email from [users] join department on department.dep_id = users.dep_id  where users.role_id = 2;";
                    cmd.Parameters.AddWithValue("@role_id", 2);
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        Staff staff = new Staff
                        {
                            userId = row.Field<int>("USER_ID"),
                            fname = row.Field<string>("USER_FNAME"),
                            mi = row.Field<string>("USER_MI"),
                            lname = row.Field<string>("USER_LNAME"),
                            dep = row.Field<string>("DEP_NAME"),
                            status = row.Field<string>("user_isActive"),
                            phone = row.Field<string>("user_phone"),
                            address = row.Field<string>("user_address"),
                            email = row.Field<string>("user_email")
                        };
                        staffList.Add(staff);
                    }

                }
            }
            return View(staffList);
        }

        public ActionResult DeleteStaff(int userId)
        {
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE USERS SET USER_ISACTIVE = @STATUS WHERE USER_ID = @USER_ID";
                    cmd.Parameters.AddWithValue("@STATUS", "INACTIVE");
                    cmd.Parameters.AddWithValue("@USER_ID", userId);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
                    {
                        return Json(new { success = true, message = "Deleted Successfully!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Unsuccessful" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        public ActionResult ViewStaff(int userId)
        {
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM USERS WHERE ROLE_ID = @role and USER_ID = @user_id";
                    cmd.Parameters.AddWithValue("@role", 2);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
                    {
                        return Json(new { success = true, message = "Deactivated!" }, JsonRequestBehavior.AllowGet);
                        
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }

        public ActionResult ReactivateStaff(int userId)
        {
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE USERS SET USER_ISACTIVE = @status WHERE USER_ID = @id";
                    cmd.Parameters.AddWithValue("@status","ACTIVE");
                    cmd.Parameters.AddWithValue("@id",userId);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr>=1)
                    {
                        return Json(new { success = true, message = "Activated!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult EditStaff(int userId,string fname,string mi,string lname, string phone,string address, string email, Staff model)
        {
            
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE USERS SET USER_FNAME = @fname, USER_MI = @mi, USER_LNAME = @lname, USER_PHONE = @phone, USER_ADDRESS = @address, USER_EMAIL=@email, DEP_ID=@dep WHERE USER_ID = @id";
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.Parameters.AddWithValue("@fname", fname);
                    cmd.Parameters.AddWithValue("@mi", mi);
                    cmd.Parameters.AddWithValue("@lname", lname);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@dep", 1);

                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
                    {
                        return Json(new { success = true, message = "Updated Successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Unsuccessful" });
                    }
                }
            }
        }
    }
}

