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
using System.Threading.Tasks;


namespace Team7_LonghornMusic.Controllers
{
    public class AppUsersController : Controller
    {
        private AppDbContext db = new AppDbContext();
        //Customer role code 

        private AppSignInManager _signInManager;
        private AppUserManager _userManager;

        public AppUsersController()
        {
        }

        public AppUsersController(AppUserManager userManager, AppSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: AppUsers
        public ActionResult Index()
        {
            var query = from c in db.Users
                        select c;

            //var roleManager = new RoleManager<AppRole>(new RoleStore(db));
            //var customerRole = roleManager.FindByName("Customer");
            List<AppUser> customerList = query.ToList();
            customerList = db.Users.ToList().Where(x => UserManager.IsInRole(x.Id, "Customer")).ToList();

            return View(customerList);
        }

        public ActionResult EmployeeIndex()
        {
            var query = from c in db.Users
                        select c;

            //var roleManager = new RoleManager<AppRole>(new RoleStore(db));
            //var customerRole = roleManager.FindByName("Customer");
            List<AppUser> employeeList = query.ToList();
            employeeList = db.Users.ToList().Where(x => UserManager.IsInRole(x.Id, "Employee")).ToList();

            return View(employeeList);
        }

        public ActionResult MyAccountIndex()
        {
            var query = from c in db.Users
                        select c;

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();

            List<AppUser> myList = query.ToList();
            myList = db.Users.ToList().Where(c => c.Id == userId).ToList();

            return View(myList);
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

        //// GET: AppUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUser model)
        {
            if (ModelState.IsValid)
            {
                //db.Users.Add(appUser);
                //db.SaveChanges();
                //return RedirectToAction("Index");
                var user = new AppUser { UserName = model.Email, Email = model.Email, FName = model.FName, LName = model.FName, MidInitial = model.MidInitial, PhoneNumber = model.PhoneNumber, Address = model.Address, City = model.City, State = model.State, ZipCode = model.ZipCode, CreditCardOne = model.CreditCardOne, CreditCardTypeOne = model.CreditCardTypeOne };

                //Add the new user to the database
                var result = await UserManager.CreateAsync(user);//Add password in parenthesis



                if (result.Succeeded) //user was created successfully
                {
                    await UserManager.AddToRoleAsync(user.Id, "Employee");

                    //sign the user in
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    //send them to the home page
                    return RedirectToAction("Index", "Home");

                }

            }
            return View(model);
        }

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
                appUserToChange.MidInitial = appUser.MidInitial;
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
                
                if(User.IsInRole("Customer"))
                {
                    return RedirectToAction("MyAccountIndex", "Home");
                }

                if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("EmployeeIndex");
                }

                if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index");
                }

                

            }

            return View(appUser);
        }

        public ActionResult EditEmployee(string id)
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
        public ActionResult EditEmployee([Bind(Include = "Id,Address,City,State,ZipCode,PhoneNumber")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                AppUser appUserToChange = db.Users.Find(appUser.Id);

                appUserToChange.Address = appUser.Address;
                appUserToChange.City = appUser.City;
                appUserToChange.State = appUser.State;
                appUserToChange.ZipCode = appUser.ZipCode;
                appUserToChange.PhoneNumber = appUser.PhoneNumber;

                db.Entry(appUserToChange).State = EntityState.Modified;
                db.SaveChanges();

                
                if (User.IsInRole("Employee"))
                {
                    return RedirectToAction("Index","Home");
                }



            }

            return RedirectToAction("MyAccountIndex","AppUsers");
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
