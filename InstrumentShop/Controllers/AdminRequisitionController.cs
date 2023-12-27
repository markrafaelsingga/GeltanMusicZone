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
        string mainconn = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Dell\Source\Repos\GeltanMusicZone\InstrumentShop\App_Data\Database1.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";

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

                    return View(lemp);
                }
            }
        }
        public ActionResult ViewRequisition(int request_ID)
        {
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

                    cmd.Parameters.AddWithValue("@id", request_ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRequisitionForm list = new ViewRequisitionForm
                            {
                                RF_ItemStatus = reader["ri_status"].ToString(),
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

                            details.Add(list);
                        }
                    }

                }
            }
            return View(details);
        }
        public ActionResult DeleteRequisition(int delete_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition set rf_status = 'Deleted' where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", delete_ID);

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
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE requisition set rf_status = 'Pending' where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", restore_ID);

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
                                RF_SupplierID = Convert.ToInt32(reader["sup_id"]),
                                RF_Suppliercompany = reader["sup_company"].ToString(),
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

            }
            return View(details);
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
                        "JOIN requisition_item ri ON ri.canvas_id = c.canvas_id " +
                        "JOIN requisition rf ON rf.rf_id = ri.rf_id " +
                        "where c.canvas_id = @canId";
                    cmd1.Parameters.AddWithValue("@canId", edit_ID);

                    SqlDataReader reader = cmd1.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        EditItemViewModel item = new EditItemViewModel
                        {
                            Canvas_FormID = Convert.ToInt32(reader["rf_id"]),
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
        public ActionResult Edit(int ItemEdit_ID, int ItemEdit_Qty, string ItemEdit_Unit, decimal ItemEdit_Total, int edit_ID)
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

        public ActionResult SaveUpdate(int request_ID, decimal EstimateTotal, string selectedStatus)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * from canvas c JOIN requisition_item ri ON c.canvas_id = ri.canvas_id where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", request_ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int riID = Convert.ToInt32(reader["ri_id"]);
                            int CanvasID = Convert.ToInt32(reader["canvas_id"]);
                            int CanvasQuantity = Convert.ToInt32(reader["canvas_quantity"]);
                            string CanvasUnit = reader["canvas_unit"].ToString();
                            decimal CanvasTotal = Convert.ToDecimal(reader["canvas_total"]);
                            string status = reader["ri_status"].ToString();

                            Update(db, CanvasID, CanvasQuantity, CanvasUnit, CanvasTotal);
                            if (selectedStatus == "Declined")
                            {
                                Status(riID, "Declined");
                            }
                            else if (selectedStatus == "Approved")
                            {
                                if(status == "Pending")
                                {
                                    Status(riID, "Approved");
                                }
                            }
                            
                        }
                    }
                }

                using (var cmd2 = db.CreateCommand())
                {
                    cmd2.CommandType = CommandType.Text;
                    cmd2.CommandText = "UPDATE requisition SET rf_estimated_cost = @cost WHERE rf_id = @id";
                    cmd2.Parameters.AddWithValue("@cost", EstimateTotal);
                    cmd2.Parameters.AddWithValue("@id", request_ID);

                    cmd2.ExecuteNonQuery();
                }

                Approve(db, selectedStatus, request_ID);

                return RedirectToAction("ViewRequisition", new { request_ID = request_ID });
            }
        }

        public ActionResult CancelAction(int request_ID)
        {
            using (var db = new SqlConnection(mainconn))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * from canvas c JOIN requisition_item ri ON c.canvas_id = ri.canvas_id where rf_id = @id";
                    cmd.Parameters.AddWithValue("@id", request_ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int riID = Convert.ToInt32(reader["ri_id"]);
                            int CanvasID = Convert.ToInt32(reader["canvas_id"]);
                            int CanvasQuantity = Convert.ToInt32(reader["ri_quantity"]);
                            string CanvasUnit = reader["ri_unit"].ToString();
                            decimal CanvasTotal = Convert.ToDecimal(reader["ri_total"]);

                            Reset(db, CanvasID, CanvasQuantity, CanvasUnit, CanvasTotal);
                            Status(riID, "Pending");
                        }
                    }
                }
                return RedirectToAction("Requisition");
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

        public void Reset(SqlConnection db, int id, int qty, string unit, decimal total)
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
        public void Approve(SqlConnection db, string status, int id)
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

    }
}