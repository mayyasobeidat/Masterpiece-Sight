using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sight.Controllers
{
    public class UsersPagesController : Controller
    {
        // GET: UsersPages
        public ActionResult Index()
        {
            return View();
        }
    }
}