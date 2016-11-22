using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class Song
    {
        public Int32 SongID { get; set; }

        [Display(Name = "Song Title")]
        [Required(ErrorMessage = "Song Title is required")]
        public String SongTitle { get; set; }

        [Display(Name = "Featured")]
        public bool IsFeatured { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Every song must have a price")]
        [DataType(DataType.Currency)]
        public decimal SongPrice { get; set; }

        [Display(Name = "Song's Genres")]
        public virtual List<Genre> SongGenres { get; set; }

        [Display(Name = "Song's Artists")]
        public virtual List<Artist> SongArtists { get; set; }

        [Display(Name = "Song's Albums")]
        public virtual List<Album> SongAlbums { get; set; }

        [Display(Name = "Song's Reviews")]
        public virtual List<SongReview> SongReviews { get; set; }

        [Display(Name = "Song's Discount")]
        public virtual List<Discount> SongDiscount { get; set; }


        public Song()
        {
            if(this.SongGenres == null)
            {
                this.SongGenres = new List<Genre>();
            }
            if(this.SongArtists == null)
            {
                this.SongArtists = new List<Artist>();
            }
            if(this.SongAlbums == null)
            {
                this.SongAlbums = new List<Album>();
            }
        }
    }
}