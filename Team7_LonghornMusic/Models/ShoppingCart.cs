using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class ShoppingCart
    {
        public Int32 ShoppingCartID { get; set; }
        [Required]
        public float SubTotal { get; set; }
        [Required]
        public float CartTax { get; set; }
       
        public CardType CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }

        public virtual AppUser User { get; set; }
        public virtual List<Discount> Discounts { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}