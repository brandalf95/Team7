using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team7_LonghornMusic.Models;

namespace Team7_LonghornMusic.Controllers
{
    public class AlbumsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Albums
        public ActionResult Index(String SearchString)
        {

            List<Album> SelectedAlbums = new List<Album>();
            List<Album> TotalAlbums = new List<Album>();
            TotalAlbums = db.Albums.ToList();

            if (SearchString == null || SearchString == "")
            {
                SelectedAlbums = db.Albums.ToList();
                ViewBag.SelectedAlbumCount = "Displaying " + SelectedAlbums.Count() + " of " + TotalAlbums.Count() + " Records";
            }

            else
            {
                SelectedAlbums = db.Albums.Where(a => a.AlbumTitle.Contains(SearchString)).ToList();
                ViewBag.SelectedAlbumCount = "Displaying " + SelectedAlbums.Count() + " of " + TotalAlbums.Count() + " Records";
            }

            SelectedAlbums = SelectedAlbums.OrderBy(a => a.AlbumTitle).ToList();
            List<AvgAlbumRating> ratingList = new List<AvgAlbumRating>();
            foreach (Album a in SelectedAlbums)
            {
                AvgAlbumRating dude = new AvgAlbumRating();
                dude.Album = a;
                dude.AvgRating = ComputeAverage(a.AlbumID);
                ratingList.Add(dude);

            }
            return View(ratingList);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            //ViewBag.AllArtists = GetAllArtists();
            return View();
        }

        public ActionResult SearchResults(String AlbumSearchString, String ArtistSearchString, int[] SelectedGenres)
        {
            var query = from a in db.Albums
                        select a;

            //code for textbox
            if (AlbumSearchString == null || AlbumSearchString == "")
            {
                query = query.Where(a => a.AlbumTitle != null);
            }
            else
            {
                query = query.Where(a => a.AlbumTitle.Contains(AlbumSearchString));
            }

            //code for artist textbox
            if (ArtistSearchString == null || ArtistSearchString == "")
            {
                query = from a in query from ar in a.AlbumArtists where ar.ArtistName != null select a;
            }
            else
            {
                query = from a in query from ar in a.AlbumArtists where ar.ArtistName.Contains(ArtistSearchString) select a;
            }


            //code for genre filter
            List<Album> DisplayAlbums = new List<Album>();
            if (SelectedGenres != null)
            {
                foreach (int i in SelectedGenres)
                {
                    List<Album> AlbumsFound = query.Where(a => a.AlbumGenres.Any(g => g.GenreID == i)).ToList();

                    foreach (Album a in AlbumsFound)
                    {
                        DisplayAlbums.Add(a);
                    }
                }
            }

            else
            {
                List<Album> AlbumsFound = query.Where(a => a.AlbumGenres.Any()).ToList();

                foreach (Album a in AlbumsFound)
                {
                    DisplayAlbums.Add(a);
                }
            }

            List<Album> SelectedAlbums = DisplayAlbums.Distinct().ToList();

            var TotalAlbums = db.Albums.ToList();
            ViewBag.SelectedAlbumCount = "Displaying " + SelectedAlbums.Count() + " of " + TotalAlbums.Count() + " Records";

            SelectedAlbums = SelectedAlbums.OrderBy(a => a.AlbumTitle).ToList();
            List<AvgAlbumRating> ratingList = new List<AvgAlbumRating>();
            foreach (Album a in SelectedAlbums)
            {
                AvgAlbumRating dude = new AvgAlbumRating();
                dude.Album = a;
                dude.AvgRating = ComputeAverage(a.AlbumID);
                ratingList.Add(dude);

            }
            return View("Index", ratingList);
        }

        public MultiSelectList GetAllGenres()
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            //Add in choice for not selecting a frequency
            //Genre NoChoice = new Genre() { GenreID = 0, GenreName = "All" };
            //allGenres.Add(NoChoice);
            MultiSelectList GenreList = new MultiSelectList(allGenres.OrderBy(g => g.GenreName), "GenreID", "GenreName");
            return GenreList;
        }

        //public MultiSelectList GetAllArtists()
        //{
        //    var query = from c in db.Artists
        //                orderby c.ArtistName
        //                select c;

        //    List<Artist> allArtists = query.ToList();

        //    //Add in choice for not selecting a frequency
        //    //Genre NoChoice = new Genre() { GenreID = 0, GenreName = "All" };
        //    //allGenres.Add(NoChoice);
        //    MultiSelectList ArtistList = new MultiSelectList(allArtists.OrderBy(a => a.ArtistName), "ArtistID", "ArtistName");
        //    return ArtistList;
        //}

        
        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            AvgAlbumRating albumRating = new AvgAlbumRating();
            albumRating.Album = album;
            albumRating.AvgRating = ComputeAverage(album.AlbumID);
            return View(albumRating);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,AlbumTitle,IsFeatured,AlbumPrice")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,AlbumTitle,IsFeatured,AlbumPrice")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public decimal ComputeAverage(Int32 Artist)
        {

            AvgAlbumRating rating = new AvgAlbumRating();
            Album artist = db.Albums.Find(Artist);
            List<AlbumReview> reviewList = new List<AlbumReview>();
            reviewList = db.AlbumReviews.ToList();
            List<AlbumReview> selectedReviewList = new List<AlbumReview>();
            selectedReviewList = db.AlbumReviews.Where(a => a.Album.AlbumTitle.Contains(artist.AlbumTitle)).ToList();
            decimal sum = new decimal();
            decimal count = new decimal();
            foreach (AlbumReview a in selectedReviewList)
            {
                sum += a.Rating;
                count += 1;
            }

            if (count == 0)
            {
                return (0);
            }
            return (sum / count);
        }
    }
}
