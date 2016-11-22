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
    public class OrderDetailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: OrderDetails
        public ActionResult Index()
        {
            return View(db.OrderDetails.ToList());
        }

        // GET: OrderDetails/Details/5
        public ActionResult ShoppingCart(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = new OrderDetail();
            List<OrderDetail> listDetails = new List<OrderDetail>();
            listDetails = db.OrderDetails.Where(a => a.User.UserName.Contains(id)).ToList();
            foreach(OrderDetail item in listDetails)
            {
                if (!item.IsConfirmed)
                {
                    orderDetail = item;
                }
            }
            

            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddToCart(Int32? SongID, Int32? AlbumID, string UserName)
        {

            if (ModelState.IsValid)
            {
                OrderDetail orderDetail = new OrderDetail();
                List<OrderDetail> listDetails = new List<OrderDetail>();
                listDetails = db.OrderDetails.Where(a => a.User.UserName.Contains(UserName)).ToList();
                bool dummy = new bool();
                dummy = true;
                foreach (OrderDetail item in listDetails)
                {
                    if (!item.IsConfirmed)
                    {
                        orderDetail = item;
                        dummy = false;
                    }
                }
                Discount newSong = new Discount();
                Discount newAlbum = new Discount();
                if (AlbumID != null)
                {


                    if (!dummy)
                    {
                        Album album = new Album();
                        album = db.Albums.Find(AlbumID);
                        newAlbum.Album = album;
                        db.OrderDetails.Find(orderDetail.OrderDetailID).Discounts.Add(newAlbum);
                        
                    }
                    else
                    {
                        Album album = new Album();
                        album = db.Albums.Find(AlbumID);
                        newAlbum.Album = album;
                        orderDetail.Discounts.Add(newAlbum);
                    }

                }
                if (SongID != null)
                {
                    
                   
                    if (!dummy)
                    {
                        Song song = new Song();
                        song = db.Songs.Find(SongID);
                        newSong.Song = song;
                        db.OrderDetails.Find(orderDetail.OrderDetailID).Discounts.Add(newSong);
                    }
                    else
                    {
   
                        Song song = new Song();
                        song = db.Songs.Find(SongID);
                        newSong.Song = song;
                        if (newSong != null)
                        {
                            orderDetail.Discounts.Add(newSong);
                        }
                    }
                }

                if (dummy)
                {
                    db.OrderDetails.Add(orderDetail);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderDetailID,IsGift,IsConfirmed,IsRefunded")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetail);
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
