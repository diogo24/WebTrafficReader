﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTrafficAnalyser.Controllers
{
    public class CalculationsController : Controller
    {
        // GET: Calculations
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MealValueCalculation()
        {
            return View();
        }
    }
}