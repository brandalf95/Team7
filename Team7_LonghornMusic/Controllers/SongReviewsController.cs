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
    public class SongReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: SongReviews
        public ActionResult Index()
        {
            return View(db.SongReviews.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: SongReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SongReviewID,Rating,Comment")] SongReview songReview, string UserID, Int32 SongID)
        {
            songReview.Song = db.Songs.Find(SongID);
            List<AppUser> theseUsers = new List<AppUser>();
            theseUsers = db.Users.Where(a => a.UserName.Contains(UserID)).ToList();
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
