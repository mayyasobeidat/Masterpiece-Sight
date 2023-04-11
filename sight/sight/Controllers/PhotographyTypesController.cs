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
    public class PhotographyTypesController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: PhotographyTypes
        public ActionResult Index()
        {
            return View(db.PhotographyTypes.ToList());
        }

  


        // GET: PhotographyTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographyType photographyType = db.PhotographyTypes.Find(id);
            if (photographyType == null)
            {
                return HttpNotFound();
            }
            return View(photographyType);
        }

        // GET: PhotographyTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhotographyTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeID,TypeName,ImageUrl,Description")] PhotographyType photographyType, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {

                string imgPath = "";
                if (ImageUrl != null)
                {
                    imgPath = Path.GetFileName(ImageUrl.FileName);
                    ImageUrl.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/") + ImageUrl.FileName));
                }

                photographyType.ImageUrl = imgPath;



                db.PhotographyTypes.Add(photographyType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photographyType);
        }

        // GET: PhotographyTypes/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographyType photographyType = db.PhotographyTypes.Find(id);
            Session["ImageUrl"] = photographyType.ImageUrl;

            if (photographyType == null)
            {
                return HttpNotFound();
            }
            return View(photographyType);
        }

        // POST: PhotographyTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeID,TypeName,ImageUrl,Description")] PhotographyType photographyType, HttpPostedFileBase ImageUrl)
        {
            photographyType.ImageUrl = Session["ImageUrl"].ToString();

            if (ModelState.IsValid)
            {
                string imgPath = "";
                if (ImageUrl != null)
                {
                    imgPath = Path.GetFileName(ImageUrl.FileName);
                    ImageUrl.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/") + ImageUrl.FileName));
                    photographyType.ImageUrl = imgPath;

                }


                db.Entry(photographyType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photographyType);
        }


        // GET: PhotographyTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographyType photographyType = db.PhotographyTypes.Find(id);
            if (photographyType == null)
            {
                return HttpNotFound();
            }
            return View(photographyType);
        }

        // POST: PhotographyTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotographyType photographyType = db.PhotographyTypes.Find(id);
            db.PhotographyTypes.Remove(photographyType);
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
