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
    public class ArtistsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Artists
        public ActionResult Index(String SearchString)
        {
            var averageRating =
                (from r in db.ArtistReviews
                 select r.Rating).Average();

            ViewBag.avgRating = averageRating;

            List<Artist> SelectedArtists = new List<Artist>();
            List<Artist> TotalArtists = new List<Artist>();
            TotalArtists = db.Artists.ToList();

            if (SearchString == null || SearchString == "")
            {
                SelectedArtists = db.Artists.ToList();
                ViewBag.SelectedArtistCount = "Displaying " + SelectedArtists.Count() + " of " + TotalArtists.Count() + " Records";
            }

            else
            {
                SelectedArtists = db.Artists.Where(a => a.ArtistName.Contains(SearchString)).ToList();
                ViewBag.SelectedArtistCount = "Displaying " + SelectedArtists.Count() + " of " + TotalArtists.Count() + " Records";
            }

            SelectedArtists = SelectedArtists.OrderBy(a => a.ArtistName).ToList();
            return View(SelectedArtists);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        public ActionResult SearchResults(String SearchString, int[] SelectedGenres)
        {
            var query = from a in db.Artists
                                  select a;

            //code for textbox
            if (SearchString == null || SearchString == "")
            {
                query = query.Where(a => a.ArtistName != null);
            }
            else
            {
                query = query.Where(a => a.ArtistName.Contains(SearchString));
            }

            //code for genre filter
            List<Artist> DisplayArtists = new List<Artist>();

            foreach (int i in SelectedGenres)
            {
                List<Artist> ArtistsFound = query.Where(a => a.ArtistGenres.Any(g => g.GenreID == i)).ToList();

                foreach (Artist a in ArtistsFound)
                {
                    DisplayArtists.Add(a);
                }
            }

            //TODO: code for Rating Filter for Artist
            // if 

            List<Artist> SelectedArtists = query.ToList();

            var TotalArtists = db.Artists.ToList();
            ViewBag.SelectedArtistCount = "Displaying " + SelectedArtists.Count() + " of " + TotalArtists.Count() + " Records";

            SelectedArtists = SelectedArtists.OrderBy(a => a.ArtistName).ToList();

            return View("Index", SelectedArtists);
        }


        public MultiSelectList GetAllGenres()
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            //Add in choice for not selecting a frequency
            Genre NoChoice = new Genre() { GenreID = 0, GenreName = "All" };
            allGenres.Add(NoChoice);
            MultiSelectList GenreList = new MultiSelectList(allGenres.OrderBy(g => g.GenreName), "GenreID", "GenreName");
            return GenreList;
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID,ArtistName,IsFeatured")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Artists.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,IsFeatured")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artists.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artists.Find(id);
            db.Artists.Remove(artist);
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
