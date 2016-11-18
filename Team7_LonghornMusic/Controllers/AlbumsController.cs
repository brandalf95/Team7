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
            //TODO: FIGURE THIS OUT
            //var averageRating =
              //(from r in db.AlbumReviews
               //select r.Rating).Average();

            //ViewBag.avgRating = averageRating;

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
            return View(SelectedAlbums);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            return View();
        }

        public ActionResult SearchResults(String SearchString, int[] SelectedGenres, int[] SelectedArtists)
        {
            var query = from a in db.Albums
                        select a;


            //code for textbox
            if (SearchString == null || SearchString == "")
            {
                query = query.Where(a => a.AlbumTitle != null);
            }
            else
            {
                query = query.Where(a => a.AlbumTitle.Contains(SearchString));
            }

            List<Album> DisplayAlbums = new List<Album>();

            //code for artist filter
            if (SelectedArtists != null)
            {
                foreach (int i in SelectedArtists)
                {
                    List<Album> AlbumsFound = query.Where(a => a.AlbumArtists.Any(ar => ar.ArtistID == i)).ToList();

                    foreach (Album a in AlbumsFound)
                    {
                        DisplayAlbums.Add(a);
                    }
                }
            }
            else
            {
                List<Album> AlbumsFound = query.Where(a => a.AlbumArtists.Any()).ToList();

                foreach (Album a in AlbumsFound)
                {
                    DisplayAlbums.Add(a);
                }
            }

            //code for genre filter
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

            //TODO: code for Rating Filter for Album
            // if 

            List<Album> SelectedAlbums = DisplayAlbums.Distinct().ToList();

            var TotalAlbums = db.Albums.ToList();
            ViewBag.SelectedAlbumCount = "Displaying " + SelectedAlbums.Count() + " of " + TotalAlbums.Count() + " Records";

            SelectedAlbums = SelectedAlbums.OrderBy(a => a.AlbumTitle).ToList();

            return View("Index", SelectedAlbums);
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

        public MultiSelectList GetAllArtists()
        {
            var query = from c in db.Artists
                        orderby c.ArtistName
                        select c;

            List<Artist> allArtists = query.ToList();

            //Add in choice for not selecting a frequency
            //Genre NoChoice = new Genre() { GenreID = 0, GenreName = "All" };
            //allGenres.Add(NoChoice);
            MultiSelectList ArtistList = new MultiSelectList(allArtists.OrderBy(a => a.ArtistName), "ArtistID", "ArtistName");
            return ArtistList;
        }

        
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
            return View(album);
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
    }
}
