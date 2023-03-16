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
    public class photosAdminsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photosAdmins
        public ActionResult Index()
        {
            var photosAdmins = db.photosAdmins.Include(p => p.PhotographyType1);
            return View(photosAdmins.ToList());
        }

        // GET: photosAdmins/Details/5
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

        // GET: photosAdmins/Create
        public ActionResult Create()
        {

            //ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            //ViewBag.photographers = new SelectList(db.photographers, "id", "FullName");

            //var photographerList = db.photographers.Select(p => new SelectListItem { Value = p.id.ToString(), Text = "https://localhost:44339/photographers/PhotographerProfile/" + p.id }).ToList();
            //ViewBag.photographersProfile = photographerList;
            //photosAdmin photosAdmins = new photosAdmin();
            //photosAdmins.photographerProfile = "https://localhost:44339/photographers/PhotographerProfile/" + photosAdmins.photographerID;
            //photosAdmins.photo = "hi.png";
            //photosAdmins.created_at = DateTime.Now; // تعيين تاريخ التسجيل الحالي
            //int photographerID = Convert.ToInt32(Request.Form["photographers"]); // تحويل القيمة المرسلة من الدروب داون ليست إلى int
            //photosAdmins.photographerID = photographerID;
            //photosAdmins.photographerName = Request.Form["photographers"];
            //photosAdmins.photographyType = Convert.ToInt32(Request.Form["photographyType"]);

            //---------------

            //ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            //ViewBag.photographers = new SelectList(db.photographers, "id", "FullName");

            //    var photographerList = db.photographers.Select(p => new SelectListItem { Value = p.id.ToString(), Text = "https://localhost:44339/photographers/PhotographerProfile/" + p.id }).ToList();
            //    ViewBag.photographersProfile = photographerList;
            //    return View();

            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName");
            return View();
        }

        // POST: photosAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographerProfile,photo,created_at,photographyType")] photosAdmin photosAdmin)
        {
            if (ModelState.IsValid)
            {
                db.photosAdmins.Add(photosAdmin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins/Edit/5
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
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // POST: photosAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographerProfile,photo,created_at,photographyType")] photosAdmin photosAdmin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photosAdmin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographyType = new SelectList(db.PhotographyTypes, "TypeID", "TypeName", photosAdmin.photographyType);
            return View(photosAdmin);
        }

        // GET: photosAdmins/Delete/5
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
