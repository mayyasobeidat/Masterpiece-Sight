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
    public class commentsHomesController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: commentsHomes

        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var commentsHomes = db.commentsHomes.Include(c => c.photographer);
            return View(commentsHomes.ToList());
        }


        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Accept(int? id)
        {
            var comment = db.commentsHomes.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.is_approved = true;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }


        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Hide(int? id)
        {
            var comment = db.commentsHomes.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.is_approved = false;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }

        // GET: commentsHomes/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            commentsHome commentsHome = db.commentsHomes.Find(id);
            //if (commentsHome == null)
            //{
            //    return HttpNotFound();
            //}
            return View(commentsHome);
        }

        // GET: commentsHomes/Create

        [HandleError(View = "Error")]
        [Authorize(Roles = "Photographer")]
        public ActionResult Create(int? id)
        {

            var x = User.Identity.GetUserId();
            id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.photographer_id = id;


            //ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id");
            return View();
        }

        // POST: commentsHomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        [HandleError(View = "Error")]
        [Authorize(Roles = "Photographer")]
        public ActionResult Create([Bind(Include = "id,photographer_id,comment,is_approved,created_at")] commentsHome commentsHome)
        {
            if (ModelState.IsValid)
            {
                var x = User.Identity.GetUserId();
                int iduser = db.photographers.FirstOrDefault(a => a.user_id == x).id;
                ViewBag.clientid = iduser;

                commentsHome.photographer_id= iduser;
                commentsHome.created_at = DateTime.Now;
                db.commentsHomes.Add(commentsHome);
                commentsHome.is_approved = false;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Your comment has been submitted successfully.";

                return View();
            }

            //ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", commentsHome.photographer_id);
            return View(commentsHome);
        }

        // GET: commentsHomes/Edit/5
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            commentsHome commentsHome = db.commentsHomes.Find(id);
            //if (commentsHome == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", commentsHome.photographer_id);
            return View(commentsHome);
        }

        // POST: commentsHomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographer_id,comment,is_approved,created_at")] commentsHome commentsHome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commentsHome).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", commentsHome.photographer_id);
            return View(commentsHome);
        }

        // GET: commentsHomes/Delete/5

        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            commentsHome commentsHome = db.commentsHomes.Find(id);
            //if (commentsHome == null)
            //{
            //    //return HttpNotFound();
            //}
            return View(commentsHome);
        }

        // POST: commentsHomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            commentsHome commentsHome = db.commentsHomes.Find(id);
            db.commentsHomes.Remove(commentsHome);
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
