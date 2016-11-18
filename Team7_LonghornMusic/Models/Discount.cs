using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Team7_LonghornMusic.Models
{
    public class Discount
    {
        public Int32 DiscountID { get; set; }
        [Required]
        public float DiscountAmt { get; set; }

        [Display(Name = "Song")]
        public virtual Song Song { get; set; }

        [Display(Name = "Album")]
        public virtual Album Album { get; set; }

        [Display(Name = "Shopping Cart Details") ]
        public virtual ShoppingCart ShoppingCart { get; set; }

    }
    
}