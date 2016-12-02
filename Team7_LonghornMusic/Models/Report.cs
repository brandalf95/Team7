using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class Report
    {
        public decimal TotalPurchases { get; set; }
        public decimal Revenue { get; set; }
        

        public virtual Discount Discount { get; set; }

    }
}