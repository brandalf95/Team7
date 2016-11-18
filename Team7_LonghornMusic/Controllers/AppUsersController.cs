﻿using System;
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
    public class AppUsersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: AppUsers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: AppUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.Users.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        //// GET: AppUsers/Create - No need for this method~~~~
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AppUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,FName,LName,MidInitial,IsDisabled,Address,City,State,ZipCode,CreditCardOne,CreditCardTypeOne,CreditCardTwo,CreditCardTypeTwo,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AppUser appUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.AppUsers.Add(appUser);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(appUser);
        //}

        // GET: AppUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.Users.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FName,LName,MidInitial,IsDisabled,Address,City,State,ZipCode,CreditCardOne,CreditCardTypeOne,CreditCardTwo,CreditCardTypeTwo,Email,PhoneNumber")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                AppUser appUserToChange = db.Users.Find(appUser.Id);


                appUserToChange.FName = appUser.FName;
                appUserToChange.LName = appUser.LName;
                appUserToChange.Email = appUser.Email;
                appUserToChange.PhoneNumber = appUser.PhoneNumber;
                appUserToChange.Address = appUser.Address;
                appUserToChange.City = appUser.City;
                appUserToChange.State = appUser.State;
                appUserToChange.ZipCode = appUser.ZipCode;
                appUserToChange.CreditCardOne = appUser.CreditCardOne;
                appUserToChange.CreditCardTwo = appUser.CreditCardTwo;
                appUserToChange.CreditCardTypeOne = appUser.CreditCardTypeOne;
                appUserToChange.CreditCardTypeTwo = appUser.CreditCardTypeTwo;

                if(User.IsInRole("Employee"))
                {
                    appUserToChange.IsDisabled = appUser.IsDisabled;

                }

                db.Entry(appUserToChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppUser appUser = db.Users.Find(id);
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AppUser appUser = db.Users.Find(id);
            db.Users.Remove(appUser);
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