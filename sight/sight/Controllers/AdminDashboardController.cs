using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using sight.Models;

namespace sight.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        private sightEntities db = new sightEntities();


        public ActionResult photographersCount()
        {
            var count = db.photographers.Count();
            return Content(count.ToString());
        }

        public ActionResult clientsCount()
        {
            var count = db.clients.Count();
            return Content(count.ToString());
        }

        public ActionResult bookingCount()
        {
            var count = db.photo_sessions.Count();
            return Content(count.ToString());
        }

        public ActionResult usersCount()
        {
            var count = db.AspNetUsers.Count();
            return Content(count.ToString());
        }

        public ActionResult totalProfits()
        {
            var count = db.Subscriptions.Sum(s => s.Price);
            return Content(count.ToString());
        }

        public ActionResult totalSubscriptions()
        {
            var count = db.Subscriptions.Count();
            return Content(count.ToString());
        }


        public ActionResult Index()
        {
            return View();
        }

   

   
       





        public ActionResult Statistics()
        {
            int irbidCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid")).Count();
            int ammanCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Amman")).Count();
            int jarashCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Jarash")).Count();
            int ajlounCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Ajloun")).Count();
            int aqabaCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Al-aqaba")).Count();

            ViewBag.IrbidCount = irbidCount;
            ViewBag.AmmanCount = ammanCount;
            ViewBag.JarashCount = jarashCount;
            ViewBag.AjlounCount = ajlounCount;
            ViewBag.AqabaCount = aqabaCount;
            return View();
        }
        public ActionResult Statisticssss()
        {
            //var cityPhotographers = db.photographer_cities.GroupBy(pc => pc.city_id).Select(g => new { CityId = g.Key, Count = g.Count() });


            int irbidCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid")).Count();
            int ammanCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Amman")).Count();
            int jarashCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Jarash")).Count();
            int ajlounCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Ajloun")).Count();
            int aqabaCount = db.photographers.Include(p => p.photographer_cities).Where(p => p.photographer_cities.Any(c => c.city.cityName == "Al-aqaba")).Count();
        
            ViewBag.IrbidCount = irbidCount;
            ViewBag.AmmanCount = ammanCount;
            ViewBag.JarashCount = jarashCount;
            ViewBag.AjlounCount = ajlounCount;
            ViewBag.AqabaCount = aqabaCount;
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}