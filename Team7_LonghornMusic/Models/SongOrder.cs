using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class SongOrder
    {
        public Int32 SongOrderID { get; set; }

        public Int32 TrackNumber { get; set; }

        public virtual Album Album { get; set; }

        public virtual Song Song { get; set; }
    }
}