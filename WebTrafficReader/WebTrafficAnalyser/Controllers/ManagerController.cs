using ManagerService.Competition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTrafficAnalyser.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Competition()
        {
            var standings = new StandingsApi().GetStandings();

            return View(standings);
        }

        public ActionResult Team()
        {
            return View();
        }
    }
}