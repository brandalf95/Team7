using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class FeaturedViewModel
    {
        public Int32 FeaturedViewModelID { get; set; }

        public virtual List<Song> Songs { get; set; }

        public virtual List<Album> Albums { get; set; }

        public virtual List<Artist> Artists { get; set; }
    }
}