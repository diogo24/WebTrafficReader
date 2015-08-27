using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTrafficAnalyser.Controllers
{
    public class TrafficController : Controller
    {
        // GET: Traffic
        public ActionResult Index()
        {
            return View();
        }
    }
}