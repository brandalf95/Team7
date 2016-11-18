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
    public class ArtistReviewsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: ArtistReviews
        public ActionResult Index()
        {
            return View(db.ArtistReviews.ToList());
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArtistReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistReviewID,Rating,Comment")] ArtistReview artistReview)
        {

            if (ModelState.IsValid)
            {
                db.ArtistReviews.Add(artistReview);
                db.SaveChanges();
                return RedirectToAction("Index");
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
