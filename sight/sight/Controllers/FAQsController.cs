﻿using System;
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
    public class FAQsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: FAQs
        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.FAQs.ToList());
        }

        // GET: FAQs/Details/5

        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            FAQ fAQ = db.FAQs.Find(id);
            //if (fAQ == null)
            //{
            //    return HttpNotFound();
            //}
            return View(fAQ);
        }

        // GET: FAQs/Create

        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: FAQs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "id,question,answer")] FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                db.FAQs.Add(fAQ);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(fAQ);
        }

        // GET: FAQs/Edit/5

        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            FAQ fAQ = db.FAQs.Find(id);
            //if (fAQ == null)
            //{
            //    return HttpNotFound();
            //}
            return View(fAQ);
        }

        // POST: FAQs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "id,question,answer")] FAQ fAQ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fAQ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            return View(fAQ);
        }

        // GET: FAQs/Delete/5
        [HandleError(View = "Error")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            FAQ fAQ = db.FAQs.Find(id);
            //if (fAQ == null)
            //{
            //    return HttpNotFound();
            //}
            return View(fAQ);
        }

        // POST: FAQs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FAQ fAQ = db.FAQs.Find(id);
            db.FAQs.Remove(fAQ);
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