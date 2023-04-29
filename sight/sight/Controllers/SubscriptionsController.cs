using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
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
        public ActionResult Edit([Bind(Include = "ID,PhotographerId,startDate,endDate,Price,theCounter,status,cardNumber,cvv,cardExpiry,cardName")] Subscription subscription,string Sub)
        {
            if (ModelState.IsValid)
            {
                // تحديث حالة المصور بعد الدفع
                var photographer = db.photographers.Find(subscription.PhotographerId);
                photographer.is_hidden = false;
                db.Entry(photographer).State = EntityState.Modified;


                
                if (Sub == "one")
                { 
                    subscription.Price = 50;
                    subscription.endDate = subscription.startDate?.AddMonths(1) ?? DateTime.Now.AddMonths(1);
                }
                else if (Sub == "three")
                { 
                    subscription.Price = 125;
                    subscription.endDate = subscription.startDate?.AddMonths(3) ?? DateTime.Now.AddMonths(3);
                }


                db.Entry(subscription).State = EntityState.Added;
                db.SaveChanges();
                return RedirectToAction("Edit", "photographersProfile");
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
