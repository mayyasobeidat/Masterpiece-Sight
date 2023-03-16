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
using sight.Models;

namespace sight.Controllers
{
    public class photographersController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photographers
        public ActionResult Index()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }
        public ActionResult Pays()
        {
            return View();
        }

        public ActionResult photographers()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }

        public ActionResult PhotographerProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photographer photographer = db.photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }

            return View(photographer);
        }


        // GET: photographers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            photographer photographer = db.photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            return View(photographer);
        }

        // GET: photographers/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: photographers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,FullName,subscription_type,profilePhoto,coverPhoto,bio,accept,is_hidden,created_at,age,instagram,facebook,twitter,linkedin,PhoneNumber")] photographer photographer)
        {
            if (ModelState.IsValid)
            {
                db.photographers.Add(photographer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", photographer.user_id);
            return View(photographer);
        }

        // GET: photographers/Edit/5
        public ActionResult Edit(int? id)
        {
            var x = User.Identity.GetUserId();
            id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            photographer photographer = db.photographers.Find(id);
            Session["coverPhoto"] = photographer.coverPhoto;
            Session["profilePhoto"] = photographer.profilePhoto;
            if (photographer == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", photographer.user_id);
            return View(photographer);
        }

        // POST: photographers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,FullName,subscription_type,profilePhoto,coverPhoto,bio,accept,is_hidden,age,instagram,facebook,twitter,linkedin,PhoneNumber")] photographer photographer, HttpPostedFileBase coverPhoto, HttpPostedFileBase profilePhoto)
        {
            photographer.profilePhoto = Session["profilePhoto"].ToString();
            photographer.coverPhoto = Session["coverPhoto"].ToString();
            if (ModelState.IsValid)
            {
                string imgPath = "";
                string cvPath = "";
                if (profilePhoto != null)
                {
                    imgPath = Path.GetFileName(profilePhoto.FileName);
                    profilePhoto.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/") + profilePhoto.FileName));
                    photographer.profilePhoto = imgPath;
                }

                if (coverPhoto != null)
                {
                    cvPath = Path.GetFileName(coverPhoto.FileName);
                    coverPhoto.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/") + coverPhoto.FileName));
                    photographer.coverPhoto = cvPath;
                }
                db.Entry(photographer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", photographer.user_id);
            return View(photographer);
        }


        // POST: photographers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            photographer photographer = db.photographers.Find(id);
            db.photographers.Remove(photographer);
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
