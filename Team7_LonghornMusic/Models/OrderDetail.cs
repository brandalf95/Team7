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

        [Display(Name = "Is it a gift?")]
        public bool IsGift { get; set; }

        [Display(Name = "Is it Confirmed?")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Is it Refunded?")]
        public bool IsRefunded { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
     

    }
}