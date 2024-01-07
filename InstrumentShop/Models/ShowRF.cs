using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class ShowRF
    {
        public string rfCode { get; set; }
        public string rfDate { get; set; }
        public string rfStatus { get; set; }
        public decimal rfCost { get; set; }
        public int userId { get; set; }
        public int depId { get; set; }
    }
}