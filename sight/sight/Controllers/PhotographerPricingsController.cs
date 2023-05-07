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
    public class PhotographerPricingsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: PhotographerPricings1



        [HandleError(View = "Error")]
        [Authorize(Roles = "Photographer")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var PhotographerPricings = db.PhotographerPricings.Include(p => p.photographer)
                .Include(p => p.PhotographyType)
                .Where(p => p.photographer.user_id == userId)
                .ToList();
            return View(PhotographerPricings.ToList());
        }

        // GET: PhotographerPricings1/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            //if (photographerPricing == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photographerPricing);
        }

        // GET: PhotographerPricings1/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "FullName");
            ViewBag.PhotographyTypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: PhotographerPricings1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PhotographerID,PhotographyTypeID,PriceOneHour,PriceOneAndHalfHour,PriceTwoHours")] PhotographerPricing photographerPricing)
        {
            if (ModelState.IsValid)
            {
                db.PhotographerPricings.Add(photographerPricing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "FullName", photographerPricing.PhotographerID);
            ViewBag.PhotographyTypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerPricing.PhotographyTypeID);
            return View(photographerPricing);
        }

        // GET: PhotographerPricings1/Edit/5


        [HandleError(View = "Error")]
        [Authorize(Roles = "Photographer")]
        public ActionResult Edit(int? id)
        {
         

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            Session["photographer"] = photographerPricing.PhotographerID;
            Session["type"] = photographerPricing.PhotographyTypeID;
            //if (photographerPricing == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.PhotographerID = new SelectList(db.photographers, "id", "FullName", photographerPricing.PhotographerID);
            //ViewBag.PhotographyTypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerPricing.PhotographyTypeID);
            return View(photographerPricing);
        }

        // POST: PhotographerPricings1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [HandleError(View = "Error")]
        [Authorize(Roles = "Photographer")]
        public ActionResult Edit([Bind(Include = "ID,PhotographerID,PhotographyTypeID,PriceOneHour,PriceOneAndHalfHour,PriceTwoHours")] PhotographerPricing photographerPricing)
        {

            if (ModelState.IsValid)
            {
                photographerPricing.PhotographerID = (int)Session["photographer"];
                photographerPricing.PhotographyTypeID = (int)Session["type"];
                db.Entry(photographerPricing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "photographersProfile");
            }
            //ViewBag.PhotographerID = new SelectList(db.photographers, "id", "FullName", photographerPricing.PhotographerID);
            //ViewBag.PhotographyTypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerPricing.PhotographyTypeID);
            return View(photographerPricing);
        }

        // GET: PhotographerPricings1/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            PhotographerPricing photographerPricing = db.PhotographerPricings.Find(id);
            //if (photographerPricing == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photographerPricing);
        }

        // POST: PhotographerPricings1/Delete/5
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
