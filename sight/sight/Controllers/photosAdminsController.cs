using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sight.Models;

namespace sight.Controllers
{
    [HandleError(View = "Error")]
    [Authorize(Roles = "Admin")]
    public class photosAdminsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photosAdmins
        public ActionResult Index()
        {

            var photosAdmins = db.photosAdmins.Include(p => p.photographer).Include(p => p.PhotographyType1).OrderByDescending(x => x.id);
            return View(photosAdmins.ToList());
        }

        // GET: photosAdmins/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            //if (photosAdmin == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photosAdmin);
        }

        // GET: photosAdmins/Create
        public ActionResult Create()
        {
            ViewBag.photographerProfile = new SelectList(db.photographers, "id", "id");
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName");
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: photosAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographerProfile,photo,created_at,photographyType,title,photographerName,photographerID")] photosAdmin photosAdmin, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                string imgPath = "";
                if (photo != null)
                {
                    imgPath = Path.GetFileName(photo.FileName);
                    photo.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/portfolio/") + photo.FileName));
                }

                photosAdmin.photo = imgPath;
                db.photosAdmins.Add(photosAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins/Edit/5
        public ActionResult Edit(int? id)
        {

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            Session["photo"] = photosAdmin.photo;

            //if (photosAdmin == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // POST: photosAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographerProfile,photo,created_at,photographyType,title,photographerName,photographerID")] photosAdmin photosAdmin, HttpPostedFileBase photo)
        {
            photosAdmin.photo = Session["photo"].ToString();

            if (ModelState.IsValid)
            {
                string imgPath = "";
                if (photo != null)
                {
                    imgPath = Path.GetFileName(photo.FileName);
                    photo.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/portfolio/") + photo.FileName));
                    photosAdmin.photo = imgPath;

                }

                db.Entry(photosAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photosAdmin.photographerID);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photosAdmin photosAdmin = db.photosAdmins.Find(id);
            //if (photosAdmin == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photosAdmin);
        }

        // POST: photosAdmins/Delete/5
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
