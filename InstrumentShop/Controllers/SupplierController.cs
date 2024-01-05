using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using InstrumentShop.Models;

namespace InstrumentShop.Controllers
{
    public class SupplierController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        // GET: Supplier
        public ActionResult Supplier(Supplier model)
        {
            List<Supplier> suppList = new List<Supplier>();
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUPPLIER";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        Supplier staff = new Supplier
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
                        suppList.Add(staff);
                    }
                }
            }
            return View(suppList);
        }

        public ActionResult DeleteSupplier(int suppid)
        {
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE SUPPLIER SET SUP_ISACTIVE = @status WHERE SUP_ID = @suppid";
                    cmd.Parameters.AddWithValue("@status", "INACTIVE");
                    cmd.Parameters.AddWithValue("@suppid", suppid);
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
        public ActionResult ReactivateSuppler(int suppid)
        {
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE SUPPLIER SET SUPPLIER_ISACTIVE = @status WHERE SUP_ID = @suppid";
                    cmd.Parameters.AddWithValue("@status", "ACTIVE");
                    cmd.Parameters.AddWithValue("@suppid", suppid);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr >= 1)
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

        public ActionResult EditSupplier(int suppid,string company,string fname,string mi,string lname,string phone,string address,string email)
        {
            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE SUPPLIER SET SUP_COMPANY = @company ,SUP_FNAME = @fname, SUP_MI = @mi, SUP_LNAME = @lname, SUP_PHONE = @phone, SUP_ADDRESS = @address, SUP_EMAIL=@email WHERE SUP_ID = @suppid";
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