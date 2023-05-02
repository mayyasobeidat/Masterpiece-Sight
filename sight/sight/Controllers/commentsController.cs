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
    public class commentsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: comments
        public ActionResult Index()
        {
            var comments = db.comments.Include(c => c.client).Include(c => c.photographer);
            return View(comments.ToList());
        }
        public ActionResult feedback()
        {

            var userId = User.Identity.GetUserId(); // 
            int iduser = db.clients.FirstOrDefault(a => a.user_id == userId).id;

            var comments = db.comments
                            .Include(p => p.client)
                            .Include(p => p.photographer)
                            .Where(p => p.client_id == iduser);

            return View(comments.ToList());

        }
        public ActionResult Accept(int? id)
        {
            var comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.is_approved = true;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "photographersProfile"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }

        public ActionResult Hide(int? id)
        {
            var comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.is_approved = false;
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "photographersProfile"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }
        

        public ActionResult feedbackPhot()
        {

            var userId = User.Identity.GetUserId(); // 
            int iduser = db.photographers.FirstOrDefault(a => a.user_id == userId).id;

            var comments = db.comments
                            .Include(p => p.client)
                            .Include(p => p.photographer)
                            .Where(p => p.photographer_id == iduser);

            return View(comments.ToList());

        }

        // GET: comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: comments/Create
        public ActionResult Create(int? id)
        {
            int photographerId = (int)id;

            ViewBag.client_id = new SelectList(db.clients, "id", "user_id");
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id");
            return View();
        }

        // POST: comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,photographer_id,client_id,comment1,is_approved,created_at")] comment comment, int? id)
        {
            if (ModelState.IsValid)
            {
                var x = User.Identity.GetUserId();
                int iduser = db.clients.FirstOrDefault(a => a.user_id == x).id;
                ViewBag.clientid = iduser;

                comment.client_id = iduser;
                comment.photographer_id = (int)id;
                comment.created_at = DateTime.Now;

                db.comments.Add(comment);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your comment has been submitted successfully.";

            }
            int photographerId = (int)id;


            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", comment.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", comment.photographer_id);
            return View(comment);
        }

        // GET: comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", comment.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", comment.photographer_id);
            return View(comment);
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,photographer_id,client_id,comment1,is_approved,created_at")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", comment.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", comment.photographer_id);
            return View(comment);
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comment comment = db.comments.Find(id);
            db.comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Edit", "Clients");
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
