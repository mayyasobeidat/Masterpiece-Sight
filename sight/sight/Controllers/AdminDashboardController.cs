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
            var allCount = db.photo_sessions.Count();
            ViewBag.allCount = allCount;

            var cities = db.cities.ToList(); // جلب جميع المدن

            foreach (var city in cities) // حلقة لجلب بيانات كل مدينة
            {
                var cityName = city.cityName;

                int cityCount = db.photographers
                                  .Include(p => p.photographer_cities)
                                  .Where(p => p.photographer_cities.Any(c => c.city.cityName == cityName))
                                  .Count();
                ViewBag.cityCount = cityCount;

                // حساب النسبة المئوية لكل مدينة
                decimal cityPercentage = cityCount > 0 ? Math.Round((decimal)cityCount / allCount * 100, 1) : 0;


            }


            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult MostPopulerCities()
        {
            int TotalSale = db.photo_sessions.Count();
            var PopulerCityCount = db.photo_sessions.GroupBy(a => a.city_id).Select(g => new PopulerCities { CitiesID = g.Key, CitiesCount = g.Select(x => x.client_id).Count() }).ToList();
            int cityNum = db.cities.Select(a => a.cityName).Count();


            var cityID = db.cities.ToList();
            string[] ArrayCity = db.cities.Select(a => a.cityName).ToArray();
            double[] cityper = new double[cityNum];
            int c = 0;

            foreach (var city in cityID)
            {
                float result = 0;
                foreach (var item1 in PopulerCityCount)
                {
                    if (city.id == item1.CitiesID)
                    {
                        float avg = Convert.ToInt32(TotalSale);
                        float coun = Convert.ToInt32(item1.CitiesCount);
                        result = (coun / avg) * 100;
                        string Perc = result.ToString("0.00");
                        double Finalpercent = Convert.ToDouble(Perc);
                        cityper[c] = Finalpercent;
                        c++;
                    }
                }
                if (result == 0)
                {
                    cityper[c] = 0;
                    c++;
                }
            }

            ViewBag.City = cityper;
            ViewBag.PP = ArrayCity;
            var myData = new
            {
              
                labels = ArrayCity,
                values = cityper
            };

            return Json(myData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult MostPopulerTypes()
        {
            int TotalType = db.photo_sessions.Count();
            var PopulerTypeCount = db.photo_sessions.GroupBy(a => a.TypeID).Select(g => new PopulerTypes { TypesID = g.Key, TypesCount = g.Select(x => x.client_id).Count() }).ToList();
            int typeNum = db.PhotographyTypes.Select(a => a.TypeName).Count();


            var typeID = db.PhotographyTypes.ToList();
            string[] ArrayType = db.PhotographyTypes.Select(a => a.TypeName).ToArray();
            double[] typeper = new double[typeNum];
            int c = 0;

            foreach (var type in typeID)
            {
                float result = 0;
                foreach (var item1 in PopulerTypeCount)
                {
                    if (type.TypeID == item1.TypesID)
                    {
                        float avg = Convert.ToInt32(TotalType);
                        float coun = Convert.ToInt32(item1.TypesID);
                        result = (coun / avg) * 100;
                        string Perc = result.ToString("0.00");
                        double Finalpercent = Convert.ToDouble(Perc);
                        typeper[c] = Finalpercent;
                        c++;
                    }
                }
                if (result == 0)
                {
                    typeper[c] = 0;
                    c++;
                }
            }

            ViewBag.City = typeper;
            ViewBag.PP = ArrayType;
            var myDatas = new
            {

                labels = ArrayType,
                values = typeper
            };
            return Json(myDatas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PhotoSessions()
        {
            double Total = db.photo_sessions.Count();

            string[] Sessions = new string[] { "Canceled", "Waiting", "Finished" };
            double Waiting = db.photo_sessions.Where(a => a.caase == 1).Count();
            double Finished = db.photo_sessions.Where(a => a.caase == 2).Count();
            double Canceled = db.photo_sessions.Where(a => a.caase == 3).Count();




            double Progresult = (Waiting / Total) * 100;
            string PerWait = Progresult.ToString("0.00");
            double waitpercent = Convert.ToDouble(PerWait);

            double doneresult = (Finished / Total) * 100;
            string Perfinish = doneresult.ToString("0.00");
            double finishpercent = Convert.ToDouble(Perfinish);

            double Canresult = (Canceled / Total) * 100;
            string PerCan = Canresult.ToString("0.00");
            double Canpercent = Convert.ToDouble(PerCan);

            double[] percetage = new double[] { Canpercent, waitpercent, finishpercent };

            var myData = new
            {
                labels = Sessions,
                values = percetage,
                backgroundColor = new string[] { "#c81313", "#F2C511", "#8FD14F" }
            };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Photographers()
        {
            double Total = db.photographers.Count();

            string[] State = new string[] { "Active", "Blocked", "Deleted" , "Inactive" };
            double Deleted = db.photographers.Where(a => a.state == "deleted").Count();
            double Blocked = db.photographers.Where(a => a.state == "Block").Count();
            double Active= db.photographers.Where(a => a.state == null && a.is_hidden == false && a.accept == true).Count();
            double Inactive = db.photographers.Where(a => a.state == null && a.is_hidden == true && a.accept == true).Count();




            double Deletedresult = (Deleted / Total) * 100;
            string PerDeleted = Deletedresult.ToString("0.00");
            double Deletedpercent = Convert.ToDouble(PerDeleted);

            double Blockedresult = (Blocked / Total) * 100;
            string PerBlocked = Blockedresult.ToString("0.00");
            double Blockedpercent = Convert.ToDouble(PerBlocked);

            double Activedresult = (Active / Total) * 100;
            string PerActived = Activedresult.ToString("0.00");
            double Activedpercent = Convert.ToDouble(PerActived);

            double inActivresult = (Inactive / Total) * 100;
            string PerInActive = inActivresult.ToString("0.00");
            double inActivePercent = Convert.ToDouble(PerInActive);


            double[] percetage = new double[] { Activedpercent, Blockedpercent, Deletedpercent, inActivePercent };

            var myData = new
            {
                labels = State,
                values = percetage,
                backgroundColor = new string[] { "#8FD14F", "#808080", "#c81313", "#F2C511" }
            };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Subscription()
        {
            double Total = db.Subscriptions.Count();

            string[] Sub = new string[] { "Free 7 Days", "One Month 50 JD", "Three Months 125 JD"};
            double Free = db.Subscriptions.Where(a => a.Price == 0).Count();
            double One = db.Subscriptions.Where(a => a.Price == 50).Count();
            double Three = db.Subscriptions.Where(a => a.Price == 125).Count();




            double Freeresult = (Free / Total) * 100;
            string PerFree = Freeresult.ToString("0.00");
            double Freepercent = Convert.ToDouble(PerFree);

            double Oneresult = (One / Total) * 100;
            string PerOne = Oneresult.ToString("0.00");
            double Onepercent = Convert.ToDouble(PerOne);

            double Threeresult = (Three / Total) * 100;
            string PerThree = Threeresult.ToString("0.00");
            double Threepercent = Convert.ToDouble(PerThree);


            double[] percetage = new double[] { Freepercent, Onepercent, Threepercent };

            var myData = new
            {
                labels = Sub,
                values = percetage,
                backgroundColor = new string[] { "#8FD14F", "#c81313", "#F2C511" }
            };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

    }
}