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
    public class SongsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Songs
        public ActionResult Index(String SearchString)
        {
            List<Song> SelectedSongs = new List<Song>();
            List<Song> TotalSongs  = new List<Song>();
            TotalSongs = db.Songs.ToList();

            if (SearchString == null || SearchString == "")
            {
                SelectedSongs = db.Songs.ToList();
                ViewBag.SelectedSongCount = "Displaying " + SelectedSongs.Count() + " of " + TotalSongs.Count() + " Records";
            }

            else
            {
                SelectedSongs = db.Songs.Where(a => a.SongTitle.Contains(SearchString)).ToList();
                ViewBag.SelectedSongCount = "Displaying " + SelectedSongs.Count() + " of " + TotalSongs.Count() + " Records";
            }

            SelectedSongs = SelectedSongs.OrderBy(a => a.SongTitle).ToList();
            return View(SelectedSongs);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            ViewBag.AllAlbums = GetAllAlbums();
            return View();
        }

        public ActionResult SearchResults(String SongSearchString, String AlbumSearchString, String ArtistSearchString, int[] SelectedGenres)
        {
            var query = from s in db.Songs
                        select s;


            //code for song textbox
            if (SongSearchString == null || SongSearchString == "")
            {
                query = query.Where(s => s.SongTitle != null);
            }
            else
            {
                query = query.Where(s => s.SongTitle.Contains(SongSearchString));
            }

            List<Song> DisplaySongs = new List<Song>();

            //code for Album textbox
            if (ArtistSearchString == null || ArtistSearchString == "")
            {
                query = from s in query from al in s.SongAlbums where al.AlbumTitle != null select s;
            }
            else
            {
                query = from s in query from al in s.SongAlbums where al.AlbumTitle.Contains(AlbumSearchString) select s;
            }

            //code for artist textbox
            if (ArtistSearchString == null || ArtistSearchString == "")
            {
                query = from s in query from ar in s.SongArtists where ar.ArtistName != null select s;
            }
            else
            {
                query = from s in query from ar in s.SongArtists where ar.ArtistName.Contains(ArtistSearchString) select s;
            }

            //code for genre filter
            if (SelectedGenres != null)
            {
                foreach (int i in SelectedGenres)
                {
                    List<Song> SongsFound = query.Where(a => a.SongGenres.Any(g => g.GenreID == i)).ToList();

                    foreach (Song a in SongsFound)
                    {
                        DisplaySongs.Add(a);
                    }
                }
            }

            else
            {
                List<Song> SongsFound = query.Where(a => a.SongGenres.Any()).ToList();

                foreach (Song a in SongsFound)
                {
                    DisplaySongs.Add(a);
                }
            }



            //TODO: code for Rating Filter for Album
            // if 

            List<Song> SelectedSongs = DisplaySongs.Distinct().ToList();

            var TotalSongs = db.Songs.ToList();
            ViewBag.SelectedSongCount = "Displaying " + SelectedSongs.Count() + " of " + TotalSongs.Count() + " Records";

            SelectedSongs = SelectedSongs.OrderBy(a => a.SongTitle).ToList();

            return View("Index", SelectedSongs);
        }

        //populate multiselect for genres
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

        public MultiSelectList GetAllAlbums()
        {
            var query = from c in db.Albums
                        orderby c.AlbumTitle
                        select c;
            List<Album> allAlbums = query.ToList();

            MultiSelectList AlbumList = new MultiSelectList(allAlbums.OrderBy(a => a.AlbumTitle), "AlbumID", "AlbumTitle");
            return AlbumList;
        }

        // GET: Songs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // GET: Songs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SongID,SongTitle,IsFeatured,SongPrice")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Songs.Add(song);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(song);
        }

        // GET: Songs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SongID,SongTitle,IsFeatured,SongPrice")] Song song)
        {
            if (ModelState.IsValid)
            {
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(song);
        }

        // GET: Songs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = db.Songs.Find(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Song song = db.Songs.Find(id);
            db.Songs.Remove(song);
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
