using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class SongReview
    {
        public Int32 SongReviewID { get; set; }

        [Required]
        public Int32 Rating { get; set; }

        //TODO: Add regex for comment
        public String Comment { get; set; }

        public virtual Song Song { get; set; }
        //TODO: Make sure this is right, plus make sure consistent
        public virtual AppUser User { get; set; }
    }

    public class ArtistReview
    {
        public Int32 ArtistReviewID { get; set; }

        [Required]
        public Int32 Rating { get; set; }

        //TODO: Add regex for comment
        public String Comment { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual AppUser User { get; set; }
    
    }

    public class AlbumReview
    {
        public Int32 AlbumReviewID { get; set; }

        [Required]
        public Int32 Rating { get; set; }

        //TODO: Add regex for comment
        public String Comment { get; set; }

        public virtual Album Album { get; set; }
        public virtual AppUser User { get; set; }
    }
}