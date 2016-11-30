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
    public class OrderDetailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: OrderDetails

        public ActionResult Index()

        {
            
                List<OrderDetail> details = new List<OrderDetail>();
                details = db.OrderDetails.ToList();
                return View(details);
            
           
        }

        // GET: OrderDetails/Details/5
        public ActionResult ShoppingCart(string UserName, string error)
        {
            if(error == null)
            {
                ViewBag.Error = "";
            }else
            {
                ViewBag.Error = error;
            }
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            OrderDetail orderDetail = new OrderDetail();
            List<OrderDetail> listDetails = new List<OrderDetail>();
            listDetails = db.OrderDetails.Where(a => a.User.UserName.Contains(UserName)).ToList();
            foreach(OrderDetail item in listDetails)
            {
                if (!item.IsConfirmed)
                {
                    orderDetail = item;
                }
            }
            ShoppingCartViewModel shoppingCart = new ShoppingCartViewModel();
            shoppingCart.OrderDetail = orderDetail;

            shoppingCart.SubTotal = CalcSubTotal(shoppingCart);
            shoppingCart.Tax = CalcTax(shoppingCart);
            shoppingCart.Total = (shoppingCart.Tax + shoppingCart.SubTotal);
            foreach (Discount item in shoppingCart.OrderDetail.Discounts)
            {
                if (item.Album != null)
                {
                    AvgAlbumRating album = new AvgAlbumRating();
                    album.Album = item.Album;
                    album.AvgRating = ComputeAlbumAverage(album.Album.AlbumID);
                    shoppingCart.avgAlbumRatings.Add(album);
                }
                else
                {
                    AvgSongRating song = new AvgSongRating();
                    song.Song = item.Song;
                    song.AvgRating = ComputeSongAverage(song.Song.SongID);
                    shoppingCart.avgSongRatings.Add(song);
                }


               

            }
            return View(shoppingCart);
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
        [Authorize(Roles = "Customer")]
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
                    List<AppUser> list = db.Users.Where(a => a.UserName.Contains(UserName)).ToList();
                    orderDetail.User = db.Users.FirstOrDefault(a => a.UserName.Contains(UserName));
                    db.OrderDetails.Add(orderDetail);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return RedirectToAction("Index","Home");
        }

        // GET: OrderDetails/Edit/5
        public ActionResult Checkout(Int32 id)
        {
            //if (shoppingCart.OrderDetail.Discounts == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            bool dummy = true;
            
            int i = orderDetail.Discounts.ToArray().Length-1;
            int j = i - 1;
            while (j>=0)
            {
               
                if (orderDetail.Discounts[i].Song != null)
                {
                    if (orderDetail.Discounts[j].Song != null)
                    {
                        if (orderDetail.Discounts[i].Song == orderDetail.Discounts[j].Song)
                        {
                            return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName, error = "You can't have duplicate songs." }); ;
                        }
                        
                    }

                }else
                {
                    
                    if (orderDetail.Discounts[j].Album != null)
                    {
                        int q = orderDetail.Discounts[j].Album.AlbumSongs.ToArray().Length-1;
                        while (q>=0)
                        {
                            if (orderDetail.Discounts[i].Song == orderDetail.Discounts[j].Album.AlbumSongs[q])
                            {
                                dummy = false;

                            }
                            q -= 1;

                        }
                    }
                }
                i -= 1;
                j -= 1;
                
            }
           
                return View(orderDetail);
         
            
        }

        [HttpPost]
        public ActionResult Checkout([Bind(Include = "OrderDetailID,GifteeEmail,CreditCardNumber")] OrderDetail shoppingCart)
        {
            
            
            db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardNumber = shoppingCart.CreditCardNumber;
            db.OrderDetails.Find(shoppingCart.OrderDetailID).GifteeEmail = shoppingCart.GifteeEmail;
            db.OrderDetails.Find(shoppingCart.OrderDetailID).GifterEmail = db.OrderDetails.Find(shoppingCart.OrderDetailID).User.UserName;
            db.SaveChanges();
            shoppingCart = db.OrderDetails.Find(shoppingCart.OrderDetailID);

            return RedirectToAction("Confirm", shoppingCart);
        }

        public ActionResult Confirm(OrderDetail shoppingCart)
        {
            ShoppingCartViewModel newShoppingCart = new ShoppingCartViewModel();
            newShoppingCart.OrderDetail = db.OrderDetails.Find(shoppingCart.OrderDetailID);
            newShoppingCart.SubTotal = CalcSubTotal(newShoppingCart);
            newShoppingCart.Tax = CalcTax(newShoppingCart);
            newShoppingCart.Total = newShoppingCart.SubTotal + newShoppingCart.Tax;
            

            return View(newShoppingCart);
        }

        public ActionResult ConfirmScreen(Int32 id)
        {
            OrderDetail shoppingCart = db.OrderDetails.Find(id);
            if(shoppingCart.GifteeEmail.Length > 1)
            {
                db.OrderDetails.Find(shoppingCart.OrderDetailID).User = db.Users.FirstOrDefault(a=>a.UserName.Contains(shoppingCart.GifteeEmail));
            }
            db.OrderDetails.Find(shoppingCart.OrderDetailID).IsConfirmed = true;
            db.SaveChanges();
         

            return View();
        }
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
        public ActionResult Delete(int OrderID, int? SongID, int? AlbumID)
        {
            if (SongID == null)
            {
                if(AlbumID != null)
                {
                    db.OrderDetails.Find(OrderID).Discounts.Remove(db.OrderDetails.Find(OrderID).Discounts.FirstOrDefault(a => a.Album.AlbumID.Equals(AlbumID))); 
                }

            }else
            {
                if(SongID != null)
                {
                    db.OrderDetails.Find(OrderID).Discounts.Remove(db.OrderDetails.Find(OrderID).Discounts.FirstOrDefault(a => a.Song.SongID.Equals(SongID)));
                }
            }
            db.SaveChanges();
            OrderDetail orderDetail = db.OrderDetails.Find(OrderID);
            if (orderDetail == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName });
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
        public decimal CalcSubTotal(ShoppingCartViewModel orderDetail)
        {
            List<decimal> songList = new List<decimal>();
            List<decimal> albumList = new List<decimal>();
            List<decimal> discountList = new List<decimal>();
            List<Discount> list = new List<Discount>();
            list = orderDetail.OrderDetail.Discounts.ToList();
            foreach(Discount item in list)
            {
                if(item.Song == null)
                {
                    albumList.Add(item.Album.AlbumPrice);
                }else
                {
                    songList.Add(item.Song.SongPrice);
                }
                discountList.Add(item.DiscountAmt);
            }
            decimal subtotal = new decimal();
            foreach(decimal item in albumList)
            {
                subtotal += item;
            }
            foreach (decimal item in songList)
            {
                subtotal += item;
            }
            foreach (decimal item in discountList)
            {
                subtotal -= item;
            }
            return subtotal;

        }
        public decimal CalcTax(ShoppingCartViewModel shoppingCart)
        {
            decimal tax = new decimal();
            tax = Convert.ToDecimal(.0825) * shoppingCart.SubTotal;
            return tax;
        }
        public decimal ComputeSongAverage(Int32 Song)
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
        public decimal ComputeAlbumAverage(Int32 Artist)
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
