using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InstrumentShop.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace InstrumentShop.Controllers
{
    public class PurchaseController : Controller
    {

        string mainconn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        // GET: Purchase
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewPurchase()
        {
            List<requisitionDetails> lemp = new List<requisitionDetails>();

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

               
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] WHERE rf_recentStatus like @Approve ";
                    cmd.Parameters.AddWithValue("@Approve", "Approved");

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            requisitionDetails rf = new requisitionDetails
                            {
                                rf_id = Convert.ToInt32(dr["rf_id"]),
                                rf_date_requested = dr["rf_date_requested"].ToString(),
                                rf_code = dr["rf_code"].ToString(),
                                rf_status = dr["rf_status"].ToString(),
                                rf_estimated_cost = Convert.ToDecimal(dr["rf_estimated_cost"]),
                            };
                            lemp.Add(rf);
                        }
                    }
                }
            }

          
            return View(lemp);
        }

        [HttpGet]
        public ActionResult GetDistinctSuppliersForRequisition(int requisitionId)
        {
            Session["id"] = requisitionId;
         
            List<string> suppliers = GetDistinctSuppliersFromDatabase(requisitionId);

           
            return Json(suppliers, JsonRequestBehavior.AllowGet);
        }

       
        private List<string> GetDistinctSuppliersFromDatabase(int requisitionId)
        {
            List<string> suppliers = new List<string>();

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT DISTINCT s.sup_company FROM supplier s " +
                                      "JOIN product p ON s.sup_id = p.sup_id " +
                                      "JOIN canvas c ON p.prod_id = c.prod_id " +
                                      "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                                      "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                                      "JOIN ApprovalStatus a ON rf.rf_id = a.rf_id " +
                                      "JOIN users u ON a.user_id = u.user_id " +
                                      "WHERE rf.rf_id = @id";

                    cmd.Parameters.AddWithValue("@id", requisitionId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string supplierCompany = reader["sup_company"].ToString();
                            suppliers.Add(supplierCompany);
                        }
                    }
                }
            }

            return suppliers;
        }

        [HttpGet]
        public ActionResult GetDetailsForSelectedSupplier(string supplier)
        {
           
            var details = GetDetailsFromDatabase(supplier);

           
            return Json(details, JsonRequestBehavior.AllowGet);
        }


        private List<viewRequisition> GetDetailsFromDatabase(string supplier)
        {
            int id = Convert.ToInt32(Session["id"]);

            List<viewRequisition> details = new List<viewRequisition>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM requisition_item ri JOIN canvas c ON ri.canvas_id = c.canvas_id " +
                                      "JOIN product p ON c.prod_id = p.prod_id " +
                                      "JOIN supplier s ON p.sup_id = s.sup_id " +
                                      "WHERE s.sup_company = @company AND ri.rf_id = @id;";
                    cmd.Parameters.AddWithValue("@company", supplier);
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader canvasReader = cmd.ExecuteReader())
                    {
                        while (canvasReader.Read())
                        {
                            viewRequisition canvas = new viewRequisition
                            {
                                CanvasID = Convert.ToInt32(canvasReader["canvas_id"]),
                                CanvasItem = canvasReader["prod_name"].ToString(),
                                CanvasDesc = canvasReader["prod_desc"].ToString(),
                                CanvasQuantity = Convert.ToInt32(canvasReader["canvas_quantity"]),
                                CanvasUnit = canvasReader["canvas_unit"].ToString(),
                                CanvasPrice = Convert.ToDecimal(canvasReader["prod_price"]),
                                CanvasTotal = Convert.ToDecimal(canvasReader["canvas_total"]),
                            };

                            details.Add(canvas);
                        }
                    }
                }
            }

            return details;
        }


        public ActionResult Supplier(int page = 1, int pageSize = 6)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[supplier]";
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    List<supplierDetails> suppliers = new List<supplierDetails>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        supplierDetails supplier = new supplierDetails
                        {
                          
                            sup_id = Convert.ToInt32(dr["sup_id"]),
                            sup_company = dr["sup_company"].ToString(),
                            sup_address = dr["sup_address"].ToString(),
                            sup_lname = dr["sup_lname"].ToString(),
                            sup_fname = dr["sup_fname"].ToString(),
                            sup_mi = dr["sup_mi"].ToString(),
                            sup_email = dr["sup_email"].ToString(),
                            sup_phone = dr["sup_phone"].ToString(),
                        };

                        suppliers.Add(supplier);
                    }

                    db.Close();

                   
                    var paginatedSuppliers = suppliers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalItems = suppliers.Count;

                  
                    return View(paginatedSuppliers);
                }
            }
        }

        public ActionResult SupplierDetails(int supId)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd1 = db.CreateCommand())
                {
                    try
                    {
                        cmd1.CommandType = CommandType.Text;
                        cmd1.CommandText = "SELECT * FROM [dbo].[product] WHERE sup_id = @supId;";
                        cmd1.Parameters.AddWithValue("@supId", supId);

                        SqlDataReader reader = cmd1.ExecuteReader();

                        if (reader.HasRows)
                        {
                          
                            reader.Close();

                            using (var cmd2 = db.CreateCommand())
                            {
                                cmd2.CommandType = CommandType.Text;
                                cmd2.CommandText = "SELECT * FROM [dbo].[supplier] WHERE sup_id = @supId;";
                                cmd2.Parameters.AddWithValue("@supId", supId);

                                SqlDataReader supplierReader = cmd2.ExecuteReader();

                                if (supplierReader.HasRows)
                                {
                                    supplierReader.Read();

                                    supplierDetails supplier = new supplierDetails
                                    {
                                        sup_id = Convert.ToInt32(supplierReader["sup_id"]),
                                        sup_company = supplierReader["sup_company"].ToString(),
                                        sup_address = supplierReader["sup_address"].ToString(),
                                        sup_lname = supplierReader["sup_lname"].ToString(),
                                        sup_fname = supplierReader["sup_fname"].ToString(),
                                        sup_mi = supplierReader["sup_mi"].ToString(),
                                        sup_email = supplierReader["sup_email"].ToString(),
                                        sup_phone = supplierReader["sup_phone"].ToString(),
                                    };

                                    supplierReader.Close();  
                                    List<productLists> productList = new List<productLists>();

                                  
                                    using (var cmd3 = db.CreateCommand())
                                    {
                                        cmd3.CommandType = CommandType.Text;
                                        cmd3.CommandText = "SELECT * FROM [dbo].[product] WHERE sup_id = @supId;";
                                        cmd3.Parameters.AddWithValue("@supId", supId);

                                        SqlDataReader productReader = cmd3.ExecuteReader();

                                        while (productReader.Read())
                                        {
                                            productLists product = new productLists
                                            {
                                                prod_id = Convert.ToInt32(productReader["prod_id"]),
                                                prod_code = productReader["prod_code"].ToString(),
                                                prod_name = productReader["prod_name"].ToString(),
                                                prod_desc = productReader["prod_desc"].ToString(),
                                                prod_price = productReader["prod_price"].ToString(),
                                            };

                                            productList.Add(product);
                                        }


                                        productReader.Close();

                                       
                                        var combinedData = new Tuple<supplierDetails, List<productLists>>(supplier, productList);
                                        return View(combinedData);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        return View("Error"); 
                    }
                    finally
                    {
                        db.Close();
                    }
                }

              
                return HttpNotFound();
            }
        }

        public ActionResult Purchase(PurchaseLists model)
        {
            return View();
        }

        public ActionResult ViewApproveRF()
        {
            int user = (int)Session["user_id"];
            List<PurchaseLists> lemp = new List<PurchaseLists>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM REQUISITION WHERE RF_STATUS LIKE @approve OR RF_RECENTSTATUS like @approve1";
                    cmd.Parameters.AddWithValue("@approve", "Approved");
                    cmd.Parameters.AddWithValue("@approve1", "Approved");
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        PurchaseLists request = new PurchaseLists
                        {
                            rf_id = row.Field<int>("RF_ID"),
                            rf_code = row.Field<string>("RF_CODE")
                            
                        };
                        lemp.Add(request);
                        
                    }

                    db.Close();

                   
                    return View(lemp);
                }
            }
        }

        public ActionResult StaffProfile()
        {

            int id = (int)Session["user_id"];
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT USERS.USER_FNAME,USERS.USER_MI,USERS.USER_LNAME,USERS.USER_ADDRESS,USERS.USER_EMAIL,USERS.USER_DOB,USERS.USER_PHONE,USER_ROLE.ROLE_DESC,DEPARTMENT.DEP_NAME FROM USERS JOIN USER_ROLE ON USER_ROLE.ROLE_ID = USERS.ROLE_ID JOIN DEPARTMENT ON DEPARTMENT.DEP_ID = USERS.DEP_ID WHERE USERS.USER_ID = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var model = new StaffProfile
                            {
                                fname = reader["USER_FNAME"].ToString(),
                                mi = reader["USER_MI"].ToString(),
                                lname = reader["USER_LNAME"].ToString(),
                                address = reader["USER_ADDRESS"].ToString(),
                                email = reader["USER_EMAIL"].ToString(),
                                phone = reader["USER_PHONE"].ToString(),
                                dob = (DateTime)reader["USER_DOB"],
                                department = reader["DEP_NAME"].ToString(),
                                role = reader["ROLE_DESC"].ToString(),
                            };
                            ViewBag.fullname = model.fname + " " + model.mi + " " + model.lname;
                            ViewBag.address = model.address;
                            ViewBag.email = model.email;
                            ViewBag.dob = model.dob;
                            ViewBag.contact = model.phone;
                            return View(model);
                        }
                    }
                }
            }

            return View();
        }

        public ActionResult ShowRF()
        {
            return View();
        }
    }
}