using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstrumentShop.Models;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

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
                    cmd.CommandText = "select users.user_id, users.user_fname, users.user_mi, users.user_lname,department.dep_name,users.user_username,users.user_password,users.user_pic,users.user_isActive,users.user_phone,users.user_address,users.user_email from [users] join department on department.dep_id = users.dep_id  where users.role_id = 2 ORDER BY USERS.USER_ID DESC;";
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
                            email = row.Field<string>("user_email"),
                            uname = row.Field<string>("user_username"),
                            pword = row.Field<string>("user_password"),
                            uimg = row.Field<string>("user_pic")
                        };
                        staffList.Add(staff);
                    }

                }
            }
            return View(staffList);
        }

        public ActionResult DeleteStaff(int userId)
        {
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                    // Check if the staff is already inactive
                    bool isAlreadyInactive = false;
                    using (var checkCmd = db.CreateCommand())
                    {
                        checkCmd.CommandType = CommandType.Text;
                        checkCmd.CommandText = "SELECT USER_ISACTIVE FROM USERS WHERE USER_ID = @userId";
                        checkCmd.Parameters.AddWithValue("@userId", userId);

                        object statusObj = checkCmd.ExecuteScalar();
                        if (statusObj != null && statusObj != DBNull.Value)
                        {
                            string currentStatus = statusObj.ToString();
                            if (currentStatus == "INACTIVE")
                            {
                                isAlreadyInactive = true;
                            }
                        }
                    }

                    // Inactivate the staff only if it's not already inactive
                    if (!isAlreadyInactive)
                    {
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE USERS SET USER_ISACTIVE = @status WHERE USER_ID = @userId";
                            cmd.Parameters.AddWithValue("@status", "INACTIVE");
                            cmd.Parameters.AddWithValue("@userId", userId);
                            var ctr = cmd.ExecuteNonQuery();

                            if (ctr >= 1)
                            {
                                TempData["AlertMessage"] = "Deleted Successfully";
                                return RedirectToAction("Staff");
                            }
                            else
                            {
                                TempData["AlertFailed"] = "Failed to delete";
                                return RedirectToAction("Staff");
                            }
                        }
                    }
                    else
                    {
                        TempData["AlertDelFailed"] = "Staff is already Inactive!";
                        return RedirectToAction("Staff");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertDelFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Staff");
        }
      
        public ActionResult ReactivateStaff(int userId)
        {
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();
                    bool isAlreadyActive = false;
                    using (var checkCmd = db.CreateCommand())
                    {
                        checkCmd.CommandType = CommandType.Text;
                        checkCmd.CommandText = "SELECT USER_ISACTIVE FROM USERS WHERE USER_ID = @userId";
                        checkCmd.Parameters.AddWithValue("@userId", userId);

                        object statusObj = checkCmd.ExecuteScalar();
                        if (statusObj != null && statusObj != DBNull.Value)
                        {
                            string currentStatus = statusObj.ToString();
                            if (currentStatus == "ACTIVE")
                            {
                                isAlreadyActive = true;
                            }
                        }
                    }
                    if (!isAlreadyActive)
                    {
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE USERS SET USER_ISACTIVE = @status WHERE USER_ID = @id";
                            cmd.Parameters.AddWithValue("@status", "ACTIVE");
                            cmd.Parameters.AddWithValue("@id", userId);
                            var ctr = cmd.ExecuteNonQuery();

                            if (ctr >= 1)
                            {
                                TempData["AlertMessage"] = "Activated Successfully";
                                return RedirectToAction("Staff");
                            }
                            else
                            {
                                TempData["AlertFailed"] = "Operation Failed!";
                                return RedirectToAction("Staff");
                            }
                        }
                    }
                    else
                    {
                        TempData["AlertDelFailed"] = "Staff is already activated!";
                        return RedirectToAction("Staff");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Staff");
        }

        [HttpPost]
        public ActionResult EditStaff(int userId, string fname, string mi, string lname, int department, string phone, string address, string email, string uname, string pword, Staff model)
        {
            try
            {
                if (string.IsNullOrEmpty(fname) || string.IsNullOrEmpty(lname) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pword))
                {
                    TempData["ALertField"] = "Input all fields!";
                    return RedirectToAction("Staff");
                }

                using (var db = new SqlConnection(connString))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE USERS SET USER_FNAME = @fname, USER_MI = @mi, USER_LNAME = @lname, USER_PHONE = @phone, USER_ADDRESS = @address, USER_USERNAME = @uname,USER_PASSWORD = @pword,USER_EMAIL=@email, DEP_ID=@dep WHERE USER_ID = @id";
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mi", mi);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@uname", uname);
                        cmd.Parameters.AddWithValue("@pword", pword);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@dep", department);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Updated!";
                            return RedirectToAction("Staff");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Failed to updated!";
                            return RedirectToAction("Staff");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Staff");
        }
    }
}

