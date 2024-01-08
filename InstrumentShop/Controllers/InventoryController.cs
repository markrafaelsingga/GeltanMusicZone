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
            List<Supplier> suppList = new List<Supplier>();
            List<Product> prodList = new List<Product>();
            List<Product> prodSup = new List<Product>();

            using(var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM SUPPLIER";
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        Supplier supp = new Supplier
                        {
                            suppid = row.Field<int>("SUP_ID"),
                            company = row.Field<string>("SUP_COMPANY")
                        };
                        suppList.Add(supp);
                    }
                    ViewBag.suppList = new SelectList(suppList, "suppid","company");
                }              
            }



            using (var db = new SqlConnection(connString))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
            SELECT
                I.PROD_ID,
                P.PROD_DESC,
                P.PROD_CODE,
                P.PROD_NAME,
                P.PROD_PRICE,
                P.PROD_CAT,
                P.PROD_ISACTIVE,
                I.INV_QOH,
                S.SUP_COMPANY
            FROM
                INVENTORY I
            JOIN
                PRODUCT P ON I.PROD_ID = P.PROD_ID
            LEFT JOIN
                SUPPLIER S ON P.SUP_ID = S.SUP_ID
            ORDER BY P.PROD_ID DESC
        ";

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
                            prodPrice = row.Field<decimal>("PROD_PRICE"),
                            cat = row.Field<string>("PROD_CAT"),
                            qoh = row.Field<int>("INV_QOH"),
                            compName = row.Field<string>("SUP_COMPANY"),
                            status = row.Field<string>("PROD_ISACTIVE")
                        };
                        prodList.Add(prod);
                    }
                }
            }
            return View(prodList);
        }

        public ActionResult AddProduct(string prodName, string prodCat, string prodDesc, decimal prodPrice, int qoh)
        {
            int prodId;

            try
            {


                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO PRODUCT(prod_name, prod_cat, prod_desc, prod_price)" +
                            "VALUES(@prodName, @prodCat, @prodDesc, @prodPrice)";
                        cmd.Parameters.AddWithValue("@prodName", prodName);
                        cmd.Parameters.AddWithValue("@prodCat", prodCat);
                        cmd.Parameters.AddWithValue("@prodDesc", prodDesc);
                        cmd.Parameters.AddWithValue("@prodPrice", prodPrice);


                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr > 0)
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT TOP 1 * FROM product ORDER BY prod_id DESC";

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    prodId = Convert.ToInt32(reader["prod_id"]);
                                }
                                else
                                {
                                    TempData["AlertFailed"] = "No Product ID Detected!";
                                    return RedirectToAction("Inventory");
                                }
                            }

                            cmd.Parameters.Clear();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO INVENTORY(inv_qoh, prod_id)" +
                                "VALUES(@qoh, @prodId)";
                            cmd.Parameters.AddWithValue("@qoh", qoh);
                            cmd.Parameters.AddWithValue("@prodId", prodId);

                            var ctr2 = cmd.ExecuteNonQuery();

                            if (ctr2 > 0)
                            {
                                TempData["AlertMessage"] = "Inserted successfully";
                                return RedirectToAction("Inventory");
                            }
                            else
                            {
                                TempData["AlertMessage"] = "Operation unsuccessful";
                                return RedirectToAction("Inventory");
                            }
                        }
                        else
                        {
                            TempData["AlertFailed"]="Failed to add product!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Inventory");
        }

        public ActionResult updateProduct(string prodId,string prodName,string prodCat,string prodDesc,decimal prodPrice, int qoh, int supId)
        {
            try
            {
                using (var db = new SqlConnection(connString))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE PRODUCT SET PROD_NAME = @PRODNAME,PROD_CAT = @PRODCAT,PROD_DESC = @PRODDESC,PROD_PRICE = @PRODPRICE, SUP_ID = @supId WHERE PROD_ID = @PRODID";
                        cmd.Parameters.AddWithValue("@PRODID", prodId);
                        cmd.Parameters.AddWithValue("@PRODNAME", prodName);
                        cmd.Parameters.AddWithValue("@PRODCAT", prodCat);
                        cmd.Parameters.AddWithValue("@PRODDESC", prodDesc);
                        cmd.Parameters.AddWithValue("@PRODPRICE", prodPrice);
                        cmd.Parameters.AddWithValue("@supId", supId);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE INVENTORY SET INV_QOH = @QOH WHERE PROD_ID = @PRODID";
                            cmd.Parameters.AddWithValue("@PRODID", prodId);
                            cmd.Parameters.AddWithValue("@QOH", qoh);
                            var ctr1 = cmd.ExecuteNonQuery();
                            if (ctr1 > 0)
                            {
                                TempData["AlertMessage"] = "Updated successfully";
                                return RedirectToAction("Inventory");
                            }
                            else
                            {
                                TempData["AlertFailed"] = "Update unsuccess";
                                return RedirectToAction("Inventory");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Inventory");
        }

        public ActionResult DeleteProduct(int prodId)
        {
            try
            {


                using (var db = new SqlConnection(connString))
                {
                    db.Open();

                  
                    bool isAlreadyInactive = false;
                    using (var checkCmd = db.CreateCommand())
                    {
                        checkCmd.CommandType = CommandType.Text;
                        checkCmd.CommandText = "SELECT PROD_ISACTIVE FROM PRODUCT WHERE PROD_ID = @prodId";
                        checkCmd.Parameters.AddWithValue("@prodId", prodId);

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

                 
                    if (!isAlreadyInactive)
                    {
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE PRODUCT SET PROD_ISACTIVE = @status WHERE PROD_ID = @prodId";
                            cmd.Parameters.AddWithValue("@status", "INACTIVE");
                            cmd.Parameters.AddWithValue("@prodId", prodId);
                            var ctr = cmd.ExecuteNonQuery();

                            if (ctr >= 1)
                            {
                                TempData["AlertMessage"] = "Deleted Successful";
                                return RedirectToAction("Inventory");
                            }
                            else
                            {
                                TempData["AlertFailed"] = "Deleted ";
                                return RedirectToAction("Inventory");
                            }
                        }
                    }
                    else
                    {
                        TempData["AlertDelFailed"] = "Product is already Inactive!";
                        return RedirectToAction("Inventory");
                    }
                }
            }catch(Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Inventory");
        }

        public ActionResult ReactivateProduct(int prodId)
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
                        checkCmd.CommandText = "SELECT PROD_ISACTIVE FROM PRODUCT WHERE PROD_ID = @prodId";
                        checkCmd.Parameters.AddWithValue("@prodId", prodId);

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
                            cmd.CommandText = "UPDATE PRODUCT SET PROD_ISACTIVE = @status WHERE PROD_ID = @prodId";
                            cmd.Parameters.AddWithValue("@prodId", prodId);
                            cmd.Parameters.AddWithValue("@status", "ACTIVE");
                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr >= 1)
                            {
                                TempData["AlertMessage"] = "Activated Successfully";
                                return RedirectToAction("Inventory");
                            }
                            else
                            {
                                TempData["AlertFailed"] = "Operation Failed!";
                                return RedirectToAction("Inventory");
                            }
                        }
                    }
                    else
                    {
                        TempData["AlertDelFailed"] = "Product is already activated!";
                        return RedirectToAction("Inventory");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["AlertFailed"] = $"Failed! Error: {ex.Message}";
            }
            return RedirectToAction("Inventory");
        }     
    }
}