using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using InstrumentShop.Models;
using System.Drawing.Imaging;
using System.IO;

namespace InstrumentShop.Controllers
{

    public class RegisterController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";


        private List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            using (var db = new SqlConnection(connString))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT dep_id, dep_name FROM DEPARTMENT";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
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
                }
            }
            return departments;
        }


        [HttpGet]
        public ActionResult Register()
        {
            try
            {
                List<Department> departments = GetDepartments();
                ViewBag.DepList = new SelectList(departments, "DepartmentId", "DepartmentName");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            return View(new Register());

        }     
        [HttpPost]
        public ActionResult Register(Register model, HttpPostedFileBase file)
        {
            try
            {
                string user = model.Username;
                string pass = model.Password;
                string fname = model.fname;
                string mi = model.mi;
                mi = string.IsNullOrEmpty(mi) ? null : mi;
                string lname = model.lname;
                DateTime dob = model.DateofBirth;
                string phone = model.Phone;
                string address = model.Address;
                string email = model.Email;
                int dep_id = model.Department;
               


                // Check if the user already exists
                if (IsUserExists(user, pass))
                {
                    TempData["AlertAcc"] = "Account already exists!";
                    return RedirectToAction("Register");
                }

                // Rest of your code for file handling and other validations...
                List<Department> departments = GetDepartments();
                ViewBag.DepList = new SelectList(departments, "DepartmentId", "DepartmentName");

                if (file == null || file.ContentLength == 0)
                {
                    TempData["AlertImage"] = "Image is Required!";
                    return RedirectToAction("Register");
                }


                if (!ModelState.IsValid)
                {

                    return View(model);
                }
                // Insert the new user
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                    using (var cmd1 = db.CreateCommand())
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = "INSERT INTO [USERS] (user_fname, user_mi, user_lname, user_dob, user_phone, user_address, user_email, user_username, user_password, role_id, dep_id, user_pic)" +
                            "VALUES(@fname, @mi, @lname, @dob, @phone, @address, @email, @user, @pass, @role_id, @DepartmentId, @pic)";

                        // Add parameters for the insert command
                        cmd1.Parameters.AddWithValue("@fname", fname);
                        cmd1.Parameters.AddWithValue("@mi", string.IsNullOrEmpty(mi) ? DBNull.Value : (object)mi);
                        cmd1.Parameters.AddWithValue("@lname", lname);
                        cmd1.Parameters.AddWithValue("@dob", dob);
                        cmd1.Parameters.AddWithValue("@phone", phone);
                        cmd1.Parameters.AddWithValue("@address", address);
                        cmd1.Parameters.AddWithValue("@email", email);
                        cmd1.Parameters.AddWithValue("@user", user);
                        cmd1.Parameters.AddWithValue("@pass", pass);
                        cmd1.Parameters.AddWithValue("@role_id", 2);
                        cmd1.Parameters.AddWithValue("@DepartmentId", dep_id);
                        cmd1.Parameters.AddWithValue("@pic", "~/images/" + file.FileName);
                        var ctr = cmd1.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Registered!";
                            return RedirectToAction("Register");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Failed to Register!";
                            return RedirectToAction("Register");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }

            return RedirectToAction("Register");
        }

        private bool IsUserExists(string username, string password)
        {
            using (var db = new SqlConnection(connString))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM USERS WHERE USER_USERNAME = @user AND USER_PASSWORD = @pass";
                    cmd.Parameters.AddWithValue("@user", username);
                    cmd.Parameters.AddWithValue("@pass", password);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}