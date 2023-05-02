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
    public class clientsController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: clients
        public ActionResult Index()
        {
            var clients = db.clients.Include(c => c.AspNetUser);
            return View(clients.ToList());
        }

        public ActionResult state()
        {
            var clients = db.clients.Include(p => p.AspNetUser);
            return View(clients.ToList());
        }

        // GET: clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: clients/Create
        public ActionResult Create()
        {
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,photo,created_at,fullName")] client client)
        {
            if (ModelState.IsValid)
            {
                db.clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", client.user_id);
            return View(client);
        }

        // GET: clients/Edit/5
        public ActionResult Edit(int? id)
        {
            var x = User.Identity.GetUserId();
            id = db.clients.FirstOrDefault(a => a.user_id == x).id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.clients.Find(id);
            Session["photo"] = client.photo;
            Session["user_id"] = client.user_id;


            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", client.user_id);
            return View(client);
        }

        // POST: clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,photo,PhoneNumber,fullName")] client client, HttpPostedFileBase photo)
        {
            client.photo = Session["photo"].ToString();
            client.user_id = Session["user_id"].ToString();

            if (ModelState.IsValid)
            {
              

                string imgPath = "";
                if (photo != null)
                {
                    imgPath = Path.GetFileName(photo.FileName);
                    photo.SaveAs(Path.Combine(Server.MapPath("~/assetsUser/img/") + photo.FileName));
                    client.photo = imgPath;
                }
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", client.user_id);
            return View(client);
        }

        // GET: clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            client client = db.clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            client client = db.clients.Find(id);
            db.clients.Remove(client);
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
