using sight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace sight.Controllers
{
    public class HomeController : Controller
    {

        private sightEntities db = new sightEntities();
        public ActionResult Index()
        {
            return View(db.PhotographyTypes.ToList());

        }

        public ActionResult ChoosePhotographer()
        {

            var city = db.cities;
            return View(city.ToList());

        }

        public ActionResult gallery()
        {
            var photos = db.photosAdmins;
            return View(photos.ToList());
        }

        public ActionResult commentsHome()
        {
            var comments = db.commentsHomes;
            return View(comments.ToList());
        }

      


        public PartialViewResult search(int city)
        {
            return PartialView();

        }



        public ActionResult Client()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Photographer()
        {
            ViewBag.city = new SelectList(db.cities.ToList(), "id", "cityName");
            ViewBag.type = new SelectList(db.PhotographyTypes.ToList(), "TypeID", "TypeName");

            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}