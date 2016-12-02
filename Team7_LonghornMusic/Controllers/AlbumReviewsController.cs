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
    public class AlbumReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: AlbumReviews
        public ActionResult Index()
        {
            return View(db.AlbumReviews.ToList());
        }

        public ActionResult MyReviewIndex()
        {
            var query = from c in db.AlbumReviews
                        select c;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            List<AlbumReview> myList = query.ToList();
            myList = db.AlbumReviews.ToList().Where(c => c.User.Id == userId).ToList();

            return View(myList);
        }

        // GET: AlbumReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumReview albumReview = db.AlbumReviews.Find(id);
            if (albumReview == null)
            {
                return HttpNotFound();
            }
            return View(albumReview);
        }

        // GET: AlbumReviews/Create
        public ActionResult Create(Int32 AlbumID, string UserName)
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
                        
                            if (x.Album != null)
                            {
                                

                                    if (x.Album == db.Albums.Find(AlbumID))
                                    {
                                        dummy = false;
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
                return RedirectToAction("Index", "Albums", new { UserName = UserName, error = "You have to buy an album to review it." });
            }

        }

        // POST: AlbumReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumReviewID,Rating,Comment")] AlbumReview albumReview, string UserName, Int32 AlbumID)
        {


            albumReview.Album = db.Albums.Find(AlbumID);
            List<AppUser> theseUsers = new List<AppUser>();
            theseUsers = db.Users.Where(a => a.UserName.Contains(UserName)).ToList();
            int i = 0;
            albumReview.User = theseUsers[0];



            if (ModelState.IsValid)
            {
                db.AlbumReviews.Add(albumReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(albumReview);
        }

        // GET: AlbumReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumReview albumReview = db.AlbumReviews.Find(id);
            if (albumReview == null)
            {
                return HttpNotFound();
            }
            return View(albumReview);
        }

        // POST: AlbumReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumReviewID,Rating,Comment")] AlbumReview albumReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albumReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(albumReview);
        }

        // GET: AlbumReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AlbumReview albumReview = db.AlbumReviews.Find(id);
            if (albumReview == null)
            {
                return HttpNotFound();
            }
            return View(albumReview);
        }

        // POST: AlbumReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AlbumReview albumReview = db.AlbumReviews.Find(id);
            db.AlbumReviews.Remove(albumReview);
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
