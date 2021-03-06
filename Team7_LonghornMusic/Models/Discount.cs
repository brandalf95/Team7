﻿using System;
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
        
        public decimal DiscountAmt { get; set; }

        [Display(Name = "Song")]
        public virtual Song Song { get; set; }

        [Display(Name = "Album")]
        public virtual Album Album { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        


    }
    
}