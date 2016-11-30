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
            List<AvgSongRating> ratingList = new List<AvgSongRating>();
            foreach (Song a in SelectedSongs)
            {
                AvgSongRating dude = new AvgSongRating();
                dude.Song = a;
                dude.AvgRating = ComputeAverage(a.SongID);
                ratingList.Add(dude);

            }

            return View(ratingList);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            ViewBag.AllAlbums = GetAllAlbums();
            return View();
        }

        public ActionResult SearchResults(String SongSearchString, String AlbumSearchString, String ArtistSearchString, int[] SelectedGenres, String AvRating, RatingFilter SelectedRatingFilter, SortBy SelectedSortBy, SortOrder SelectedSortOrder)
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

            //code for Album textbox
            if (AlbumSearchString == null || AlbumSearchString == "")
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

            List<Song> DisplaySongs = new List<Song>();

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


            var TotalSongs = db.Songs.ToList();

            DisplaySongs = DisplaySongs.OrderBy(a => a.SongTitle).ToList();

            if (SelectedSortBy == SortBy.Song && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplaySongs = DisplaySongs.OrderBy(a => a.SongTitle).ToList();
            }

            if (SelectedSortBy == SortBy.Song && SelectedSortOrder == SortOrder.Descending)
            {
                DisplaySongs = DisplaySongs.OrderByDescending(a => a.SongTitle).ToList();

            }

            if (SelectedSortBy == SortBy.Album && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplaySongs = (from a in DisplaySongs from al in a.SongAlbums.Distinct() orderby al.AlbumTitle  select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            if (SelectedSortBy == SortBy.Album && SelectedSortOrder == SortOrder.Descending)
            {
                DisplaySongs = (from a in DisplaySongs from al in a.SongAlbums.Distinct() orderby al.AlbumTitle descending select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplaySongs = (from a in DisplaySongs from ar in a.SongArtists.Distinct() orderby ar.ArtistName select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            if (SelectedSortBy == SortBy.Artist && SelectedSortOrder == SortOrder.Descending)
            {
                DisplaySongs = (from a in DisplaySongs from ar in a.SongArtists.Distinct() orderby ar.ArtistName descending select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            List<AvgSongRating> ratingList = new List<AvgSongRating>();
            foreach (Song a in DisplaySongs)
            {
                AvgSongRating dude = new AvgSongRating();
                dude.Song = a;
                dude.AvgRating = ComputeAverage(a.SongID);
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

            //TODO: code for Rating Filter for Song
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
                    foreach (AvgSongRating item in ratingList.ToList())
                    {
                        if (item.AvgRating < decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }

                else
                {
                    foreach (AvgSongRating item in ratingList.ToList())
                    {
                        if (item.AvgRating > decAvgRating)
                        {
                            ratingList.Remove(item);
                        }
                    }
                }
            }

            ViewBag.SelectedSongCount = "Displaying " + DisplaySongs.Count() + " of " + TotalSongs.Count() + " Records";

            return View("Index", ratingList);
        }

        //populate multiselect for genres
        public MultiSelectList GetAllGenres()
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            MultiSelectList GenreList = new MultiSelectList(allGenres.OrderBy(g => g.GenreName), "GenreID", "GenreName");
            return GenreList;
        }

        public MultiSelectList GetAllGenres(Song song)
        {
            var query = from c in db.Genres
                        orderby c.GenreName
                        select c;

            List<Genre> allGenres = query.ToList();

            List<Int32> SelectedGenres = new List<Int32>();

            foreach (Genre g in song.SongGenres)
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

        public MultiSelectList GetAllArtists(Song song)
        {
            var query = from c in db.Artists
                        orderby c.ArtistName
                        select c;

            List<Artist> allArtists = query.ToList();

            List<Int32> SelectedArtists = new List<Int32>();

            foreach (Artist a in song.SongArtists)
            {
                SelectedArtists.Add(a.ArtistID);
            }

            MultiSelectList allArtistsList = new MultiSelectList(allArtists, "ArtistID", "ArtistName", SelectedArtists);
            return allArtistsList;
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

        public MultiSelectList GetAllAlbums(Song song)
        {
            var query = from c in db.Albums
                        orderby c.AlbumTitle
                        select c;

            List<Album> allAlbums = query.ToList();
            List<Int32> SelectedAlbums = new List<Int32>();

            foreach (Album a in song.SongAlbums)
            {
                SelectedAlbums.Add(a.AlbumID);
            }

            MultiSelectList allAlbumsList = new MultiSelectList(allAlbums, "AlbumID", "AlbumTitle", SelectedAlbums);
            return allAlbumsList;
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
            AvgSongRating songRating = new AvgSongRating();
            songRating.Song = song;
            songRating.AvgRating = ComputeAverage(song.SongID);
            return View(songRating);
        }

        // GET: Songs/Create
        public ActionResult Create()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SongID,SongTitle,IsFeatured,SongPrice")] Song song, int[] SelectedGenres, int[] SelectedArtists)
        {
            if (ModelState.IsValid)
            {
                if (SelectedGenres != null && SelectedArtists != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        song.SongGenres.Add(genreToAdd);
                    }

                    foreach (int ArtistID in SelectedArtists)
                    {
                        Artist artistToAdd = db.Artists.Find(ArtistID);
                        song.SongArtists.Add(artistToAdd);
                    }

                    db.Songs.Add(song);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message = "An album must belong to at least one genre, and at least one artist";
                    ViewBag.AllGenres = GetAllGenres();
                    ViewBag.AllArtists = GetAllArtists();
                    return View(song);
                }
            }

            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
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

            ViewBag.AllGenres = GetAllGenres(song);
            ViewBag.AllArtists = GetAllArtists(song);
            ViewBag.AllAlbums = GetAllAlbums(song);
            return View(song);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SongID,SongTitle,IsFeatured,SongPrice")] Song song, int[] SelectedGenres, int[] SelectedArtists, int[] SelectedAlbums)
        {
            if (ModelState.IsValid)
            {
                Song songToChange = db.Songs.Find(song.SongID);

                songToChange.SongGenres.Clear();
                songToChange.SongArtists.Clear();
                songToChange.SongAlbums.Clear();

                if (SelectedGenres != null && SelectedArtists != null)
                {
                    foreach (int GenreID in SelectedGenres)
                    {
                        Genre genreToAdd = db.Genres.Find(GenreID);
                        songToChange.SongGenres.Add(genreToAdd);
                    }

                    foreach (int ArtistID in SelectedArtists)
                    {
                        Artist artistToAdd = db.Artists.Find(ArtistID);
                        songToChange.SongArtists.Add(artistToAdd);
                    }

                    foreach (int AlbumID in SelectedAlbums)
                    {
                        Album albumToAdd = db.Albums.Find(AlbumID);
                        songToChange.SongAlbums.Add(albumToAdd);
                    }
                    songToChange.SongTitle = song.SongTitle;
                    songToChange.IsFeatured = song.IsFeatured;
                    songToChange.SongPrice = song.SongPrice;

                    db.Entry(songToChange).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                else
                {
                    ViewBag.Message = "A song must belong to at least one genre, and at least one artist";
                    ViewBag.AllGenres = GetAllGenres(song);
                    ViewBag.AllArtists = GetAllArtists(song);
                    ViewBag.AllAlbums = GetAllAlbums(song);
                    return View(song);
                }
            }

            ViewBag.AllGenres = GetAllGenres(song);
            ViewBag.AllArtists = GetAllArtists(song);
            ViewBag.AllAlbums = GetAllAlbums(song);
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

        public decimal ComputeAverage(Int32 Song)
        {

            AvgSongRating rating = new AvgSongRating();
            Song song = db.Songs.Find(Song);
            List<SongReview> reviewList = new List<SongReview>();
            reviewList = db.SongReviews.ToList();
            List<SongReview> selectedReviewList = new List<SongReview>();
            selectedReviewList = db.SongReviews.Where(a => a.Song.SongTitle.Contains(song.SongTitle)).ToList();
            decimal sum = new decimal();
            decimal count = new decimal();
            foreach (SongReview a in selectedReviewList)
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
