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
    public class SongReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: SongReviews
        public ActionResult Index()
        {
            return View(db.SongReviews.ToList());
        }

        public ActionResult MyReviewIndex()
        {
            var query = from c in db.SongReviews
                        select c;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            List<SongReview> myList = query.ToList();
            myList = db.SongReviews.ToList().Where(c => c.User.Id == userId).ToList();

            return View(myList);
        }

        // GET: SongReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SongReview songReview = db.SongReviews.Find(id);
            if (songReview == null)
            {
                return HttpNotFound();
            }
            return View(songReview);
        }

        // GET: SongReviews/Create
        public ActionResult Create(Int32 SongID, string UserName)
        {
            bool dummy = true;
            List<OrderDetail> orders = new List<OrderDetail>();
            orders = db.OrderDetails.Where(a => a.User.UserName.Contains(UserName)).ToList();
            foreach (OrderDetail item in orders)
            {
                if (item.IsConfirmed && !(item.IsRefunded))
                {
                    foreach (Discount x in item.Discounts)
                    {
                        if (x.Song != null)
                        {
                            
                                if (x.Song == db.Songs.Find(SongID))
                                {
                                    dummy = false;
                                }
                  
                        }
                        else
                        {
                            if (x.Album != null)
                            {
                                foreach (Song z in x.Album.AlbumSongs)
                                {
                                    
                                        if (z == db.Songs.Find(SongID))
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
            }
            else
            {
                return RedirectToAction("Index", "Songs", new { UserName = UserName, error = "You have to buy a song to review it." });
            }
            return View();
        }

        // POST: SongReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SongReviewID,Rating,Comment")] SongReview songReview, string UserName, Int32 SongID)
        {
            songReview.Song = db.Songs.Find(SongID);
            List<AppUser> theseUsers = new List<AppUser>();
            theseUsers = db.Users.Where(a => a.UserName.Contains(UserName)).ToList();
            int i = 0;
            songReview.User = theseUsers[0];



            if (ModelState.IsValid)
            {
                db.SongReviews.Add(songReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(songReview);
        }

        // GET: SongReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SongReview songReview = db.SongReviews.Find(id);
            if (songReview == null)
            {
                return HttpNotFound();
            }
            return View(songReview);
        }

        // POST: SongReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SongReviewID,Rating,Comment")] SongReview songReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(songReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(songReview);
        }

        // GET: SongReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SongReview songReview = db.SongReviews.Find(id);
            if (songReview == null)
            {
                return HttpNotFound();
            }
            return View(songReview);
        }

        // POST: SongReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SongReview songReview = db.SongReviews.Find(id);
            db.SongReviews.Remove(songReview);
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
