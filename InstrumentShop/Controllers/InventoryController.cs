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
    public class InventoryController : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

        // GET: Inventory
        public ActionResult Inventory()
        {
            List<Product> prodList = new List<Product>();
            using (var db = new SqlConnection(connString))
            {
              
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM PRODUCT";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        Product prod = new Product
                        {
                            prodId = row.Field<int>("PROD_ID"),
                            prodDesc = row.Field<string>("PROD_DESC"),
                            prodCode = row.Field<string>("PROD_CODE"),
                            prodName = row.Field<string>("PROD_NAME"),
                            prodPrice = row.Field<decimal>("PROD_PRICE")
                        };
                        prodList.Add(prod);
                    }
                }
            }
            return View(prodList);
        }

        public ActionResult AddProduct(string prodName,string prodDesc,string prodPrice,int? supId)
        {
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                { 
                    cmd.CommandText = "INSERT INTO PRODUCT(prod_name,prod_desc,prod_price,sup_id)" +
                        "VALUES(@prodName,@prodDesc,@prodPrice,@supId)";
                    cmd.Parameters.AddWithValue("@prodName",prodName);
                    cmd.Parameters.AddWithValue("@prodDesc", prodDesc);
                    cmd.Parameters.AddWithValue("@prodPrice", prodPrice);
                    cmd.Parameters.AddWithValue("@supId", supId);
                    var ctr = cmd.ExecuteNonQuery();
                    if(ctr > 0)
                    {
                        return Json(new { success = true, message = "Added Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Unsuccessfull!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            
        }
        public ActionResult InsertItem(string cat,int qoh)
        {
            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO INVENTORY(inv_cat,inv_qoh)" +
                        "VALUES(@cat,@qoh)";
                    cmd.Parameters.AddWithValue("@cat", cat);
                    cmd.Parameters.AddWithValue("@qoh", qoh);
                    var ctr1 = cmd.ExecuteNonQuery();
                    if (ctr1 > 0)
                    {
                        return Json(new { success = true, message = "Inserted Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, message = "Unsuccessfull!" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
           
        }
    }
}