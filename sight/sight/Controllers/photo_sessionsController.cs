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
    public class photo_sessionsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photo_sessions
        public ActionResult Index()
        {
            var photo_sessions = db.photo_sessions.Include(p => p.city).Include(p => p.client).Include(p => p.photographer).Include(p => p.PhotographyType);
            return View(photo_sessions.ToList());
        }

        // GET: photo_sessions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photo_sessions photo_sessions = db.photo_sessions.Find(id);
            if (photo_sessions == null)
            {
                return HttpNotFound();
            }
            return View(photo_sessions);
        }

        // GET: photo_sessions/Create
        public ActionResult Create()
        {
            ViewBag.city_id = new SelectList(db.cities, "id", "cityName");
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id");
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id");
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: photo_sessions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographer_id,client_id,city_id,TypeID,session_date,session_time,status,created_at")] photo_sessions photo_sessions)
        {
            if (ModelState.IsValid)
            {
                db.photo_sessions.Add(photo_sessions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.city_id = new SelectList(db.cities, "id", "cityName", photo_sessions.city_id);
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", photo_sessions.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", photo_sessions.photographer_id);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo_sessions.TypeID);
            return View(photo_sessions);
        }

        // GET: photo_sessions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photo_sessions photo_sessions = db.photo_sessions.Find(id);
            if (photo_sessions == null)
            {
                return HttpNotFound();
            }
            ViewBag.city_id = new SelectList(db.cities, "id", "cityName", photo_sessions.city_id);
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", photo_sessions.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", photo_sessions.photographer_id);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo_sessions.TypeID);
            return View(photo_sessions);
        }

        // POST: photo_sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographer_id,client_id,city_id,TypeID,session_date,session_time,status,created_at")] photo_sessions photo_sessions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo_sessions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.city_id = new SelectList(db.cities, "id", "cityName", photo_sessions.city_id);
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", photo_sessions.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", photo_sessions.photographer_id);
            ViewBag.TypeID = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo_sessions.TypeID);
            return View(photo_sessions);
        }

        // GET: photo_sessions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photo_sessions photo_sessions = db.photo_sessions.Find(id);
            if (photo_sessions == null)
            {
                return HttpNotFound();
            }
            return View(photo_sessions);
        }

        // POST: photo_sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            photo_sessions photo_sessions = db.photo_sessions.Find(id);
            db.photo_sessions.Remove(photo_sessions);
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
