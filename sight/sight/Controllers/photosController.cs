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
    public class photosController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photos
        public ActionResult Index()
        {
            var x = User.Identity.GetUserId();
            int iduser = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.photographersID = iduser;

            var photos = db.photos.Include(p => p.photographer).Include(p => p.PhotographyType1).Where(p => p.photographer_id == iduser).OrderByDescending(p => p.id);
            ;
            return View(photos.ToList());
        }

        // GET: photos/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photo photo = db.photos.Find(id);
            //if (photo == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photo);
        }

        // GET: photos/Create
        public ActionResult Create(int? id)
        {
            var x = User.Identity.GetUserId();
            int iduser = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.photographersID = iduser;



            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName");
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographer_id,photo_url,created_at,photographyType,title")] photo photo, HttpPostedFileBase photo_url, int? id)
        {
            if (ModelState.IsValid)
            {
                var x = User.Identity.GetUserId();
                int iduser = db.photographers.FirstOrDefault(a => a.user_id == x).id;
                ViewBag.photographersID = iduser;

                photo.photographer_id = iduser;
                photo.created_at = DateTime.Now;


                string imgPath = "";
                if (photo_url != null)
                {
               
                    imgPath = Path.GetFileName(photo_url.FileName);
                    photo_url.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/portfolio/") + photo_url.FileName));
                }

                photo.photo_url = imgPath;

                db.photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.photographerID = new SelectList(db.photographers, "id", "FullName", photo.photographer_id);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo.photographyType);
            return View(photo);
        }

        // GET: photos/Edit/5
        public ActionResult Edit(int? id)
        {
            var x = User.Identity.GetUserId();
            int iduser = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.photographersID = iduser;

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photo photo = db.photos.Find(id);
            Session["IMG"] = photo.photo_url;
            Session["iduser"] = iduser;



            //if (photo == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", photo.photographer_id);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo.photographyType);
            return View(photo);
        }

        // POST: photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographer_id,photo_url,created_at,photographyType,title")] photo photo, HttpPostedFileBase photo_url)
        {
            photo.photo_url = Session["IMG"].ToString();
            photo.photographer_id = (int)Session["iduser"];

            if (ModelState.IsValid)
            {
                string imgPath = "";
                if (photo_url != null)
                {
                    imgPath = Path.GetFileName(photo_url.FileName);
                    photo_url.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/portfolio/") + photo_url.FileName));
                    photo.photo_url = imgPath;
                }
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", photo.photographer_id);
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photo.photographyType);
            return View(photo);
        }

        // GET: photos/Delete/5
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            photo photo = db.photos.Find(id);
            //if (photo == null)
            //{
            //    return HttpNotFound();
            //}
            return View(photo);
        }

        // POST: photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            photo photo = db.photos.Find(id);
            db.photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Create");
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
