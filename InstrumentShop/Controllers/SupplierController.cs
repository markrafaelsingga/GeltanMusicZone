using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using InstrumentShop.Models;
using System.Text.RegularExpressions;


namespace InstrumentShop.Controllers
{
    public class SupplierController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

        public ActionResult Supplier(Supplier model)
        {
            List<Supplier> suppList = new List<Supplier>();
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUPPLIER ORDER BY SUP_ID DESC";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        Supplier supp = new Supplier
                        {
                            suppid = row.Field<int>("SUP_ID"),
                            company = row.Field<string>("SUP_COMPANY"),
                            fname = row.Field<string>("SUP_FNAME"),
                            mi = row.Field<string>("SUP_MI"),
                            lname = row.Field<string>("SUP_LNAME"),
                            status = row.Field<string>("SUP_isActive"),
                            phone = row.Field<string>("SUP_phone"),
                            address = row.Field<string>("SUP_address"),
                            email = row.Field<string>("SUP_email"),

                        };
                        suppList.Add(supp);
                    }
                }
            }
            return View(suppList);
        }

        public ActionResult DeleteSupplier(int suppid)
        {
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                 
                    using (var cmdCheckStatus = db.CreateCommand())
                    {
                        cmdCheckStatus.CommandType = CommandType.Text;
                        cmdCheckStatus.CommandText = "SELECT SUP_ISACTIVE FROM SUPPLIER WHERE SUP_ID = @suppid";
                        cmdCheckStatus.Parameters.AddWithValue("@suppid", suppid);

                        using (var reader = cmdCheckStatus.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentStatus = reader["SUP_ISACTIVE"].ToString();

                              
                                if (currentStatus == "INACTIVE")
                                {
                                    TempData["AlertDelFailed"] = "Supplier is already inactive!";
                                    return RedirectToAction("Supplier");
                                }
                            }
                        }
                    }

                   
                    using (var cmdUpdateStatus = db.CreateCommand())
                    {
                        cmdUpdateStatus.CommandType = CommandType.Text;
                        cmdUpdateStatus.CommandText = "UPDATE SUPPLIER SET SUP_ISACTIVE = @status WHERE SUP_ID = @suppid";
                        cmdUpdateStatus.Parameters.AddWithValue("@status", "INACTIVE");
                        cmdUpdateStatus.Parameters.AddWithValue("@suppid", suppid);

                        var ctr = cmdUpdateStatus.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Supplier Deleted!";
                            return RedirectToAction("Supplier");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Error!";
                            return RedirectToAction("Supplier");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Supplier");
        }


        public ActionResult ReactivateSupplier(int suppid)
        {
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                 
                    using (var cmdCheckStatus = db.CreateCommand())
                    {
                        cmdCheckStatus.CommandType = CommandType.Text;
                        cmdCheckStatus.CommandText = "SELECT SUP_ISACTIVE FROM SUPPLIER WHERE SUP_ID = @suppid";
                        cmdCheckStatus.Parameters.AddWithValue("@suppid", suppid);

                        using (var reader = cmdCheckStatus.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string currentStatus = reader["SUP_ISACTIVE"].ToString();

                              
                                if (currentStatus == "ACTIVE")
                                {
                                    TempData["AlertDelFailed"] = "Supplier is already active!";
                                    return RedirectToAction("Supplier");
                                }
                            }
                        }
                    }

                    using (var cmdUpdateStatus = db.CreateCommand())
                    {
                        cmdUpdateStatus.CommandType = CommandType.Text;
                        cmdUpdateStatus.CommandText = "UPDATE SUPPLIER SET SUP_ISACTIVE = @status WHERE SUP_ID = @suppid";
                        cmdUpdateStatus.Parameters.AddWithValue("@status", "ACTIVE");
                        cmdUpdateStatus.Parameters.AddWithValue("@suppid", suppid);

                        var ctr = cmdUpdateStatus.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Supplier Activated!";
                            return RedirectToAction("Supplier");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Error!";
                            return RedirectToAction("Supplier");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Supplier");
        }


        public ActionResult AddSupplier(string company, string fname, string mi, string lname, string address, string email, string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(email))
                {
                    TempData["AlertField"] = "Input all Fields!";
                    return RedirectToAction("Supplier");
                }

               
                if (!Regex.IsMatch(phone, @"\d+(?:\s-\s\d+)*$"))
                {
                    TempData["AlertRegFailed"] = "Invalid Contact Format!";
                    return RedirectToAction("Supplier");
                }
                else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    TempData["AlertRegFailed"] = "Invalid Email Format!";
                    return RedirectToAction("Supplier");
                }               
                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                  
                    using (var checkCmd = db.CreateCommand())
                    {
                        checkCmd.CommandType = CommandType.Text;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM SUPPLIER WHERE (SUP_FNAME = @fname OR SUP_LNAME = @lname) AND SUP_MI = @mi";
                        checkCmd.Parameters.AddWithValue("@fname", fname);
                        checkCmd.Parameters.AddWithValue("@lname", lname);
                        checkCmd.Parameters.AddWithValue("@mi", string.IsNullOrEmpty(mi) ? (object)DBNull.Value : mi);
                        int existingCount = (int)checkCmd.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            TempData["AlertExistFailed"] = "Supplier existed already!";
                            return RedirectToAction("Supplier");
                        }
                    }

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO SUPPLIER (SUP_COMPANY, SUP_FNAME, SUP_MI, SUP_LNAME, SUP_ADDRESS, SUP_EMAIL, SUP_PHONE) VALUES (@comp, @fname, @mi, @lname, @address, @email, @phone)";
                        cmd.Parameters.AddWithValue("@comp", company);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mi", string.IsNullOrEmpty(mi) ? (object)DBNull.Value : mi);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@phone", phone);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Added Successfully";
                            return RedirectToAction("Supplier");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Failed!";
                            return RedirectToAction("Supplier");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }

            return RedirectToAction("Supplier");
        }

        [HttpPost]
        public ActionResult EditSupplier(int suppid, string company, string fname, string mi, string lname, string phone, string address, string email, Supplier model)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(company) || string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(email))
                {
                    TempData["AlertField"] = "Input all fields!";
                    return RedirectToAction("Supplier");
                }

             
                if (!Regex.IsMatch(phone, @"\d+(?:\s-\s\d+)*$"))
                {
                    TempData["AlertRegFailed"] = "Invalid Contact Format!";
                    return RedirectToAction("Supplier");
                }
                else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    TempData["AlertRegFailed"] = "Invalid Email Format!";
                    return RedirectToAction("Supplier");
                }

                using (var db = new SqlConnection(connString))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE SUPPLIER SET SUP_COMPANY = @company, SUP_FNAME = @fname, SUP_MI = @mi, SUP_LNAME = @lname, SUP_PHONE = @phone, SUP_ADDRESS = @address, SUP_EMAIL = @email WHERE SUP_ID = @suppid";
                        cmd.Parameters.AddWithValue("@suppid", suppid);
                        cmd.Parameters.AddWithValue("@company", company);
                        cmd.Parameters.AddWithValue("@fname", fname);
                        cmd.Parameters.AddWithValue("@mi", mi);
                        cmd.Parameters.AddWithValue("@lname", lname);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@email", email);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            TempData["AlertMessage"] = "Updated Successfully";
                            return RedirectToAction("Supplier");
                        }
                        else
                        {
                            TempData["AlertFailed"] = "Failed";
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Supplier");
        }

    }
}
