using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class originalData
    {
        //Requisition details
        public int origDetail_ID { get; set; }
        public string origDetail_Status { get; set; }
        public int origDetail_Quantity { get; set; }
        public string origDetail_Unit { get; set; }
        public decimal origDetail_Total { get; set; }
        public int origDetail_CanvasID { get; set; }
        public int origDetail_FormID { get; set; }

    }
}