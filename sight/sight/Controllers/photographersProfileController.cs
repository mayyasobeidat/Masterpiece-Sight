﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using sight.Models;

namespace sight.Controllers
{
    public class photographersProfileController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photographersProfile
        public ActionResult Index()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }

        public ActionResult Pays()
        {
            return View();
        }
      

        // GET: photographersProfile/Details/5
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

        // GET: photographersProfile/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: photographersProfile/Create
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

        // GET: photographersProfile/Edit/5
        public ActionResult Edit(int? id)
        {

            var x = User.Identity.GetUserId();
            id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
            ViewBag.phoID = id;
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

        // POST: photographersProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,FullName,subscription_type,profilePhoto,coverPhoto,bio,accept,is_hidden,created_at,age,instagram,facebook,twitter,linkedin,PhoneNumber")] photographer photographer, HttpPostedFileBase coverPhoto, HttpPostedFileBase profilePhoto)
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


        public ActionResult AddType(int id)
        {
            try
            {
                var x = User.Identity.GetUserId();
                int photographerID = db.photographers.FirstOrDefault(a => a.user_id == x).id;

                var photographerType = new PhotographerType()
                {
                    PhotographerID = photographerID,
                    TypeID = id
                };

                db.PhotographerTypes.Add(photographerType);
                db.SaveChanges();

                return RedirectToAction("Edit", "photographersProfile");
            }
            catch (DbUpdateException ex)
            {
                // يتم إلقاء استثناء عند تكرار مفتاح أساسي
                if (ex.InnerException?.InnerException is System.Data.SqlClient.SqlException sqlEx && sqlEx.Number == 2627)
                {
                    ModelState.AddModelError(string.Empty, "This type is already added!");
                    return RedirectToAction("Edit", "photographersProfile");

                }
                else
                {
                    throw;
                }
            }

        }

        public ActionResult DeleteType(int id)
        {
            var userId = User.Identity.GetUserId();
            var photographer = db.photographers.FirstOrDefault(a => a.user_id == userId);
            var photographerType = db.PhotographerTypes.FirstOrDefault(pt => pt.PhotographerID == photographer.id && pt.TypeID == id);

            if (photographerType == null)
            {
                ModelState.AddModelError(string.Empty, "Type not found!");
                return RedirectToAction("Edit", "photographersProfile");
            }

            db.PhotographerTypes.Remove(photographerType);
            db.SaveChanges();

            return RedirectToAction("Edit", "photographersProfile");
        }





        public ActionResult AddCity(int id)
        {
            try
            {
                var x = User.Identity.GetUserId();
                int photographerID = db.photographers.FirstOrDefault(a => a.user_id == x).id;

                var photographer_cities = new photographer_cities()
                {
                    photographer_id = photographerID,
                    city_id = id
                };

                db.photographer_cities.Add(photographer_cities);
                db.SaveChanges();

                return RedirectToAction("Edit", "photographersProfile");
            }
            catch (DbUpdateException ex)
            {
                // يتم إلقاء استثناء عند تكرار مفتاح أساسي
                if (ex.InnerException?.InnerException is System.Data.SqlClient.SqlException sqlEx && sqlEx.Number == 2627)
                {
                    ModelState.AddModelError(string.Empty, "This city is already added!");
                    return RedirectToAction("Edit", "photographersProfile");

                }
                else
                {
                    throw;
                }
            }

        }

        public ActionResult DeleteCity(int id)
        {
            var userId = User.Identity.GetUserId();
            var photographer = db.photographers.FirstOrDefault(a => a.user_id == userId);
            var photographer_cities = db.photographer_cities.FirstOrDefault(pt => pt.photographer_id == photographer.id && pt.city_id == id);

            if (photographer_cities == null)
            {
                ModelState.AddModelError(string.Empty, "City not found!");
                return RedirectToAction("Edit", "photographersProfile");
            }

            db.photographer_cities.Remove(photographer_cities);
            db.SaveChanges();

            return RedirectToAction("Edit", "photographersProfile");
        }








        // GET: photographersProfile/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: photographersProfile/Delete/5
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