using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7_LonghornMusic.Models
{
    public class ShoppingCartViewModel
    {
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public decimal SavingsAmount { get; set; }

        public string DisplayCard { get; set; }
        public decimal AverageRating { get; set; }

        public virtual List<AvgSongRating> avgSongRatings {get;set;}
        public virtual List<AvgAlbumRating> avgAlbumRatings { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }

        public ShoppingCartViewModel()
        {
            if (this.avgSongRatings == null)
            {
                this.avgSongRatings = new List<AvgSongRating>();
            }
            if(this.avgAlbumRatings == null)
            {
                this.avgAlbumRatings = new List<AvgAlbumRating>();
            }
        }
    }
}