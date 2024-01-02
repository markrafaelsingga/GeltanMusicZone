using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class addItemLists
    {
        public decimal ReqItem_Total { get; set; } // Total for creating new data
        public int ReqItem_Qt { get; set; } // Quantity for creating new data
        public int ReqItem_Prod { get; set; } // Product ID for creating new data
        public string ReqItem_Unit { get; set; } // Unit for creating new data
        public int request_ID { get; set; }
        public string ReqItem_Item { get; set; } // Dropdown list item
        public string ReqItem_Desc { get; set; } // Dropdown list description
        public decimal ReqItem_Price { get; set; } // Dropdown list price
        public int ReqItem_ID { get; set; } // Dropdown list ID
    }
}