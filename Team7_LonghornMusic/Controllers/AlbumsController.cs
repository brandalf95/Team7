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
            foreach (Album item in TotalAlbums)
            {
                if (item.DiscountPrice != 0)
                {
                    item.DisplayPrice = item.DiscountPrice;
                }
                else
                {
                    item.DisplayPrice = item.AlbumPrice;
                }
                db.Albums.Find(item.AlbumID).DisplayPrice = item.DisplayPrice;
            }
            db.SaveChanges();

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

        public ActionResult SearchResults(String AlbumSearchString, String ArtistSearchString, int[] SelectedGenres, String AvRating, RatingFilter SelectedRatingFilter, SortBy SelectedSortBy, SortOrder SelectedSortOrder)
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

            var TotalAlbums = db.Albums.ToList();

            DisplayAlbums = DisplayAlbums.OrderBy(a => a.AlbumTitle).ToList();

           if (SelectedSortBy == SortBy.Album && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplayAlbums = DisplayAlbums.Distinct().OrderBy(a => a.AlbumTitle).ToList();
            }

            if (SelectedSortBy == SortBy.Album && SelectedSortOrder == SortOrder.Descending)
            {
                DisplayAlbums = DisplayAlbums.Distinct().OrderByDescending(a => a.AlbumTitle).ToList();

            }

            if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplayAlbums = (from a in DisplayAlbums from ar in a.AlbumArtists.Distinct() orderby ar.ArtistName select a).ToList();
                DisplayAlbums = DisplayAlbums.Distinct().ToList();
            }

            else if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Descending)
            {          
               DisplayAlbums = (from a in DisplayAlbums from ar in a.AlbumArtists.Distinct() orderby ar.ArtistName descending select a).ToList();
               DisplayAlbums = DisplayAlbums.Distinct().ToList();
            }                     

            List<AvgAlbumRating> ratingList = new List<AvgAlbumRating>();
            foreach (Album a in DisplayAlbums)
            {
                AvgAlbumRating dude = new AvgAlbumRating();
                dude.Album = a;
                dude.AvgRating = ComputeAverage(a.AlbumID);
                ratingList.Add(dude);

            }

            if (SelectedSortBy == SortBy.Rating)
            {
                if (SelectedSortOrder == SortOrder.Ascending)
                {
                    ratingList = ratingList.Distinct().OrderBy(a => a.AvgRating).ToList();
                }
                else
                {
                    ratingList = ratingList.Distinct().OrderByDescending(a => a.AvgRating).ToList();
                }
            }

            //TODO: code for Rating Filter for Album
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
                    foreach (AvgAlbumRating item in ratingList.ToList())
                    {
                        if (item.AvgRating < decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }

                else
                {
                    foreach (AvgAlbumRating item in ratingList.ToList())
                    {
                        if (item.AvgRating > decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }
            }

            ViewBag.SelectedAlbumCount = "Displaying " + DisplayAlbums.Count() + " of " + TotalAlbums.Count() + " Records";

            return View("Index", ratingList);
        }

        public MultiSelectList GetAllGenres()
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            MultiSelectList GenreList = new MultiSelectList(allGenres.OrderBy(g => g.GenreName), "GenreID", "GenreName");
            return GenreList;
        }

        public MultiSelectList GetAllGenres(Album album)
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            List<Int32> SelectedGenres = new List<Int32>();

            foreach (Genre g in album.AlbumGenres)
            {
                SelectedGenres.Add(g.GenreID);
            }

            MultiSelectList allGenresList = new MultiSelectList(allGenres, "GenreID", "GenreName", SelectedGenres);
            return allGenresList;
        }

        public MultiSelectList GetAllArtists()
        {
            var query = from c in db.Artists
                        orderby c.ArtistName
                        select c;

            List<Artist> allArtists = query.ToList();

            MultiSelectList ArtistList = new MultiSelectList(allArtists.OrderBy(a => a.ArtistName), "ArtistID", "ArtistName");
            return ArtistList;
        }

        public MultiSelectList GetAllArtists(Album album)
        {
            var query = from c in db.Artists
                        orderby c.ArtistName
                        select c;

            List<Artist> allArtists = query.ToList();

            List<Int32> SelectedArtists = new List<Int32>();

            foreach (Artist a in album.AlbumArtists)
            {
                SelectedArtists.Add(a.ArtistID);
            }

            MultiSelectList allArtistsList = new MultiSelectList(allArtists, "ArtistID", "ArtistName", SelectedArtists);
            return allArtistsList;
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
            AvgAlbumRating albumRating = new AvgAlbumRating();
            albumRating.Album = album;
            albumRating.AvgRating = ComputeAverage(album.AlbumID);
            return View(albumRating);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumID,AlbumTitle,IsFeatured,AlbumPrice")] Album album, int[] SelectedGenres, int[] SelectedArtists)
        {
            if (ModelState.IsValid)
            {
                if (SelectedGenres != null && SelectedArtists != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        album.AlbumGenres.Add(genreToAdd);
                    }

                    foreach (int ArtistID in SelectedArtists)
                    {
                        Artist artistToAdd = db.Artists.Find(ArtistID);
                        album.AlbumArtists.Add(artistToAdd);
                    }

                    db.Albums.Add(album);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message = "An album must belong to at least one genre, and at least one artist";
                    ViewBag.AllGenres = GetAllGenres();
                    ViewBag.AllArtists = GetAllArtists();
                    return View(album);
                }
            }

            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
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
            ViewBag.AllGenres = GetAllGenres(album);
            ViewBag.AllArtists = GetAllArtists(album);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumID,AlbumTitle,IsFeatured,AlbumPrice,DiscountPrice")] Album album, int[] SelectedGenres, int[] SelectedArtists)
        {
            if (ModelState.IsValid)
            {

                Album albumToChange = db.Albums.Find(album.AlbumID);

                if (albumToChange.DiscountPrice != 0)
                {
                    albumToChange.DisplayPrice = albumToChange.DiscountPrice;
                }
                else { albumToChange.DisplayPrice = albumToChange.AlbumPrice; }

                //Album albumToChange = db.Albums.Find(album.AlbumID);

                albumToChange.AlbumGenres.Clear();
                albumToChange.AlbumArtists.Clear();

                if (SelectedGenres != null && SelectedArtists != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        albumToChange.AlbumGenres.Add(genreToAdd);
                    }

                    foreach (int ArtistID in SelectedArtists)
                    {
                        Artist artistToAdd = db.Artists.Find(ArtistID);
                        albumToChange.AlbumArtists.Add(artistToAdd);
                    }

                    albumToChange.AlbumTitle = album.AlbumTitle;
                    albumToChange.IsFeatured = album.IsFeatured;
                    albumToChange.AlbumPrice = album.AlbumPrice;
                    albumToChange.DiscountPrice = album.DiscountPrice;

                    db.Entry(albumToChange).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message = "An album must belong to at least one genre, and at least one artist";
                    ViewBag.AllGenres = GetAllGenres(album);
                    ViewBag.AllArtists = GetAllArtists(album);
                    return View(album);
                }
            }

            ViewBag.AllGenres = GetAllGenres(album);
            ViewBag.AllArtists = GetAllArtists(album);
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
