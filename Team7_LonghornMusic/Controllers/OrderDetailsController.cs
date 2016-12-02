using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team7_LonghornMusic.Models;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Team7_LonghornMusic.Messaging;

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
            foreach(Discount item in orderDetail.Discounts)
            {
                if(item.Song == null && item.Album == null)
                {
                    db.OrderDetails.Find(orderDetail.OrderDetailID).Discounts.Remove(item);
                    db.SaveChanges();
                    orderDetail = db.OrderDetails.Find(orderDetail.OrderDetailID);
                }
            }
            ShoppingCartViewModel shoppingCart = new ShoppingCartViewModel();
            shoppingCart.OrderDetail = orderDetail;

            shoppingCart.SubTotal = CalcSubTotal(shoppingCart,false);
            shoppingCart.Tax = CalcTax(shoppingCart);
            shoppingCart.Total = (shoppingCart.Tax + shoppingCart.SubTotal);
            foreach (Discount item in shoppingCart.OrderDetail.Discounts)
            {
                if (item.Album != null)
                {
                    AvgAlbumRating album = new AvgAlbumRating();
                    album.Album = item.Album;
                    album.AvgRating = ComputeAlbumAverage(album.Album.AlbumID);
                    album.SavingsAmount = album.Album.AlbumPrice - album.Album.DisplayPrice;
                    shoppingCart.avgAlbumRatings.Add(album);
                }
                else
                {
                    AvgSongRating song = new AvgSongRating();
                    song.Song = item.Song;
                    song.AvgRating = ComputeSongAverage(song.Song.SongID);
                    song.SavingsAmount = song.Song.SongPrice - song.Song.DisplayPrice;
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
        public ActionResult Checkout(Int32 id, string error)
        {
            //if (shoppingCart.OrderDetail.Discounts == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            List<Song> TotalSongs = new List<Song>();
            TotalSongs = db.Songs.ToList();
            foreach (Song item in TotalSongs)
            {
                if (item.DiscountPrice != 0)
                {
                    item.DisplayPrice = item.DiscountPrice;
                }
                else
                {
                    item.DisplayPrice = item.SongPrice;
                }
                db.Songs.Find(item.SongID).DisplayPrice = item.DisplayPrice;
            }
            db.SaveChanges();
            if (error == null)
            {
                ViewBag.Error = "";
            }else
            {
                ViewBag.Error = error;
            }
            OrderDetail orderDetail = db.OrderDetails.Find(id);
            if (orderDetail.Discounts.ToArray().Length == 0)
            {
                return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName, error = "You cannot checkout with an empty shopping cart." });
            }
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
                            return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName, error = "You can't have duplicate songs or albums." });
                        }
                        
                    }
                    else
                    {

                        if (orderDetail.Discounts[j].Album != null)
                        {
                            int q = orderDetail.Discounts[j].Album.AlbumSongs.ToArray().Length - 1;
                            while (q >= 0)
                            {
                                if (orderDetail.Discounts[i].Song == orderDetail.Discounts[j].Album.AlbumSongs[q])
                                {
                                    return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName, error = "You can't have duplicate songs or albums." });

                                }
                                q -= 1;

                            }
                        }
                    }
                }else
                {
                    if (orderDetail.Discounts[j].Album != null)
                    {
                        return RedirectToAction("ShoppingCart", new { UserName = orderDetail.User.UserName, error = "You can't have duplicate songs or albums." });

                    }
                }
                i -= 1;
                j -= 1;
                
            }
            List<String> creditCardList = new List<String>();
            creditCardList.Add("2222111122221111");
            SelectList list = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Selected = true, Text = "None", Value = "0" },
                new SelectListItem {Selected = false, Text = HideCard(orderDetail.User.CreditCardOne), Value = "1" },
                new SelectListItem {Selected = false, Text = HideCard(orderDetail.User.CreditCardTwo), Value = "2" },
                 
            }, "Value", "Text" );
            SelectList typeList = new SelectList(new List<SelectListItem>
            {
                new SelectListItem {Selected = true, Text = "None", Value = "0" },
                new SelectListItem {Selected = false, Text = "Visa", Value = "1" },
                new SelectListItem {Selected = false, Text = "MasterCard", Value = "2" },
                new SelectListItem {Selected = false, Text = "Discover", Value = "3" },
                new SelectListItem {Selected = false, Text = "AmericanExpress", Value = "4" }
                 
            }, "Value", "Text");
            ViewBag.TypeList = (typeList);
            ViewBag.CreditCardList = (list);

                return View(orderDetail);
         
            
        }
        public string HideCard(string card)
        {
            if(card == null || card == "")
            {
                return "";
            }
            int d = 1;
            int e = 0;
             string starCard = "";
                while (d <= 15)
                {
                    if (d % 5 == 0 && d != 0)
                    {
                        starCard += " ";
                    }
                    else
                    {
                        starCard += "*";

                    }
                    d += 1;
                }
                int f = card.Length - 4;
                int r = card.Length;
                while (f != r)
                {
                    starCard += card[f];
                    f += 1;
                }
            return starCard;
            }
            
        

        [HttpPost]
        public ActionResult Checkout([Bind(Include = "OrderDetailID,GifteeEmail,CreditCardNumber")] OrderDetail shoppingCart, string CreditCardType, string OnFileCard)
        {

            if(db.Users.FirstOrDefault(a=>a.UserName.Contains(shoppingCart.GifteeEmail)) == null)
            {
                return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "You entered an email for a giftee that doesn't exist." });
            }
            if(db.OrderDetails.Find(shoppingCart.OrderDetailID).User.UserName == shoppingCart.GifteeEmail)
            {
                return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "You can't gift yourself you goober!" });
            }
            if (db.OrderDetails.Find(shoppingCart.OrderDetailID).User.IsDisabled)
            {
                return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "You cannot checkout due to your account being disabled." });

            }
            if (shoppingCart.GifteeEmail == null)
            {
                return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "Please specify whether or not it is to sent to a gift email or not." });
            
            }
            if((shoppingCart.CreditCardNumber != null || Convert.ToInt32(CreditCardType) != 0)&& Convert.ToInt32(OnFileCard) != 0)
            {
                return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "Only choose one type of card, one on file or enter one." });
            }
            else
            {
                if((shoppingCart.CreditCardNumber == null || Convert.ToInt32(CreditCardType) == 0) && Convert.ToInt32(OnFileCard) == 0)
                {
                    return RedirectToAction("Checkout", new { id = shoppingCart.OrderDetailID, error = "Must choose a card and type, unless choosing card on file." });
                }
            }
            if(Convert.ToInt32(OnFileCard) != 0)
            {
                if (Convert.ToInt32(OnFileCard) == 1)
                {
                    OrderDetail detail = new OrderDetail();
                    detail = db.OrderDetails.Find(shoppingCart.OrderDetailID);
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = db.Users.FirstOrDefault(a => a.UserName.Contains(detail.User.UserName)).CreditCardTypeOne.ToString();
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardNumber = db.Users.FirstOrDefault(a => a.UserName.Contains(detail.User.UserName)).CreditCardOne;
                    db.SaveChanges();
                }
                if (Convert.ToInt32(OnFileCard) == 2)
                {
                    OrderDetail detail = new OrderDetail();
                    detail = db.OrderDetails.Find(shoppingCart.OrderDetailID);
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = db.Users.FirstOrDefault(a => a.UserName.Contains(detail.User.UserName)).CreditCardTypeTwo.ToString();
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardNumber = db.Users.FirstOrDefault(a => a.UserName.Contains(detail.User.UserName)).CreditCardTwo;
                    db.SaveChanges();
                }
            }else
            {
                db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardNumber = shoppingCart.CreditCardNumber;
                if(Convert.ToInt32(OnFileCard) == 1)
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = "Visa";
                }
                if (Convert.ToInt32(OnFileCard) == 2)
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = "MasterCard";
                }
                if (Convert.ToInt32(OnFileCard) == 3)
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = "Discover";
                }
                if (Convert.ToInt32(OnFileCard) == 4)
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).CreditCardType = "AmericanExpress";
                }
            }
           
            db.OrderDetails.Find(shoppingCart.OrderDetailID).GifteeEmail = shoppingCart.GifteeEmail;
            db.OrderDetails.Find(shoppingCart.OrderDetailID).GifterEmail = db.OrderDetails.Find(shoppingCart.OrderDetailID).User.UserName;
            db.SaveChanges();
            shoppingCart = db.OrderDetails.Find(shoppingCart.OrderDetailID);

            return RedirectToAction("Confirm", new { OrderDetailID = shoppingCart.OrderDetailID });
        }

        //public ActionResult Report()
        //{

        //    List<Report> reports = new List<Report>();
        //    foreach (Song item in db.Songs.ToList())
        //    {
        //        Report report = new Report();
        //        report.Discount = new Discount();
        //        report.Discount.Song = item;
        //        foreach (Discount x in db.Discounts.ToList())
        //        {
        //            if (x.Song != null && x.Song == item && x.DiscountAmt != 0)
        //            {
        //                report.TotalPurchases += 1;
        //                report.Revenue += x.DiscountAmt;
        //            }
        //        }
        //        reports.Add(report);
        //    }
        //    foreach (Album item in db.Albums.ToList())
        //    {
        //        Report report = new Report();
        //        report.Discount = new Discount();
        //        report.Discount.Album = item;
        //        foreach (Discount x in db.Discounts.ToList())
        //        {
        //            if (x.Album != null && x.Album == item && x.DiscountAmt != 0)
        //            {
        //                report.TotalPurchases += 1;
        //                report.Revenue += x.DiscountAmt;
        //            }
        //        }
        //        reports.Add(report);
        //    }
        //    return View(reports);

        //}

        public ActionResult Refund(Int32 OrderDetailID)
        {

            return View(db.OrderDetails.Find(OrderDetailID));
        }

        public ActionResult RefundConfirm(Int32 OrderDetailID)
        {
            db.OrderDetails.Find(OrderDetailID).IsRefunded = true;
            db.SaveChanges();
            return View("Index","Home");
        }
        public ActionResult Report()
        {

            List<Report> reports = new List<Report>();
            foreach (Song item in db.Songs.ToList())
            {
                Report report = new Report();
                report.Discount = new Discount();
                report.Discount.Song = item;
                foreach (Discount x in db.Discounts.ToList())
                {
                    if (x.Song != null && x.Song == item && x.DiscountAmt != 0)
                    {
                        report.TotalPurchases += 1;
                        report.Revenue += x.DiscountAmt;
                    }
                }
                reports.Add(report);
            }

            return View(reports);

        }

        public ActionResult AlbumReport()
        {
            List<Report> reports = new List<Report>();
            foreach (Album item in db.Albums.ToList())
            {
                Report report = new Report();
                report.Discount = new Discount();
                report.Discount.Album = item;
                foreach (Discount x in db.Discounts.ToList())
                {
                    if (x.Album != null && x.Album == item && x.DiscountAmt != 0)
                    {
                        report.TotalPurchases += 1;
                        report.Revenue += x.DiscountAmt;
                    }
                }
                reports.Add(report);
            }
            return View(reports);
        }



        public ActionResult MyMusic(string UserName, string error, List<Song> list, string SearchString)
        {
        
            if (list != null)
            {
                return View(list);
            }
            if (error == null)
            {
                ViewBag.Error = "";
            }
            else
            {
                ViewBag.Error = error;
            }

            List<OrderDetail> orderList = new List<OrderDetail>();
            orderList = db.OrderDetails.Where(a => a.User.UserName.Contains(UserName)).ToList();
            List<OrderDetail> newOrderList = new List<OrderDetail>();
            foreach(OrderDetail item in orderList)
            {
                if (item.IsConfirmed == false)
                {
                    newOrderList.Remove(item);
                }
            }
            List<Song> SelectedSongs = new List<Song>();
            newOrderList = orderList;
            foreach(OrderDetail item in newOrderList)
            {
                foreach(Discount x in item.Discounts)
                {
                    if (x.Album != null)
                    {
                        foreach(Song y in x.Album.AlbumSongs)
                        {
                            SelectedSongs.Add(y);
                        }
                    }else
                    {
                        SelectedSongs.Add(x.Song);
                    }
                }
            }

            List<Song> TotalSongs = new List<Song>();
            TotalSongs = SelectedSongs.ToList();

            if (SearchString == null || SearchString == "")
            {
                SelectedSongs = SelectedSongs.ToList();
                ViewBag.SelectedSongCount = "Displaying " + SelectedSongs.Count() + "of" + TotalSongs;
            }

            else
            {
                SelectedSongs = SelectedSongs.Where(a => a.SongTitle.Contains(SearchString)).ToList();
                ViewBag.SelectedSongCount = "Displaying " + SelectedSongs.Count() + "of" + TotalSongs;
            }

            SelectedSongs = SelectedSongs.OrderBy(a => a.SongTitle).ToList();

            return View(SelectedSongs);
        }

        public ActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenres();
            ViewBag.AllArtists = GetAllArtists();
            ViewBag.AllAlbums = GetAllAlbums();
            return View();
        }


        public ActionResult SearchResults(string UserName, String SongSearchString, String AlbumSearchString, String ArtistSearchString, int[] SelectedGenres, SortBy SelectedSortBy, SortOrder SelectedSortOrder)
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
                DisplaySongs = (from a in DisplaySongs from al in a.SongAlbums.Distinct() orderby al.AlbumTitle select a).ToList();
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

            if (SelectedSortBy == SortBy.Genre && SelectedSortOrder == SortOrder.Ascending)
            {
                DisplaySongs = (from a in DisplaySongs from g in a.SongGenres.Distinct() orderby g.GenreName select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            if (SelectedSortBy == SortBy.Genre && SelectedSortOrder == SortOrder.Descending)
            {
                DisplaySongs = (from a in DisplaySongs from g in a.SongGenres.Distinct() orderby g.GenreName descending select a).ToList();
                DisplaySongs = DisplaySongs.Distinct().ToList();
            }

            ViewBag.SelectedSongCount = "Displaying " + DisplaySongs.Count() + " of " + TotalSongs.Count() + " Records";
            List<Song> list = new List<Song>();

            return View ("MyMusic", DisplaySongs );
        }

        public ActionResult OrderHistory(string UserName)
        {
            List<OrderDetail> orderList = new List<OrderDetail>();
            List<OrderDetail> newOrderList = new List<OrderDetail>();
            orderList = db.OrderDetails.Where(a => a.User.UserName.Contains(UserName)).ToList();

            foreach (OrderDetail item in orderList)
            {
                if (item.IsConfirmed == true && item.IsRefunded!= true)
                {
                    newOrderList.Add(item);
                }

            }
            List<ShoppingCartViewModel> orders = new List<ShoppingCartViewModel>();
            foreach(OrderDetail item in newOrderList)
            {
                ShoppingCartViewModel addOrder = new ShoppingCartViewModel();
                addOrder.OrderDetail = item;
                addOrder.SubTotal = CalcSubTotal(addOrder,true);
                addOrder.Tax = CalcTax(addOrder);
                addOrder.Total = (addOrder.SubTotal + addOrder.Tax);
                addOrder.DisplayCard = HideCard(item.CreditCardNumber);
                orders.Add(addOrder);
            }
            return View(orders);
        }

        public ActionResult Confirm(Int32 OrderDetailID)
        {
            ShoppingCartViewModel newShoppingCart = new ShoppingCartViewModel();
            newShoppingCart.OrderDetail = db.OrderDetails.Find(OrderDetailID);
            newShoppingCart.SubTotal = CalcSubTotal(newShoppingCart,false);
            newShoppingCart.Tax = CalcTax(newShoppingCart);
            newShoppingCart.Total = newShoppingCart.SubTotal + newShoppingCart.Tax;
            newShoppingCart.DisplayCard = newShoppingCart.OrderDetail.CreditCardNumber;
            

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
            foreach(Discount item in shoppingCart.Discounts){
                if(item.Album != null)
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).Discounts.FirstOrDefault(a=>a.DiscountID.Equals(item.DiscountID)).DiscountAmt = item.Album.DisplayPrice;
                }else
                {
                    db.OrderDetails.Find(shoppingCart.OrderDetailID).Discounts.FirstOrDefault(a => a.DiscountID.Equals(item.DiscountID)).DiscountAmt = item.Song.DisplayPrice;
                }
            }
            
            db.SaveChanges();
            StringBuilder strPurchasedItems = new StringBuilder();
            foreach (Discount item in shoppingCart.Discounts)
            {
                if (item.Album != null && item.Song != null)
                {
                    strPurchasedItems.Append(item.Song.SongTitle + ": $" + item.Song.SongPrice);
                    strPurchasedItems.AppendLine();
                    strPurchasedItems.Append(item.Album.AlbumTitle + ": $" + item.Album.AlbumPrice);
                }

                if (item.Album != null && item.Song == null)
                {
                    strPurchasedItems.Append(item.Album.AlbumTitle + ": $" + item.Album.AlbumPrice);
                    strPurchasedItems.AppendLine();
                }

                if (item.Album == null && item.Song != null)
                {
                    strPurchasedItems.Append(item.Song.SongTitle + ": $" + item.Song.SongPrice);
                    strPurchasedItems.AppendLine();
                }
            }

            if (shoppingCart.GifteeEmail.Length < 2)
            {
                EmailMessaging.SendEmail(shoppingCart.GifterEmail, "Thanks for the Purchase!", "You purchased the Following Items:    " + strPurchasedItems);
            }

            else
            {
                EmailMessaging.SendEmail(shoppingCart.GifterEmail, "Thanks for the Purchase!", "Your gift order has gone to " + shoppingCart.GifteeEmail);
                EmailMessaging.SendEmail(shoppingCart.GifteeEmail, "You have a gift!", "You have received the following items:    " + strPurchasedItems);
            }

            return View(db.OrderDetails.Find(id));
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
        public ActionResult Delete(int OrderID, Int32 discountID)
        {
            //if (SongID == null)
            //{
            //    if(AlbumID != null)
            //    {
            //        db.OrderDetails.Find(OrderID).Discounts.Remove(db.OrderDetails.Find(OrderID).Discounts.FirstOrDefault(a => a.Album.AlbumID.Equals(AlbumID))); 
            //    }

            //}else
            //{
            //    if(SongID != null)
            //    {
            //        db.OrderDetails.Find(OrderID).Discounts.Remove(Discount);
            //    }
            //}
            db.OrderDetails.Find(OrderID).Discounts.Remove(db.Discounts.Find(discountID));
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
        public decimal CalcSubTotal(ShoppingCartViewModel orderDetail, bool dummy)
        {
            List<Discount> list = new List<Discount>();
            list = orderDetail.OrderDetail.Discounts.ToList();
            List<decimal> albumList = new List<decimal>();
            List<decimal> songList = new List<decimal>();
            List<decimal> discountList = new List<decimal>();
            if (dummy)
            {
                
                decimal subtotal = new decimal();
                foreach (Discount item in list)
                {
                    subtotal += item.DiscountAmt;

                }


                return subtotal;
            }else
            {
                foreach (Discount item in list)
                {
                    if (item.Song == null)
                    {
                        albumList.Add(item.Album.DisplayPrice);
                    }
                    else
                    {
                        songList.Add(item.Song.DisplayPrice);
                    }
                    
                }
                decimal subtotal = new decimal();
                foreach (decimal item in albumList)
                {
                    subtotal += item;
                }
                foreach (decimal item in songList)
                {
                    subtotal += item;
                }
                return subtotal;
            }
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
        public void UpdateDiscountAmt(OrderDetail orderDetail)
        {
            
            foreach (Discount item in orderDetail.Discounts)
            {
                if (item.Album != null)
                {
                    db.Discounts.Find(item.DiscountID).DiscountAmt = item.Album.DisplayPrice;
                }
                else
                {
                    db.Discounts.Find(item.DiscountID).DiscountAmt = item.Song.DisplayPrice;
                }
            }
            db.SaveChanges();
        }
        public void MakeDiscountsInitial()
        {
            foreach(Song item in db.Songs.ToList())
            {
                Discount discount = new Discount();
                discount.Song = item;
                db.Discounts.Add(discount);
            }
            foreach(Album item in db.Albums.ToList())
            {
                Discount discount = new Discount();
                discount.Album = item;
                db.Discounts.Add(discount);
            }
            db.SaveChanges();
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

        public MultiSelectList GetAllArtists()
        {
            var query = from c in db.Artists
                        orderby c.ArtistName
                        select c;

            List<Artist> allArtists = query.ToList();

            MultiSelectList ArtistList = new MultiSelectList(allArtists.OrderBy(a => a.ArtistName), "ArtistID", "ArtistName");
            return ArtistList;
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
    }
}
