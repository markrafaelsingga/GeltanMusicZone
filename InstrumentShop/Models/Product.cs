using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstrumentShop.Models
{
    public class Product
    {
        public int prodId { get; set; }
        public string prodCode { get; set; }
        public string prodName { get; set; }
        public string prodDesc { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal prodPrice { get; set; }
        public string cat { get; set; }
        public int qoh { get; set; }
    }
}