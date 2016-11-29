using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class OrderDetail
    {
        public Int32 OrderDetailID { get; set; }

        public String GifterEmail { get; set; }

        public String GifteeEmail { get; set; }

        public String CreditCardType { get; set; }

        public String CreditCardNumber { get; set; }

        [Display(Name = "Is it Confirmed?")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Is it Refunded?")]
        public bool IsRefunded { get; set; }

        public virtual AppUser User { get; set; }

        public virtual List<Discount> Discounts { get; set; }
     
        public OrderDetail()
        {
            if (this.Discounts == null)
            {
                this.Discounts = new List<Discount>();
            }
            if (this.CreditCardNumber == null)
            {
                this.CreditCardNumber = "";
            }
        }
    }
}