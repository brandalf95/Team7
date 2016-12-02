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

    public enum RatingFilter { Greater, Less }

    public enum SortBy { Artist, Album, Rating, Song, Genre}

    public enum SortOrder { Ascending, Descending}

    public class ArtistsController : Controller
    {
        private AppDbContext db = new AppDbContext();


        // GET: Artists
        public ActionResult Index(String SearchString, string error)
        {
            //var averageRating =
            //    (from r in db.ArtistReviews
            //     select r.Rating).Average();

            //ViewBag.avgRating = averageRating;
            if(error == null)
            {
                ViewBag.Error = "";
            }
            else
            {
                ViewBag.Error = error;
            }
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
            List<AvgArtistRating> ratingList = new List<AvgArtistRating>();
            foreach (Artist a in SelectedArtists)
            {
                AvgArtistRating dude = new AvgArtistRating();
                dude.Artist = a;
                dude.AvgRating = ComputeAverage(a.ArtistID);
                ratingList.Add(dude);

            }
            return View(ratingList);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        public ActionResult SearchResults(String SearchString, int[] SelectedGenres, String AvRating, RatingFilter SelectedRatingFilter, SortBy SelectedSortBy, SortOrder SelectedSortOrder)
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
            if (SelectedGenres != null)
            {
                foreach (int i in SelectedGenres)
                {
                    List<Artist> ArtistsFound = query.Where(a => a.ArtistGenres.Any(g => g.GenreID == i)).ToList();

                    foreach (Artist a in ArtistsFound)
                    {
                        DisplayArtists.Add(a);
                    }
                }
            }

            else
            {
                List<Artist> ArtistsFound = query.Where(a => a.ArtistGenres.Any()).ToList();

                foreach (Artist a in ArtistsFound)
                {
                    
                    DisplayArtists.Add(a);
                }
            }

            var TotalArtists = db.Artists.ToList();
            ViewBag.SelectedArtistCount = "Displaying " + DisplayArtists.Count() + " of " + TotalArtists.Count() + " Records";

            DisplayArtists = DisplayArtists.OrderBy(a => a.ArtistName).ToList();

            if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplayArtists = DisplayArtists.OrderBy(a => a.ArtistName).ToList();
            }

            if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Descending)
            {
                DisplayArtists = DisplayArtists.OrderByDescending(a => a.ArtistName).ToList();

            }

            List<AvgArtistRating> ratingList = new List<AvgArtistRating>();
            foreach (Artist a in DisplayArtists)
            {
                AvgArtistRating dude = new AvgArtistRating();
                dude.Artist = a;
                dude.AvgRating = ComputeAverage(a.ArtistID);
                ratingList.Add(dude);

            }

            if (SelectedSortBy == SortBy.Rating)
            {
                if (SelectedSortOrder == SortOrder.Ascending)
                {
                    ratingList = ratingList.OrderBy(a => a.AvgRating).ToList();
                }
                else
                {
                    ratingList = ratingList.OrderByDescending(a => a.AvgRating).ToList();
                }
            }

            //TODO: code for Rating Filter for Artist
            if (AvRating != null && AvRating != "")
            {

                Decimal decAvgRating;
                try
                {
                    decAvgRating = Convert.ToDecimal(AvRating);
                }
                catch
                {
                    ViewBag.Message = AvRating + " is not a valid rating; please enter a decimal from 1.0 to 5.0";
                    ViewBag.AllGenres = GetAllGenres();

                    return View("DetailedSearch");
                }
                try
                {
                    if (decAvgRating < 1.0m || decAvgRating > 5.0m)
                    {
                        throw new ArgumentException();
                    }
                }
                catch (ArgumentException)
                {
                    ViewBag.Message = AvRating + " is not a valid rating; please enter a decimal from 1.0 to 5.0";
                    ViewBag.AllGenres = GetAllGenres();

                    return View("DetailedSearch");
                }

                if (SelectedRatingFilter == RatingFilter.Greater)
                {
                  foreach(AvgArtistRating item in ratingList.ToList())
                    {
                        if(item.AvgRating < decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }

                else
                {
                    foreach (AvgArtistRating item in ratingList.ToList())
                    {
                        if (item.AvgRating > decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }
            }
            //TODO: figure this out.


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

        public MultiSelectList GetAllGenres(Artist artist)
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            List<Int32> SelectedGenres = new List<Int32>();

            foreach (Genre g in artist.ArtistGenres)
            {
                SelectedGenres.Add(g.GenreID);
            }

            MultiSelectList allGenresList = new MultiSelectList(allGenres, "GenreID", "GenreName", SelectedGenres);
            return allGenresList;
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
            AvgArtistRating artistRating = new AvgArtistRating();
            artistRating.Artist = artist;
            artistRating.AvgRating = ComputeAverage(artist.ArtistID);
            return View(artistRating);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            ViewBag.AllGenres = GetAllGenres();
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID,ArtistName,IsFeatured")] Artist artist, int[] SelectedGenres)
        {

            if (ModelState.IsValid)
            {
                if (SelectedGenres != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        artist.ArtistGenres.Add(genreToAdd);
                    }

                    db.Artists.Add(artist);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message ="An artist must belong to at least one genre.";
                    ViewBag.AllGenres = GetAllGenres();
                    return View(artist);
                }           
            }

            ViewBag.AllGenres = GetAllGenres();
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

            ViewBag.AllGenres = GetAllGenres(artist);
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,IsFeatured")] Artist artist, int[] SelectedGenres)
        {
            if (ModelState.IsValid)
            {

                Artist artistToChange = db.Artists.Find(artist.ArtistID);

                artistToChange.ArtistGenres.Clear();

                if (SelectedGenres != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        artistToChange.ArtistGenres.Add(genreToAdd);
                    }

                    artistToChange.ArtistName = artist.ArtistName;
                    artistToChange.IsFeatured = artist.IsFeatured;

                    db.Entry(artistToChange).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message = "An artist must belong to at least one genre.";
                    ViewBag.AllGenres = GetAllGenres(artist);
                    return View(artist);
                }


            }

            ViewBag.AllGenres = GetAllGenres(artist);
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

        public decimal ComputeAverage(Int32 Artist)
        {

            AvgArtistRating rating = new AvgArtistRating();
            Artist artist = db.Artists.Find(Artist);
            List<ArtistReview> reviewList = new List<ArtistReview>();
            reviewList = db.ArtistReviews.ToList();
            List<ArtistReview> selectedReviewList = new List<ArtistReview>();
            selectedReviewList = db.ArtistReviews.Where(a => a.Artist.ArtistName.Contains(artist.ArtistName)).ToList();
            decimal sum = new decimal();
            decimal count = new decimal();
            foreach (ArtistReview a in selectedReviewList)
            {
                sum += a.Rating;
                count += 1;
            }

            if (count == 0)
            {
                return (0);
            }
            return (sum/count);
        }


    }
}
