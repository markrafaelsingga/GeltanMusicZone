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
    public class AdminRequisitionController : Controller
    {
        string mainconn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Mark\source\repos\InstrumentShop\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        // GET: AdminRequisition
        public ActionResult Requisition()
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] where rf_status != 'Deleted' and rf_status != 'Cancelled'";

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

                    Session["RequisitionForm"] = lemp;

                    DeleteCanvasItem();
                    return View(lemp);
                }
            }
        }

        public ActionResult ViewRequisition(int request_ID, string message)
        {
            ViewBag.Message = message;
            List<ViewRequisitionForm> details = new List<ViewRequisitionForm>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                int user = UserDetails(db, request_ID);

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
                                                RF_Item = reader2["prod_name"].ToString(),
                                                RF_Description = reader2["prod_desc"].ToString(),
                                                RF_Quantity = Convert.ToInt32(reader2["ri_quantity"]),
                                                RF_Unit = reader2["ri_unit"].ToString(),
                                                RF_Price = Convert.ToDecimal(reader2["prod_price"]),
                                                RF_Total = Convert.ToDecimal(reader2["ri_total"]),
                                                RF_Estimatecost = Convert.ToDecimal(reader2["rf_estimated_cost"]),
                                            };

                                            // Retrieve user details
                                            using (var cmdUser = db.CreateCommand())
                                            {
                                                cmdUser.CommandType = CommandType.Text;
                                                cmdUser.CommandText = "SELECT * FROM users where user_id = @id";
                                                cmdUser.Parameters.AddWithValue("@id", user);

                                                using (SqlDataReader reader3 = cmdUser.ExecuteReader())
                                                {
                                                    while (reader3.Read())
                                                    {
                                                        list.RF_Requestor = reader3["user_fname"].ToString() + " " +
                                                            reader3["user_mi"].ToString() + ". " +
                                                            reader3["user_lname"].ToString();
                                                    }
                                                }
                                            }

                                            details.Add(list);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                List<originalData> originalDataList = RetrieveOriginalViewRequisitionData(db, request_ID);
                Session["Items"] = originalDataList;
            }
            return View(details);
        }

        public ActionResult DeleteRequisition(int delete_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

                // Get the recent status before updating
                string status = RecentStatus(db, delete_ID);

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition SET rf_status = 'Deleted', rf_recentStatus = @recent WHERE rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", delete_ID);
                    cmd.Parameters.AddWithValue("@recent", status);

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

        public ActionResult RecycleBin()
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] WHERE rf_status = 'Deleted' OR rf_status = 'Cancelled'";

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

                    return View(lemp);
                }
            }
        }
        public ActionResult restoreRequisition(int restore_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                string status = AccessRecentStatus(db, restore_ID);
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition set rf_status = @update, rf_recentStatus = @recent WHERE rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", restore_ID);
                    cmd.Parameters.AddWithValue("@update", status);
                    cmd.Parameters.AddWithValue("@recent", DBNull.Value);

                    // Execute the UPDATE statement.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Redirect to the "RecycleBin" action
                        return RedirectToAction("RecycleBin");
                    }
                    else
                    {
                        // Item not found or no changes were made
                        return View("Error");
                    }
                }
            }
        }
        public ActionResult editRequisition(int edit_ID)
        {
            Session["edit_ID"] = edit_ID;
            List<ViewRequisitionForm> details = new List<ViewRequisitionForm>();
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM supplier s " +
                        "JOIN product p ON s.sup_id = p.sup_id " +
                        "JOIN canvas c ON p.prod_id = c.prod_id " +
                        "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                        "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                        "WHERE rf.rf_id = @id";

                    cmd.Parameters.AddWithValue("@id", edit_ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRequisitionForm list = new ViewRequisitionForm
                            {
                                RF_ItemStatus = reader["ri_status"].ToString(),
                                Request_Item = Convert.ToInt32(reader["ri_id"]),
                                RF_ItemID = Convert.ToInt32(reader["canvas_id"]),
                                RF_ID = Convert.ToInt32(reader["rf_id"]),
                                RF_Status = reader["rf_status"].ToString(),
                                RF_Code = reader["rf_code"].ToString(),
                                RF_Daterequested = reader["rf_date_requested"].ToString(),
                                RF_Itemcode = reader["ri_code"].ToString(),
                                RF_Item = reader["prod_name"].ToString(),
                                RF_Description = reader["prod_desc"].ToString(),
                                RF_Quantity = Convert.ToInt32(reader["canvas_quantity"]),
                                RF_Unit = reader["canvas_unit"].ToString(),
                                RF_Price = Convert.ToDecimal(reader["prod_price"]),
                                RF_Total = Convert.ToDecimal(reader["canvas_total"]),
                                RF_Estimatecost = Convert.ToDecimal(reader["rf_estimated_cost"]),
                            };

                            details.Add(list);
                        }
                    }

                }

                using (var cmd2 = db.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "SELECT * FROM canvas c JOIN product p ON c.prod_id = p.prod_id JOIN supplier s ON s.sup_id = p.sup_id where canvas_status = 0";
                    using (SqlDataReader reader = cmd2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRequisitionForm list = new ViewRequisitionForm
                            {
                                RF_ItemStatus = "Canvas",
                                RF_ItemID = Convert.ToInt32(reader["canvas_id"]),
                                RF_Item = reader["prod_name"].ToString(),
                                RF_Description = reader["prod_desc"].ToString(),
                                RF_Quantity = Convert.ToInt32(reader["canvas_quantity"]),
                                RF_Unit = reader["canvas_unit"].ToString(),
                                RF_Price = Convert.ToDecimal(reader["prod_price"]),
                                RF_Total = Convert.ToDecimal(reader["canvas_total"]),
                            };

                            details.Add(list);
                        }
                    }
                }
            }
            return View(details);
        }
        public ActionResult addItem(int request_ID)
        {
            List<addItemLists> productList = new List<addItemLists>();

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

                // Fetch product data
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT p.prod_id FROM product p JOIN canvas c ON p.prod_id = c.prod_id " +
                        "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                        "JOIN requisition rf ON rf.rf_id = ri.rf_id WHERE rf.rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", request_ID);
                    List<int> canvasIds = new List<int>();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["prod_id"]);
                            canvasIds.Add(id);
                        }
                    }

                    using (var cmd2 = db.CreateCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "SELECT * FROM [dbo].[product]";

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd2))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                int productId = Convert.ToInt32(dr["prod_id"]);

                                // Check if the product ID is not in canvasIds
                                if (!canvasIds.Contains(productId))
                                {
                                    addItemLists product = new addItemLists
                                    {
                                        ReqItem_ID = productId,
                                        ReqItem_Item = dr["prod_name"].ToString(),
                                        ReqItem_Desc = dr["prod_desc"].ToString(),
                                        ReqItem_Price = Convert.ToDecimal(dr["prod_price"]),
                                    };
                                    productList.Add(product);
                                }
                            }
                        }
                    }
                }
            }
            return View(productList);
        }
        [HttpPost]
        public ActionResult Add(addItemLists model)
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

                        // Retrieve the request_ID from the session
                        int request_ID = (int)Session["edit_ID"];
                        return RedirectToAction("editRequisition", new { edit_ID = request_ID });
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

        public ActionResult Cancel()
        {
            int request_ID = (int)Session["edit_ID"];
            return RedirectToAction("editRequisition", new { edit_ID = request_ID });
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

        public ActionResult DeleteItem(int delete_ID, int request_ID)
        {
            Status(delete_ID, "Declined");
            return RedirectToAction("editRequisition", new { edit_ID = request_ID });
        }

        public ActionResult RestoreItem(int delete_ID, int request_ID)
        {
            Status(delete_ID, "Pending");
            return RedirectToAction("editRequisition", new { edit_ID = request_ID });
        }
        public ActionResult EditItem(int edit_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd1 = db.CreateCommand())
                {
                    cmd1.CommandType = CommandType.Text;
                    cmd1.CommandText = "SELECT * FROM canvas c JOIN product p ON c.prod_id = p.prod_id " +
                        "where c.canvas_id = @canId";
                    cmd1.Parameters.AddWithValue("@canId", edit_ID);

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
        public ActionResult Edit(int ItemEdit_ID, int ItemEdit_Qty, string ItemEdit_Unit, decimal ItemEdit_Total)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE canvas SET canvas_quantity = @qty, canvas_unit = @unit, canvas_total = @total WHERE canvas_id = @id;";
                    cmd.Parameters.AddWithValue("@qty", ItemEdit_Qty);
                    cmd.Parameters.AddWithValue("@unit", ItemEdit_Unit);
                    cmd.Parameters.AddWithValue("@total", ItemEdit_Total);
                    cmd.Parameters.AddWithValue("@id", ItemEdit_ID);

                    // Execute the UPDATE statement.
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        int edit_ID = (int)Session["edit_ID"];
                        // Redirect to the "editRequisition" action with the original form's ID
                        return RedirectToAction("editRequisition", new { edit_ID = edit_ID });
                    }
                    else
                    {
                        // Item not found or no changes were made
                        return View("Error");
                    }
                }
            }
        }

        public ActionResult Approval()
        {
            return PartialView("_ApprovalPartialView");
        }

        public ActionResult Decline()
        {
            return PartialView("_DeclinePartialView");
        }
        /*  public ActionResult ApproveRequest(int request_ID, decimal EstimateTotal, string selectedStatus, string ApprovalNote)
          {
              int user = (int)Session["user_id"];
              using (var db = new SqlConnection(mainconn))
              {
                  db.Open();

                  // Update the status and the cost
                  UpdateRF_Status(db, selectedStatus, request_ID);
                  UpdateCost(db, EstimateTotal, request_ID);

                  InsertApproval(db, selectedStatus, ApprovalNote, user, request_ID);

                  ViewBag.Message = "Requisition form approved successfully!";

                  // Redirect to the "ViewRequisition" action
                  return RedirectToAction("ViewRequisition", new { request_ID = request_ID, message = ViewBag.Message });
              }
          }*/

        public ActionResult ApproveRequest(int request_ID, decimal EstimateTotal, string selectedStatus, string ApprovalNote)
        {
            int user = (int)Session["user_id"];
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

                // Update the status and the cost
                UpdateRF_Status(db, selectedStatus, request_ID);
                UpdateCost(db, EstimateTotal, request_ID);
                InsertApproval(db, selectedStatus, ApprovalNote, user, request_ID);

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition_item SET ri_status = @stat WHERE rf_id = @id AND ri_status <> 'Declined'";
                    cmd.Parameters.AddWithValue("@stat", "Approved");
                    cmd.Parameters.AddWithValue("@id", request_ID);

                    cmd.ExecuteNonQuery();
                }

                ViewBag.Message = "Requisition form approved successfully!";

                // Redirect to the "ViewRequisition" action
                return RedirectToAction("ViewRequisition", new { request_ID = request_ID, message = ViewBag.Message });
            }
        }


        public ActionResult DeclineRequest(int request_ID, string selectedStatus, string ApprovalNote)
        {
            int user = (int)Session["user_id"];
            // Retrieve the original requisition data from the session
            var originalRequisitionData = Session["RequisitionForm"] as List<requisitionDetails>;
            var originalDataList = Session["Items"] as List<originalData>;
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                bool isAlreadyDeclined = originalRequisitionData.Any(item => item.rf_id == request_ID && item.rf_status == "Declined");

                if (isAlreadyDeclined)
                {
                    ViewBag.Message = "This requisition form is already declined";

                    // Use the original requisition data to reset changes
                    ResetRequisitionChanges(db, originalRequisitionData);

                    // Use the original view requisition data to reset changes
                    UpdateViewRequisitionData(db, originalDataList);

                    // Delete canvas items
                    DeleteCanvasItem();
                }
                else
                {
                    foreach (var originalData in originalDataList)
                    {
                        UpdateCanvas(db, originalData.origDetail_CanvasID, originalData.origDetail_Quantity, originalData.origDetail_Unit, originalData.origDetail_Total);

                        Status(originalData.origDetail_ID, selectedStatus);
                    }

                    // Delete canvas items
                    DeleteCanvasItem();

                    // Update the status
                    UpdateRF_Status(db, selectedStatus, request_ID);
                    InsertApproval(db, selectedStatus, ApprovalNote, user, request_ID);


                    ViewBag.Message = "Requisition form declined successfully!";
                }

                // Redirect to the "ViewRequisition" action
                return RedirectToAction("ViewRequisition", new { request_ID = request_ID, message = ViewBag.Message });
            }
        }

        public ActionResult CancelAction()
        {
            // Retrieve the original requisition data from the session
            var originalRequisitionData = Session["RequisitionForm"] as List<requisitionDetails>;
            var originalDataList = Session["Items"] as List<originalData>;

            if (originalRequisitionData == null || originalDataList == null)
            {
                // No changes, redirect to Requisition directly
                return RedirectToAction("Requisition");
            }

            using (var db = new SqlConnection(mainconn))
            {
                db.Open();

                // Use the original requisition data to reset changes
                ResetRequisitionChanges(db, originalRequisitionData);

                // Use the original view requisition data to reset changes
                UpdateViewRequisitionData(db, originalDataList);

                // Delete canvas items
                DeleteCanvasItem();

                return RedirectToAction("Requisition");
            }
        }


        private List<originalData> RetrieveOriginalViewRequisitionData(SqlConnection db, int request_ID)
        {
            List<originalData> details = new List<originalData>();

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

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        originalData list = new originalData
                        {
                            origDetail_Status = reader["ri_status"].ToString(),
                            origDetail_FormID = Convert.ToInt32(reader["rf_id"]),
                            origDetail_Quantity = Convert.ToInt32(reader["ri_quantity"]),
                            origDetail_Unit = reader["ri_unit"].ToString(),
                            origDetail_Total = Convert.ToDecimal(reader["ri_total"]),
                            origDetail_CanvasID = Convert.ToInt32(reader["canvas_id"]),
                            origDetail_ID = Convert.ToInt32(reader["ri_id"]),
                        };

                        details.Add(list);
                    }
                }
            }

            return details;
        }

        private void UpdateViewRequisitionData(SqlConnection db, List<originalData> originalDataList)
        {
            foreach (var originalData in originalDataList)
            {
                UpdateCanvas(db, originalData.origDetail_CanvasID, originalData.origDetail_Quantity, originalData.origDetail_Unit, originalData.origDetail_Total);

                Status(originalData.origDetail_ID, originalData.origDetail_Status);
            }
        }

        private void ResetRequisitionChanges(SqlConnection db, List<requisitionDetails> originalRequisitionData)
        {
            // Loop through the original requisition data and update the database
            foreach (var originalData in originalRequisitionData)
            {
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [dbo].[requisition] " +
                                      "SET rf_date_requested = @date_requested, " +
                                      "    rf_code = @code, " +
                                      "    rf_status = @status, " +
                                      "    rf_estimated_cost = @estimated_cost " +
                                      "WHERE rf_id = @id";

                    // Add parameters using AddWithValue
                    cmd.Parameters.AddWithValue("@id", originalData.rf_id);
                    cmd.Parameters.AddWithValue("@date_requested", originalData.rf_date_requested);
                    cmd.Parameters.AddWithValue("@code", originalData.rf_code);
                    cmd.Parameters.AddWithValue("@status", originalData.rf_status);
                    cmd.Parameters.AddWithValue("@estimated_cost", originalData.rf_estimated_cost);

                    // Execute the UPDATE statement
                    cmd.ExecuteNonQuery();
                }
            }

            // Clear the session after resetting
            Session["RequisitionForm"] = null;
        }

        private void InsertRequisitionItem(SqlConnection db, int canvas, int quantity, string unit, decimal total, int rfForm, string status)
        {
            // Insert into [dbo].[requisition_item]
            using (var insertCmd = db.CreateCommand())
            {
                insertCmd.CommandType = CommandType.Text;
                insertCmd.CommandText = "INSERT INTO [dbo].[requisition_item] (ri_status, ri_quantity, ri_unit, ri_total, canvas_id, rf_id) " +
                                            "VALUES (@stat, @qty, @unit, @total, @id, @rf)";
                insertCmd.Parameters.AddWithValue("@stat", status);
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

        public ActionResult DeleteCanvas(int delete_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM [dbo].[canvas] WHERE canvas_status = 0 and canvas_id = @id";
                    cmd.Parameters.AddWithValue("@id", delete_ID);

                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
            int request_ID = (int)Session["edit_ID"];
            return RedirectToAction("editRequisition", new { edit_ID = request_ID });
        }
        public void InsertApproval(SqlConnection db, string status, string note, int userID, int formID)
        {
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;

                // Use a single INSERT statement with conditional parameter inclusion
                cmd.CommandText = "INSERT INTO ApprovalStatus (approval_status, approval_note, user_id, rf_id) VALUES (@status, @note, @user, @form)";

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@user", userID);
                cmd.Parameters.AddWithValue("@form", formID);

                // Add the note parameter only if it is not null
                if (note != null)
                {
                    cmd.Parameters.AddWithValue("@note", note);
                }
                else
                {
                    // If note is null, set it to DBNull.Value
                    cmd.Parameters.AddWithValue("@note", DBNull.Value);
                }

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCost(SqlConnection db, decimal EstimateTotal, int request_ID)
        {
            using (var cmd2 = db.CreateCommand())
            {
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = "UPDATE requisition SET rf_estimated_cost = @cost WHERE rf_id = @id";
                cmd2.Parameters.AddWithValue("@cost", EstimateTotal);
                cmd2.Parameters.AddWithValue("@id", request_ID);

                cmd2.ExecuteNonQuery();
            }
        }
        public void InsertNewItem(SqlConnection db, int request_ID, string selectedStatus)
        {
            //New item iserted by the admin
            using (var cmd1 = db.CreateCommand())
            {
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "SELECT * FROM canvas WHERE canvas_status = 0";

                using (SqlDataReader reader = cmd1.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int CanvasID = Convert.ToInt32(reader["canvas_id"]);
                        int CanvasQuantity = Convert.ToInt32(reader["canvas_quantity"]);
                        string CanvasUnit = reader["canvas_unit"].ToString();
                        decimal CanvasTotal = Convert.ToDecimal(reader["canvas_total"]);

                        if (selectedStatus == "Approved")
                        {
                            InsertRequisitionItem(db, CanvasID, CanvasQuantity, CanvasUnit, CanvasTotal, request_ID, "Approved");
                        }
                        else
                        {
                            DeleteCanvasItem();
                        }
                    }
                }
                // Update canvas status
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "UPDATE canvas SET canvas_status = 1 WHERE canvas_status = 0";
                cmd1.ExecuteNonQuery();
            }
        }
        public void UpdateItems(SqlConnection db, int request_ID, string selectedStatus)
        {
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * from canvas c JOIN requisition_item ri ON c.canvas_id = ri.canvas_id where rf_id = @id";
                cmd.Parameters.AddWithValue("@id", request_ID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Updated the records of requisition items if ever there are changes made in canvas
                        int riID = Convert.ToInt32(reader["ri_id"]);
                        int CanvasID = Convert.ToInt32(reader["canvas_id"]);
                        int CanvasQuantity = Convert.ToInt32(reader["canvas_quantity"]);
                        string CanvasUnit = reader["canvas_unit"].ToString();
                        decimal CanvasTotal = Convert.ToDecimal(reader["canvas_total"]);

                        Update(db, CanvasID, CanvasQuantity, CanvasUnit, CanvasTotal);

                        //Update the status of requisition items based on the status of requisition form
                        string status = reader["ri_status"].ToString();
                        if (selectedStatus == "Declined")
                        {
                            Status(riID, "Declined");
                        }
                        else if (selectedStatus == "Approved")
                        {
                            if (status == "Pending")
                            {
                                Status(riID, "Approved");
                            }
                        }

                    }
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

        public void Status(int delete_ID, string status)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition_item set ri_status = @stat where ri_id = @id";
                    cmd.Parameters.AddWithValue("@stat", status);
                    cmd.Parameters.AddWithValue("@id", delete_ID);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UpdateCanvas(SqlConnection db, int id, int qty, string unit, decimal total)
        {
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE canvas SET canvas_quantity = @qty, canvas_unit = @unit, canvas_total = @total WHERE canvas_id = @id;";
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

            }
        }

        public void Update(SqlConnection db, int id, int qty, string unit, decimal total)
        {
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE requisition_item SET ri_quantity = @qty, ri_unit = @unit, ri_total = @total WHERE canvas_id = @id;";
                cmd.Parameters.AddWithValue("@qty", qty);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

            }
        }
        public void UpdateRF_Status(SqlConnection db, string status, int id)
        {
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE requisition SET rf_status = @stat WHERE rf_id = @id;";

                // Add parameters using AddWithValue
                cmd.Parameters.AddWithValue("@stat", status);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

        public string RecentStatus(SqlConnection db, int rfId)
        {
            string recentStatus = null;

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT rf_status FROM [dbo].[requisition] WHERE rf_id = @id";
                cmd.Parameters.AddWithValue("@id", rfId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Assuming rf_status is a string column; adjust the type accordingly
                        recentStatus = reader["rf_status"].ToString();
                    }
                }
            }

            return recentStatus;
        }

        public string AccessRecentStatus(SqlConnection db, int rfId)
        {
            string recentStatus = null;

            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT rf_recentStatus FROM [dbo].[requisition] WHERE rf_id = @id";
                cmd.Parameters.AddWithValue("@id", rfId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Assuming rf_status is a string column; adjust the type accordingly
                        recentStatus = reader["rf_recentStatus"].ToString();
                    }
                }
            }

            return recentStatus;
        }
        public int UserDetails(SqlConnection db, int rfId)
        {
            int user = 0;
            using (var cmd = db.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT user_id FROM [dbo].[requisition] WHERE rf_id = @id";
                cmd.Parameters.AddWithValue("@id", rfId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = Convert.ToInt32(reader["user_id"]);
                    }
                }
            }

            return user;
        }

    }
}