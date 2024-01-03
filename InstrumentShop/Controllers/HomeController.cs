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
    public class HomeController : Controller
    {
        string mainconn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        public ActionResult AdminPage()
        {
            int id = (int)Session["user_id"];
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "select users.user_phone,users.user_email,users.user_address,users.user_fname,users.user_mi,users.user_lname,user_pic,user_role.role_desc from users join user_role on user_role.role_id = users.role_id where user_id = @id ";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            var model = new Home
                            {
                                fname = reader["user_fname"].ToString(),
                                mi = reader["user_mi"].ToString(),
                                lname = reader["user_lname"].ToString(),
                                role = reader["role_desc"].ToString(),
                                Phone = reader["user_phone"].ToString(),
                                Email = reader["user_email"].ToString(),
                                Address = reader["user_address"].ToString(),
                                uimg = reader["user_pic"].ToString()
                            };
                            ViewBag.uname = $"{model.fname} {model.mi} {model.lname}";
                            Session["uname"] = $"{model.fname} {model.mi} {model.lname}";
                            ViewBag.picture = $"{model.uimg}";
                            Session["images"] = $"{model.uimg}";
                            return View(model);
                        }

                    }
                }
            }

            return View();
        }
        public ActionResult Index()
        {
            DeleteCanvasItem();
            return View();
        }

        public ActionResult CreateNewRequisition()
        {
            try
            {
                List<requisitionItemLists> productList = new List<requisitionItemLists>();
                List<viewRequisition> itemList = new List<viewRequisition>();

                using (var db = new SqlConnection(mainconn))
                {
                    db.Open();

                    // Fetch product data
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [dbo].[product]";

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                requisitionItemLists product = new requisitionItemLists
                                {
                                    ReqItem_ID = Convert.ToInt32(dr["prod_id"]),
                                    ReqItem_Item = dr["prod_name"].ToString(),
                                    ReqItem_Desc = dr["prod_desc"].ToString(),
                                    ReqItem_Price = Convert.ToDecimal(dr["prod_price"]),
                                };
                                productList.Add(product);
                            }
                        }
                    }


                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM canvas c JOIN product p ON c.prod_id = p.prod_id where canvas_status = 0";

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

                                itemList.Add(canvas);
                            }
                        }
                    }
                }

                requisitionModel combine = new requisitionModel
                {
                    requisitionItemList = productList,
                    canvas = itemList
                };

                return View(combine);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"An error occurred while processing your request: {ex.Message}";
                return View("Error");
            }
        }
        public ActionResult CancelledRequest(int page = 1, int pageSize = 6)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] WHERE rf_status like @Cancelled";
                    cmd.Parameters.AddWithValue("@Cancelled", "Cancelled");


                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    List<requisitionDetails> lemp = new List<requisitionDetails>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        requisitionDetails request = new requisitionDetails
                        {
                            // Populate properties based on your database columns
                            rf_id = Convert.ToInt32(dr["rf_id"]),
                            rf_date_requested = dr["rf_date_requested"].ToString(),
                            rf_code = dr["rf_code"].ToString(),
                            rf_status = dr["rf_status"].ToString(),
                            rf_estimated_cost = Convert.ToDecimal(dr["rf_estimated_cost"]),
                        };

                        lemp.Add(request);
                    }

                    db.Close();

                    // Call GetMinDate with a requisitionDetails instance
                    requisitionDetails minDateModel = new requisitionDetails();
                    GetMinDate(minDateModel);

                    // Call GetMaxDate with a requisitionDetails instance
                    requisitionDetails maxDateModel = new requisitionDetails();
                    GetMaxDate(maxDateModel);

                    // Perform pagination logic
                    var paginatedModel = lemp.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalItems = lemp.Count;

                    // Pass the paginated list and minimum date model to the view
                    ViewBag.MinDate = minDateModel.fromRequestdate;
                    ViewBag.MaxDate = maxDateModel.toRequestdate;

                    // Pass the paginated list to the view
                    return View(paginatedModel);
                }
            }
        }

        public ActionResult Requisition(int page = 1, int pageSize = 6)
        {
            DeleteCanvasItem();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] WHERE rf_status not like @Cancelled AND rf_status not like @Deleted";
                    cmd.Parameters.AddWithValue("@Cancelled", "Cancelled");
                    cmd.Parameters.AddWithValue("@Deleted", "Deleted");


                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    List<requisitionDetails> lemp = new List<requisitionDetails>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        requisitionDetails request = new requisitionDetails
                        {
                            // Populate properties based on your database columns
                            rf_id = Convert.ToInt32(dr["rf_id"]),
                            rf_date_requested = dr["rf_date_requested"].ToString(),
                            rf_code = dr["rf_code"].ToString(),
                            rf_status = dr["rf_status"].ToString(),
                            rf_estimated_cost = Convert.ToDecimal(dr["rf_estimated_cost"]),
                        };

                        lemp.Add(request);
                    }

                    db.Close();

                    // Call GetMinDate with a requisitionDetails instance
                    requisitionDetails minDateModel = new requisitionDetails();
                    GetMinDate(minDateModel);

                    // Call GetMaxDate with a requisitionDetails instance
                    requisitionDetails maxDateModel = new requisitionDetails();
                    GetMaxDate(maxDateModel);

                    // Perform pagination logic
                    var paginatedModel = lemp.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalItems = lemp.Count;

                    // Pass the paginated list and minimum date model to the view
                    ViewBag.MinDate = minDateModel.fromRequestdate;
                    ViewBag.MaxDate = maxDateModel.toRequestdate;

                    // Pass the paginated list to the view
                    return View(paginatedModel);
                }
            }
        }
        public void DeleteCanvasItem()
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM [dbo].[canvas] WHERE canvas_status = 0";

                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }

        private void GetMinDate(requisitionDetails model)
        {
            model.fromRequestdate = DateTime.MinValue; // Initialize with a default value

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT MIN(rf_date_requested) FROM requisition";


                    // Execute the command and retrieve the result
                    object result = cmd.ExecuteScalar();

                    // Check if the result is not DBNull and update fromRequestdate
                    if (result != DBNull.Value)
                    {
                        model.fromRequestdate = (DateTime)result;
                    }

                }
                db.Close();
            }
        }

        private void GetMaxDate(requisitionDetails model)
        {
            model.toRequestdate = DateTime.MaxValue; // Initialize with a default value

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT MAX(rf_date_requested) FROM requisition";


                    // Execute the command and retrieve the result
                    object result = cmd.ExecuteScalar();

                    // Check if the result is not DBNull and update minDate
                    if (result != DBNull.Value)
                    {
                        model.toRequestdate = (DateTime)result;
                    }

                }
                db.Close();
            }
        }


        private void InsertCanvasRecord(SqlConnection connection, SqlTransaction transaction, int quantity, decimal total, int selectedProduct, string unit)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT INTO [dbo].[canvas] (canvas_quantity, canvas_status, canvas_total, prod_id, canvas_unit) VALUES (@quantity, @stat, @total, @product, @unit)";

                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@stat", 0);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@product", selectedProduct);
                command.Parameters.AddWithValue("@unit", unit);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected < 1)
                {
                    throw new Exception("INSERT operation unsuccessful.");
                }
            }
        }
        [HttpPost]
        public ActionResult AddRequisition(requisitionItemLists model)
        {
            int product = model.ReqItem_Prod;
            int quantity = model.ReqItem_Qt;
            var totalValue = model.ReqItem_Total;
            string unit = model.ReqItem_Unit;
            using (var connection = new SqlConnection(mainconn))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        InsertCanvasRecord(connection, transaction, quantity, totalValue, product, unit);
                        transaction.Commit();
                        return RedirectToAction("CreateNewRequisition");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";
                        return View("Index");
                    }
                }
            }
        }
        public ActionResult Supplier(int page = 1, int pageSize = 6)
        {
            DeleteCanvasItem();
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
                            // Populate properties based on your database columns
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

                    // Perform pagination logic
                    var paginatedSuppliers = suppliers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                    ViewBag.PageNumber = page;
                    ViewBag.PageSize = pageSize;
                    ViewBag.TotalItems = suppliers.Count;

                    // Pass the paginated list to the view
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
                            // Close the previous reader
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

                                    supplierReader.Close();  // Close the supplierReader

                                    // Create a list to store product details
                                    List<productLists> productList = new List<productLists>();

                                    // Fetch all products associated with the supplier
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

                                        // Close the productReader
                                        productReader.Close();

                                        // Pass both objects to the view
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
                        return View("Error"); // You might want to create an error view
                    }
                    finally
                    {
                        // Close the connection in a finally block to ensure it's closed even in case of an exception
                        db.Close();
                    }
                }

                // Handle the case where the supplier with the given supId is not found
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult DeleteItem(int ItemDelete)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM canvas WHERE canvas_id = @canvas_Item";
                    cmd.Parameters.AddWithValue("@canvas_Item", ItemDelete);

                    try
                    {
                        // Execute the DELETE statement.
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected to determine if the deletion was successful.
                        if (rowsAffected > 0)
                        {
                            // Redirect to the "CreateNewRequisition" action
                            return RedirectToAction("CreateNewRequisition");
                        }
                        else
                        {
                            return View("Supplier");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);

                        // Return an appropriate view or handle the error
                        return View("Index");
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult EditItem(int ItemEdit_ID, int ItemEdit_Qty, string ItemEdit_Unit, decimal ItemEdit_Total)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE canvas SET canvas_quantity = @c_qty, canvas_unit = @c_unit, canvas_total = @c_total WHERE canvas_id = @id;";
                    cmd.Parameters.AddWithValue("@c_qty", ItemEdit_Qty);
                    cmd.Parameters.AddWithValue("@c_unit", ItemEdit_Unit);
                    cmd.Parameters.AddWithValue("@c_total", ItemEdit_Total);
                    cmd.Parameters.AddWithValue("@id", ItemEdit_ID);

                    // Execute the UPDATE statement.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Redirect to the "CreateNewRequisition" action
                        return RedirectToAction("CreateNewRequisition");
                    }
                    else
                    {
                        // Item not found or no changes were made
                        return View("Error");
                    }
                }
            }
        }

        public ActionResult EditItemView(int ItemEdit_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd1 = db.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT * FROM canvas c JOIN product p ON c.prod_id = p.prod_id where canvas_id = @canId";
                    cmd1.Parameters.AddWithValue("@canId", ItemEdit_ID);

                    SqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        EditItemViewModel item = new EditItemViewModel
                        {
                            CanvasID = Convert.ToInt32(reader["canvas_id"]),
                            CanvasItem = reader["prod_name"].ToString(),
                            CanvasDesc = reader["prod_desc"].ToString(),
                            CanvasQuantity = Convert.ToInt32(reader["canvas_quantity"]),
                            CanvasUnit = reader["canvas_unit"].ToString(),
                            CanvasPrice = Convert.ToDecimal(reader["prod_price"]),
                            CanvasTotal = Convert.ToDecimal(reader["canvas_total"]),
                        };

                        return View(item);
                    }
                    else
                    {
                        return View("Index");
                    }
                }
            }
        }
        private void InsertRequisitionItem(SqlConnection db, int canvas, int quantity, string unit, decimal total)
        {
            // Retrieve last inserted ID
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT TOP 1 rf_id FROM requisition ORDER BY rf_id DESC";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int rfForm = Convert.ToInt32(reader["rf_id"]);
                        TempData["LatestInsert"] = rfForm;

                        // Close the SqlDataReader before executing the inner command
                        reader.Close();

                        // Insert into [dbo].[requisition_item]
                        using (var insertCmd = db.CreateCommand())
                        {
                            insertCmd.CommandType = CommandType.Text;
                            insertCmd.CommandText = "INSERT INTO [dbo].[requisition_item] (ri_status, ri_quantity, ri_unit, ri_total, canvas_id, rf_id) " +
                                                        "VALUES ('Pending', @qty, @unit, @total, @id, @rf)";
                            insertCmd.Parameters.AddWithValue("@qty", quantity);
                            insertCmd.Parameters.AddWithValue("@unit", unit);
                            insertCmd.Parameters.AddWithValue("@total", total);
                            insertCmd.Parameters.AddWithValue("@id", canvas);
                            insertCmd.Parameters.AddWithValue("@rf", rfForm);

                            int rowsAffected = insertCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("Requisition item inserted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("No rows affected. Requisition item not inserted.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data retrieved from the requisition table.");
                    }
                }
            }
        }
        public ActionResult SubmitDataRF(decimal EstimateTotal)
        {
            int user = (int)Session["user_id"];
            try
            {
                using (var db = new SqlConnection(mainconn))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO [dbo].[requisition] (rf_date_requested, rf_status, rf_estimated_cost, user_id, dep_id) " +
                            "VALUES (GETDATE(), 'Pending', @estimate, @user, 1)";
                        cmd.Parameters.AddWithValue("@estimate", EstimateTotal);
                        cmd.Parameters.AddWithValue("@user", user);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {

                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "SELECT * FROM canvas WHERE canvas_status = 0";

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int CanvasID = Convert.ToInt32(reader["canvas_id"]);
                                    int CanvasQuantity = Convert.ToInt32(reader["canvas_quantity"]);
                                    string CanvasUnit = reader["canvas_unit"].ToString();
                                    decimal CanvasTotal = Convert.ToDecimal(reader["canvas_total"]);

                                    InsertRequisitionItem(db, CanvasID, CanvasQuantity, CanvasUnit, CanvasTotal);
                                }
                            }

                            // Update canvas status
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "UPDATE canvas SET canvas_status = 1 WHERE canvas_status = 0";
                            cmd.ExecuteNonQuery();

                            return RedirectToAction("RequisitionForm");
                        }
                        else
                        {
                            // No row were inserted
                            return View("Index", "No row were inserted");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return View("Error", ex.Message);
            }
        }


        public ActionResult RequisitionForm()
        {
            int rfId = Convert.ToInt32(TempData["LatestInsert"]);
            List<ViewRequisitionForm> view = new List<ViewRequisitionForm>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT rf.rf_id, rf.rf_status, rf.rf_code, rf.rf_date_requested, ri.ri_code, s.sup_id, s.sup_company, p.prod_name, " +
                        "p.prod_desc, ri.ri_quantity, ri.ri_unit, p.prod_price, ri.ri_total, rf.rf_estimated_cost " +
                        "FROM supplier s " +
                        "JOIN product p ON s.sup_id = p.sup_id " +
                        "JOIN canvas c ON p.prod_id = c.prod_id " +
                        "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                        "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                        "WHERE rf.rf_id = @id";

                    cmd.Parameters.AddWithValue("@id", rfId);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRequisitionForm list = new ViewRequisitionForm
                            {
                                RF_ID = Convert.ToInt32(reader["rf_id"]),
                                RF_Status = reader["rf_status"].ToString(),
                                RF_Code = reader["rf_code"].ToString(),
                                RF_Daterequested = reader["rf_date_requested"].ToString(),
                                RF_Itemcode = reader["ri_code"].ToString(),
                                RF_SupplierID = Convert.ToInt32(reader["sup_id"]),
                                RF_Suppliercompany = reader["sup_company"].ToString(),
                                RF_Item = reader["prod_name"].ToString(),
                                RF_Description = reader["prod_desc"].ToString(),
                                RF_Quantity = Convert.ToInt32(reader["ri_quantity"]),
                                RF_Unit = reader["ri_unit"].ToString(),
                                RF_Price = Convert.ToDecimal(reader["prod_price"]),
                                RF_Total = Convert.ToDecimal(reader["ri_total"]),
                                RF_Estimatecost = Convert.ToDecimal(reader["rf_estimated_cost"]),
                            };

                            view.Add(list);
                        }
                    }

                }
            }
            return View(view);
        }
        public ActionResult RequisitionDetails(int request_ID)
        {
            List<ViewRequisitionForm> details = new List<ViewRequisitionForm>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd1 = db.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT rf_status from requisition where rf_id = @rf";
                    cmd1.Parameters.AddWithValue("@rf", request_ID);

                    using (SqlDataReader reader = cmd1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string status = reader["rf_status"].ToString();

                            if (status == "Approved" || status == "Declined")
                            {
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM supplier s " +
                                          "JOIN product p ON s.sup_id = p.sup_id " +
                                          "JOIN canvas c ON p.prod_id = c.prod_id " +
                                          "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                                          "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                                          "JOIN ApprovalStatus a ON rf.rf_id = a.rf_id " +
                                          "JOIN users u ON a.user_id = u.user_id " +
                                          "WHERE rf.rf_id = @id ORDER BY a.approval_date DESC";


                                    cmd.Parameters.AddWithValue("@id", request_ID);

                                    using (SqlDataReader reader1 = cmd.ExecuteReader())
                                    {
                                        while (reader1.Read())
                                        {
                                            ViewRequisitionForm list = new ViewRequisitionForm
                                            {
                                                approvalName = reader1["user_fname"].ToString() + " " +
                                                           reader1["user_mi"].ToString() + ". " +
                                                           reader1["user_lname"].ToString(),
                                                approvalNote = reader1["approval_note"].ToString(),
                                                approvalDate = reader1["approval_date"].ToString(),


                                                RF_ItemStatus = reader1["ri_status"].ToString(),
                                                RF_ID = Convert.ToInt32(reader1["rf_id"]),
                                                RF_Status = reader1["rf_status"].ToString(),
                                                RF_Code = reader1["rf_code"].ToString(),
                                                RF_Daterequested = reader1["rf_date_requested"].ToString(),
                                                RF_Itemcode = reader1["ri_code"].ToString(),
                                                RF_SupplierID = Convert.ToInt32(reader1["sup_id"]),
                                                RF_Suppliercompany = reader1["sup_company"].ToString(),
                                                RF_Item = reader1["prod_name"].ToString(),
                                                RF_Description = reader1["prod_desc"].ToString(),
                                                RF_Quantity = Convert.ToInt32(reader1["ri_quantity"]),
                                                RF_Unit = reader1["ri_unit"].ToString(),
                                                RF_Price = Convert.ToDecimal(reader1["prod_price"]),
                                                RF_Total = Convert.ToDecimal(reader1["ri_total"]),
                                                RF_Estimatecost = Convert.ToDecimal(reader1["rf_estimated_cost"]),
                                            };

                                            details.Add(list);
                                        }
                                    }

                                }
                            }
                            else
                            {
                                using (var cmd = db.CreateCommand())
                                {
                                    cmd.CommandType = CommandType.Text;
                                    cmd.CommandText = "SELECT * FROM supplier s " +
                                        "JOIN product p ON s.sup_id = p.sup_id " +
                                        "JOIN canvas c ON p.prod_id = c.prod_id " +
                                        "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                                        "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                                        "WHERE rf.rf_id = @id";

                                    cmd.Parameters.AddWithValue("@id", request_ID);

                                    using (SqlDataReader reader2 = cmd.ExecuteReader())
                                    {
                                        while (reader2.Read())
                                        {
                                            ViewRequisitionForm list = new ViewRequisitionForm
                                            {
                                                RF_ItemStatus = reader2["ri_status"].ToString(),
                                                RF_ID = Convert.ToInt32(reader2["rf_id"]),
                                                RF_Status = reader2["rf_status"].ToString(),
                                                RF_Code = reader2["rf_code"].ToString(),
                                                RF_Daterequested = reader2["rf_date_requested"].ToString(),
                                                RF_Itemcode = reader2["ri_code"].ToString(),
                                                RF_SupplierID = Convert.ToInt32(reader2["sup_id"]),
                                                RF_Suppliercompany = reader2["sup_company"].ToString(),
                                                RF_Item = reader2["prod_name"].ToString(),
                                                RF_Description = reader2["prod_desc"].ToString(),
                                                RF_Quantity = Convert.ToInt32(reader2["ri_quantity"]),
                                                RF_Unit = reader2["ri_unit"].ToString(),
                                                RF_Price = Convert.ToDecimal(reader2["prod_price"]),
                                                RF_Total = Convert.ToDecimal(reader2["ri_total"]),
                                                RF_Estimatecost = Convert.ToDecimal(reader2["rf_estimated_cost"]),
                                            };

                                            details.Add(list);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return View(details);
        }

        public ActionResult CancelRequisition(int Cancel)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition set rf_status = 'Cancelled' where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", Cancel);

                    // Execute the UPDATE statement.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Redirect to the "Requisition" action
                        return RedirectToAction("Requisition");
                    }
                    else
                    {
                        // Item not found or no changes were made
                        return View("Error");
                    }
                }
            }
        }
        public ActionResult restoreRequisition(int Restore)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition set rf_status = 'Pending' where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", Restore);

                    // Execute the UPDATE statement.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Redirect to the "Requisition" action
                        return RedirectToAction("Requisition");
                    }
                    else
                    {
                        // Item not found or no changes were made
                        return View("Error");
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult SearchSupplier(string search)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[supplier] WHERE sup_company LIKE '%' + @key + '%'";
                    cmd.Parameters.AddWithValue("@key", search);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    List<supplierDetails> lemp = new List<supplierDetails>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        supplierDetails supplier = new supplierDetails
                        {
                            // Populate properties based on your database columns
                            sup_id = Convert.ToInt32(dr["sup_id"]),
                            sup_company = dr["sup_company"].ToString(),
                            sup_address = dr["sup_address"].ToString(),
                            sup_lname = dr["sup_lname"].ToString(),
                            sup_fname = dr["sup_fname"].ToString(),
                            sup_mi = dr["sup_mi"].ToString(),
                            sup_email = dr["sup_email"].ToString(),
                            sup_phone = dr["sup_phone"].ToString(),
                        };

                        lemp.Add(supplier);
                    }

                    db.Close();

                    // Pass the list to the view
                    return View("Supplier", lemp);
                }
            }
        }

        public ActionResult SearchRequisition(string search)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] where rf_status != 'Cancelled' and rf_date_requested LIKE '%' + @key + '%'";
                    cmd.Parameters.AddWithValue("@key", search);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    List<requisitionDetails> lemp = new List<requisitionDetails>();

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        requisitionDetails supplier = new requisitionDetails
                        {
                            // Populate properties based on your database columns
                            rf_id = Convert.ToInt32(dr["rf_id"]),
                            rf_date_requested = dr["rf_date_requested"].ToString(),
                            rf_code = dr["rf_code"].ToString(),
                            rf_status = dr["rf_status"].ToString(),
                            rf_estimated_cost = Convert.ToDecimal(dr["rf_estimated_cost"]),
                        };

                        lemp.Add(supplier);
                    }

                    db.Close();

                    // Pass the list to the view
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
                    cmd.CommandText = "SELECT USERS.USER_FNAME,USERS.USER_MI,USERS.USER_LNAME,USERS.USER_ADDRESS,USERS.USER_EMAIL,USERS.USER_DOB,USERS.USER_PIC,USERS.USER_PHONE,USER_ROLE.ROLE_DESC,DEPARTMENT.DEP_NAME FROM USERS JOIN USER_ROLE ON USER_ROLE.ROLE_ID = USERS.ROLE_ID JOIN DEPARTMENT ON DEPARTMENT.DEP_ID = USERS.DEP_ID WHERE USERS.USER_ID = @id";
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
                                uimg = reader["USER_PIC"].ToString()
                            };
                            ViewBag.fullname = model.fname + " " + model.mi + " " + model.lname;
                            ViewBag.address = model.address;
                            ViewBag.email = model.email;
                            ViewBag.dob = model.dob;
                            ViewBag.contact = model.phone;
                            ViewBag.picture = model.uimg;
                            return View(model);
                        }
                    }
                }
            }

            return View();
        }

    }
}