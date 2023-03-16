using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sight.Models;

namespace sight.Controllers
{
    public class PhotographerPricingsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: PhotographerPricings

        public ActionResult Pay()
        {
            var photographerPricings = db.PhotographerPricings.Include(p => p.photographer);
            return View(photographerPricings.ToList());
        }
        public ActionResult Index()
        {
            var photographerPricings = db.PhotographerPricings.Include(p => p.photographer);
            return View(photographerPricings.ToList());
        }

        // GET: PhotographerPricings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            if (photographerPricing == null)
            {
                return HttpNotFound();
            }
            return View(photographerPricing);
        }

        // GET: PhotographerPricings/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id");
            return View();
        }

        // POST: PhotographerPricings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PhotographerID,PhotographyType,PriceOneHour,PriceOneAndHalfHour,PriceTwoHours")] PhotographerPricing photographerPricing)
        {
            if (ModelState.IsValid)
            {
                db.PhotographerPricings.Add(photographerPricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerPricing.PhotographerID);
            return View(photographerPricing);
        }

        // GET: PhotographerPricings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            if (photographerPricing == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerPricing.PhotographerID);
            return View(photographerPricing);
        }

        // POST: PhotographerPricings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PhotographerID,PhotographyType,PriceOneHour,PriceOneAndHalfHour,PriceTwoHours")] PhotographerPricing photographerPricing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photographerPricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerPricing.PhotographerID);
            return View(photographerPricing);
        }

        // GET: PhotographerPricings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            if (photographerPricing == null)
            {
                return HttpNotFound();
            }
            return View(photographerPricing);
        }

        // POST: PhotographerPricings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            db.PhotographerPricings.Remove(photographerPricing);
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
