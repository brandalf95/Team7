using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class ShoppingCartViewModel
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal SavingsAmount { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}