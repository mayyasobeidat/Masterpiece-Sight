using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using sight.Models;

namespace sight.Controllers
{
    public class SubscriptionsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: Subscriptions1
        public ActionResult Index()
        {
            var subscriptions = db.Subscriptions.Include(s => s.photographer);
            return View(subscriptions.ToList());
        }

        // GET: Subscriptions1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // GET: Subscriptions1/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerId = new SelectList(db.photographers, "id", "user_id");
            return View();
        }

        // POST: Subscriptions1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PhotographerId,startDate,endDate,Price,theCounter,status,cardNumber,cvv,cardExpiry")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                db.Subscriptions.Add(subscription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhotographerId = new SelectList(db.photographers, "id", "user_id", subscription.PhotographerId);
            return View(subscription);
        }

        // GET: Subscriptions1/Edit/5
        public ActionResult Edit()
        {
            var x = User.Identity.GetUserId();
            int id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.photographerid = id;

            //int userId = ViewBag.photographerid ; 
            //var Subscriptions = db.Subscriptions
            //    .Where(p => p.PhotographerId == userId)
            //    .FirstOrDefault(a => a.PhotographerId == userId).ID;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var subID = db.Subscriptions.FirstOrDefault(a => a.PhotographerId == id).ID;

            Subscription subscription = db.Subscriptions.Find(subID);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhotographerId = new SelectList(db.photographers, "id", "user_id", subscription.PhotographerId);
            return View(subscription);
        }

        // POST: Subscriptions1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PhotographerId,startDate,endDate,Price,theCounter,status,cardNumber,cvv,cardExpiry,cardName")] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                //subscription.startDate = DateTime.Now;
                subscription.endDate = subscription.startDate?.AddMonths(1) ?? DateTime.Now.AddMonths(1);
                subscription.Price = 100;
                subscription.status = false;

                //if (subscription.endDate.HasValue && subscription.startDate.HasValue && (subscription.endDate.Value - subscription.startDate.Value).Days == 0)
                //{
                //    subscription.status = false;
                //    ViewBag.DivDisplayStyle = "";

                //}
                //else
                //{
                //    subscription.status = true;
                //    ViewBag.DivDisplayStyle = "display:none;";

                //}

                //if (subscription.status == false)
                //{
                //    ViewBag.DivDisplayStyle = "";

                //}
                //else if (subscription.status == true)
                //{
                //    ViewBag.DivDisplayStyle = "display:none;";

                //}
                if (subscription.endDate < DateTime.Now)
                {
                    subscription.status = false;
                }
                if (subscription.startDate >= DateTime.Now && subscription.endDate >= DateTime.Now)
                {
                    subscription.status = true;
                }
                if (subscription.startDate >= DateTime.Now)
                {
                    subscription.status = false;
                }


                db.Entry(subscription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Photographers");
            }
            ViewBag.PhotographerId = new SelectList(db.photographers, "id", "user_id", subscription.PhotographerId);
            return View(subscription);
        }

        // GET: Subscriptions1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        // POST: Subscriptions1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
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
