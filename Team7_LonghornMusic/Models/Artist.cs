using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class Artist
    {
        public Int32 ArtistID { get; set; }

        [Display(Name = "Artist Name")]
        [Required(ErrorMessage = "Artist Name is required")]
        public String ArtistName { get; set; }

        [Display(Name = "Featured")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Artist's Genres")]
        public virtual List<Genre> ArtistGenres { get; set; }

        [Display(Name = "Artist's Albums")]
        public virtual List<Album> ArtistAlbums { get; set; }

        [Display(Name = "Artist's Songs")]
        public virtual List<Song> ArtistSongs { get; set; }

        [Display(Name = "Artist's Reviews")]
        public virtual List<ArtistReview> ArtistReviews { get; set; }
        
        public Artist()
        {
            if(this.ArtistGenres == null)
            {
                this.ArtistGenres = new List<Genre>();
            }
        }

    }
}