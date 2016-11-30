using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7_LonghornMusic.Models;

namespace Team7_LonghornMusic.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext db = new AppDbContext();
        // GET: Home
        public ActionResult Index()
        {
            if(User.IsInRole("Customer"))
            {
                return RedirectToAction("CustomerHome", "Home");          
            }
            return View();
        }

        public ActionResult ReviewHome()
        {
            return View();
        }

        public ActionResult ContentHome()
        {
            return View();
        }

        public ActionResult CustomerHome()
        {
            
            var query = from s in db.Songs
                        where s.IsFeatured == true
                        select s;

            List<Song> FeaturedSongs = query.ToList();          
            foreach (Song s in FeaturedSongs)
            {               
                ViewBag.Song = s.SongTitle;
                ViewBag.SongPrice = s.DisplayPrice;
            }

            var query2 = from a in db.Albums
                        where a.IsFeatured == true
                        select a;

            List<Album> FeaturedAlbums = query2.ToList();
            foreach (Album a in FeaturedAlbums)
            {
                ViewBag.Album = a.AlbumTitle;
                ViewBag.AlbumPrice = a.DisplayPrice;
            }

            var query3 = from x in db.Artists
                        where x.IsFeatured == true
                        select x;

            List<Artist> FeaturedArtists = query3.ToList();
            foreach (Artist x in FeaturedArtists)
            {
                ViewBag.Artist = x.ArtistName;
                
            }

            return View();
        }

        //public List<Song> GetFeaturedSong()
        //{
        //    var query = from s in db.Songs
        //                where s.IsFeatured == true
        //                select s;

        //    List<Song> FeaturedSongs = query.ToList();
        //    foreach (Song s in FeaturedSongs)
        //    {
        //        ViewBag.FeaturedSongs = s.SongTitle;
        //    }
        //    return View(FeaturedSongs);
        //}
    }
}