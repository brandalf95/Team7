using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Team7_LonghornMusic.Models
{
    public class Album
    {
        public Int32 AlbumID { get; set; }

        [Display(Name = "Album Title")]
        [Required(ErrorMessage = "Album Title is required")]
        public String AlbumTitle { get; set; }

        [Display(Name = "Featured")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Every album must have a price")]
        [DataType(DataType.Currency)]
        public decimal AlbumPrice { get; set; }

        [Display(Name = "Album's Genres")]
        public virtual List<Genre> AlbumGenres { get; set; }

        [Display(Name = "Album's Artists")]
        public virtual List<Artist> AlbumArtists { get; set; }

        [Display(Name = "Album's Songs")]
        public virtual List<Song> AlbumSongs { get; set; }

        [Display(Name = "Album's Reviews")]
        public virtual List<AlbumReview> AlbumReviews { get; set; }

        [Display(Name = "Album's Discount")]
        public virtual List<Discount> AlbumDiscount { get; set; }

        public Album()
        {
            if (this.AlbumGenres == null)
            {
                this.AlbumGenres = new List<Genre>();
            }
            if (this.AlbumArtists == null)
            {
                this.AlbumArtists = new List<Artist>();
            }
        }
    }
}