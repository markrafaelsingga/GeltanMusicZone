using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class viewRequisition
    {
        public int CanvasID { get; set; }
        public string CanvasItem { get; set; }
        public string CanvasDesc { get; set; }
        public int CanvasQuantity { get; set; }
        public string CanvasUnit { get; set; }
        public decimal CanvasPrice { get; set; }
        public decimal CanvasTotal { get; set; }
        public int ItemDelete { get; set; } //ID to delete
        public int ItemEdit_ID { get; set; } //ID to edit
        public int EstimateTotal { get; set; } //Total estimate cost
        public int ItemEdit_Qty { get; set; } //Quantity to edit
        public string ItemEdit_Unit { get; set; } //Unit to edit
        public decimal ItemEdit_Total { get; set; } //Total cost to edit

    }

    public class EditItemViewModel
    {
        public int Canvas_FormID { get; set; }
        public int CanvasID { get; set; }
        public string CanvasItem { get; set; }
        public string CanvasDesc { get; set; }
        public int CanvasQuantity { get; set; }
        public string CanvasUnit { get; set; }
        public decimal CanvasPrice { get; set; }
        public decimal CanvasTotal { get; set; }
    }

    public class ViewRequisitionForm
    {
        public int RF_ItemID { get; set; }
        public int RF_ID { get; set; }
        public string RF_Status { get; set; }
        public string RF_Code { get; set; }
        public string RF_Daterequested { get; set; }
        public string RF_Itemcode { get; set; }
        public string RF_Suppliercompany { get; set; }
        public int RF_SupplierID { get; set; }
        public string RF_Item { get; set; }
        public string RF_Description { get; set; }
        public int RF_Quantity { get; set; }
        public string RF_Unit { get; set; }
        public decimal RF_Price { get; set; }
        public decimal RF_Total { get; set; }
        public decimal RF_Estimatecost { get; set; }
        public int Cancel { get; set; }
        public int Restore { get; set; }
        public int Edit { get; set; }

    }
}
