using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class AvgAlbumRating
    {
        public decimal AvgRating { get; set; }

        public virtual Album Album { get; set; }
    }
}