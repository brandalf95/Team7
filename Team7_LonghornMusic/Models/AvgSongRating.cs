using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class AvgSongRating
    {
        public decimal AvgRating { get; set; }
        public decimal SavingsAmount { get; set; }

        public virtual Song Song { get; set; }
    }
}