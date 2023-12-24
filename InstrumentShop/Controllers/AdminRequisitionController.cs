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
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] where rf_status != 'Deleted'";

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
                    cmd.CommandText = "SELECT rf.rf_id, rf.rf_status, rf.rf_code, rf.rf_date_requested, ri.ri_code, s.sup_id, s.sup_company, p.prod_name, " +
                        "p.prod_desc, c.canvas_quantity, c.canvas_unit, p.prod_price, c.canvas_total, rf.rf_estimated_cost " +
                        "FROM supplier s " +
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
                    cmd.CommandText = "SELECT * FROM [dbo].[requisition] where rf_status = 'Deleted'";

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
    }
}