﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Team7_LonghornMusic.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReviewHome()
        {
            return View();
        }

        public ActionResult ContentHome()
        {
            return View();
        }
    }
}