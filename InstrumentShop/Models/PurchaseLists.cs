using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class PurchaseLists
    {
        public int rf_id { get; set; }
        public string rf_code { get; set; }
        public string rf_status { get; set; }
        public string rf_recStatus { get; set; }
        public int ri_quant { get; set; }
        public decimal ri_total { get; set; }
        public int canvasId { get; set; }
        public int supId { get; set; }
        public int prodId {get;set;}



    }
}