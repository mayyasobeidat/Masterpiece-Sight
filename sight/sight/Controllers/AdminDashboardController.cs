using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sight.Controllers
{
    public class AdminDashboardController : Controller
    {
        // GET: AdminDashboard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Services()
        {
            return View();
        }
        public ActionResult CreateNewService()
        {
            return View();
        }

        public ActionResult Photographers()
        {
            return View();
        }

        public ActionResult Users()
        {
            return View();
        }
        public ActionResult Gallery()
        {
            return View();
        }
        public ActionResult AddNewPhoto()
        {
            return View();
        }
        public ActionResult Statistics()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}