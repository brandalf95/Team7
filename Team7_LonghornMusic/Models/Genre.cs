using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Team7_LonghornMusic.Models
{
    public class Genre
    {
        public Int32 GenreID { get; set; }

        [Display(Name="Genre Name")]
        [Required(ErrorMessage = "Genre Name required")]
        public String GenreName { get; set; }

        [Display(Name = "Genre's Artists")]
        public virtual List<Artist> GenreArtists { get; set; }

        [Display(Name = "Genre's Albums")]
        public virtual List<Album> GenreAlbums { get; set; }

        [Display(Name = "Genre's Songs")]
        public virtual List<Song> GenreSongs { get; set; }
        
    }
}