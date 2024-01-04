using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstrumentShop.Models
{
    public class requisitionModel
    {
        public List<requisitionItemLists> requisitionItemList { get; set; }
        public List<viewRequisition> canvas { get; set; }
    }

    public class adminPageModel
    {
        public List<requisitionDetails> pendingDetails { get; set; }
        public Home homeInfo { get; set; }
    }
}