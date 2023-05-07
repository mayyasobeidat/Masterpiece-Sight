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
    public class PhotographerTypesController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: PhotographerTypes
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var photographerTypes = db.PhotographerTypes.Include(p => p.photographer)
                .Include(p => p.PhotographyType)
                .Where(p => p.photographer.user_id == userId)
                .ToList();
            return View(photographerTypes);

        }

        // GET: PhotographerTypes/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            PhotographerType photographerType = db.PhotographerTypes.Find(id);
            //if (photographerType == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photographerType);
        }

        // GET: PhotographerTypes/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id");
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: PhotographerTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotographerID,TypeID,note")] PhotographerType photographerType)
        {
            if (ModelState.IsValid)
            {
                db.PhotographerTypes.Add(photographerType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerType.PhotographerID);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerType.TypeID);
            return View(photographerType);
        }

        // GET: PhotographerTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            PhotographerType photographerType = db.PhotographerTypes.Find(id);
            //if (photographerType == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerType.PhotographerID);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerType.TypeID);
            return View(photographerType);
        }

        // POST: PhotographerTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotographerID,TypeID,note")] PhotographerType photographerType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photographerType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhotographerID = new SelectList(db.photographers, "id", "user_id", photographerType.PhotographerID);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photographerType.TypeID);
            return View(photographerType);
        }

        // GET: PhotographerTypes/Delete/5
        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            //    if (id == null)
            //    {
            //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //    }
            PhotographerType photographerType = db.PhotographerTypes.Find(id);
            //if (photographerType == null)
            //{
            //    return HttpNotFound();
            //}

            return View(photographerType);
        }

        // POST: PhotographerTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotographerType photographerType = db.PhotographerTypes.Find(id);
            db.PhotographerTypes.Remove(photographerType);
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
