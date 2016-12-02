using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team7_LonghornMusic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Team7_LonghornMusic.Controllers
{
    public class ArtistReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: ArtistReviews
        public ActionResult Index()
        {
            return View(db.ArtistReviews.ToList());
        }

        public ActionResult MyReviewIndex()
        {
            var query = from c in db.ArtistReviews
                        select c;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            List<ArtistReview> myList = query.ToList();
            myList = db.ArtistReviews.ToList().Where(c => c.User.Id == userId).ToList();

            return View(myList);
        }

        // GET: ArtistReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistReview artistReview = db.ArtistReviews.Find(id);
            if (artistReview == null)
            {
                return HttpNotFound();
            }
            return View(artistReview);
        }

        // GET: ArtistReviews/Create
        public ActionResult Create(Int32 ArtistID, string UserID)
        {
            bool dummy = true;
            List<OrderDetail> orders = new List<OrderDetail>();
            orders = db.OrderDetails.Where(a => a.User.UserName.Contains(UserID)).ToList();
            foreach(OrderDetail item in orders)
            {
                if(item.IsConfirmed && !(item.IsRefunded))
                {
                    foreach(Discount x in item.Discounts)
                    {
                        if(x.Song != null)
                        {
                            foreach(Artist y in x.Song.SongArtists)
                            {
                                if(y == db.Artists.Find(ArtistID))
                                {
                                    dummy = false;
                                }
                            }
                        }else
                        {
                            
                                foreach(Artist a in x.Album.AlbumArtists)
                                {
                                    if (a == db.Artists.Find(ArtistID))
                                    {
                                        dummy = false;
                                    }
                                }
                                foreach(Song z in x.Album.AlbumSongs) {
                                    foreach (Artist y in z.SongArtists)
                                    {
                                        if (y == db.Artists.Find(ArtistID))
                                        {
                                            dummy = false;
                                        }
                                    }
                                }
                                
                            }
                        
                    }
                }
            }
            if (dummy == false)
            {
                return View();
            }else
            {
                return RedirectToAction("Index", "Artists", new { UserName = UserID, error = "You have to buy a song from the artist before you can review the artist." });
            }
        }

        // POST: ArtistReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistReviewID,Rating,Comment")] ArtistReview artistReview, string UserID, Int32 ArtistID)
        {
            if (ModelState.IsValid)
            {
                artistReview.Artist = db.Artists.Find(ArtistID);
                List<AppUser> theseUsers = new List<AppUser>();
                theseUsers = db.Users.Where(a=>a.UserName.Contains(UserID)).ToList();
                int i = 0;
                artistReview.User = theseUsers[0];

                db.ArtistReviews.Add(artistReview);
                db.SaveChanges();
                return RedirectToAction("Index", "Artists");
            }

            return View(artistReview);
        }

        // GET: ArtistReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistReview artistReview = db.ArtistReviews.Find(id);
            if (artistReview == null)
            {
                return HttpNotFound();
            }
            return View(artistReview);
        }

        // POST: ArtistReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistReviewID,Rating,Comment")] ArtistReview artistReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artistReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artistReview);
        }

        // GET: ArtistReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistReview artistReview = db.ArtistReviews.Find(id);
            if (artistReview == null)
            {
                return HttpNotFound();
            }
            return View(artistReview);
        }

        // POST: ArtistReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtistReview artistReview = db.ArtistReviews.Find(id);
            db.ArtistReviews.Remove(artistReview);
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
