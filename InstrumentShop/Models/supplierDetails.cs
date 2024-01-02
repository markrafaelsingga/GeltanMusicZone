using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class supplierDetails
    {
        public int sup_id { get; set; }
        public string sup_company { get; set; }
        public string sup_fname { get; set; }
        public string sup_mi { get; set; }
        public string sup_lname { get; set; }
        public string sup_address { get; set; }
        public string sup_email { get; set; }
        public string sup_phone { get; set; }
        public string search { get; set; }
    }

    public class requisitionDetails
    {
        public int edit_ID { get; set; }
        public int restore_ID { get; set; }
        public int delete_ID { get; set; }
        public int request_ID { get; set; }
        public int rf_id { get; set; }
        public string rf_code { get; set; }
        public string rf_date_requested { get; set; }
        public string rf_status { get; set; }
        public decimal rf_estimated_cost { get; set; }
        public string rf_requestor { get; set; }
        public DateTime fromRequestdate { get; set; }
        public DateTime toRequestdate { get; set; }

    }

    public class DeletedRequisitionInfo
    {
        public int rf_id { get; set; }
        public string rf_date_requested { get; set; }
        public string rf_code { get; set; }
        public string rf_status { get; set; }
        public string rf_recentStatus { get; set; }
        public decimal rf_estimated_cost { get; set; }
    }
}
