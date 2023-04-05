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
    public class photosAdmins1Controller : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photosAdmins1
        public ActionResult Index()
        {
            var photosAdmins = db.photosAdmins.Include(p => p.photographer).Include(p => p.PhotographyType1);
            return View(photosAdmins.ToList());
        }

        // GET: photosAdmins1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            if (photosAdmin == null)
            {
                return HttpNotFound();
            }
            return View(photosAdmin);
        }

        // GET: photosAdmins1/Create
        public ActionResult Create()
        {
            //ViewBag.photographerProfile = new SelectList(db.photographers, "id", "id");
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName");
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: photosAdmins1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographerProfile,photo,created_at,photographyType,title,photographerName,photographerID")] photosAdmin photosAdmin)
        {
            if (ModelState.IsValid)
            {
                db.photosAdmins.Add(photosAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            if (photosAdmin == null)
            {
                return HttpNotFound();
            }
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // POST: photosAdmins1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographerProfile,photo,created_at,photographyType,title,photographerName,photographerID")] photosAdmin photosAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photosAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            if (photosAdmin == null)
            {
                return HttpNotFound();
            }
            return View(photosAdmin);
        }

        // POST: photosAdmins1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            db.photosAdmins.Remove(photosAdmin);
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
