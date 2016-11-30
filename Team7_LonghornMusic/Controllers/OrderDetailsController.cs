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
        public ActionResult ShoppingCart(string UserName)
        {
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
            List<ShoppingCartViewModel> shoppingCartList = new List<ShoppingCartViewModel>();
            shoppingCartList.Add(shoppingCart);
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

    }
}
