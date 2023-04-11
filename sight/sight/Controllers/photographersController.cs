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

        public ActionResult Creates(int? id)
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
        public ActionResult Creates([Bind(Include = "id,photographer_id,client_id,comment1,is_approved,created_at")] comment comment, int? id)
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
            }
            int photographerId = (int)id;


            ViewBag.client_id = new SelectList(db.clients, "id", "user_id", comment.client_id);
            ViewBag.photographer_id = new SelectList(db.photographers, "id", "user_id", comment.photographer_id);
            return View(comment);
        }

        public ActionResult gallery()
        {
            var photos = db.photos;
            return View(photos.ToList());
        }
        public ActionResult photographers()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }
        public ActionResult photographersInCity()
        {
            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersInIrbid()
        {
            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersInAmman()
        {
            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Amman"))
                                .ToList();
            return View(photographers);
        }

         public ActionResult photographersInJarash()
        {
            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Jarash"))
                                .ToList();
            return View(photographers);
        }


        public ActionResult photographersPortrait()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Portrait"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersProduct()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Product"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersNewborn()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Newborn"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersFood()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Food"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersOccasions()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Occasions"))
                                .ToList();
            return View(photographers);
        }

        public ActionResult photographersLandscape()
        {
            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Landscape"))
                                .ToList();
            return View(photographers);
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
            try
            {
                var x = User.Identity.GetUserId();
                id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
                ViewBag.photographerid = id;

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
            catch (Exception ex)
            {
                // Log the exception
                // ...

                // Return a generic error page
                return View("Error");
            }


          
        }

        // POST: photographers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,FullName,subscription_type,profilePhoto,coverPhoto,bio,accept,is_hidden,age,instagram,facebook,twitter,linkedin,PhoneNumber")] photographer photographer, HttpPostedFileBase coverPhoto, HttpPostedFileBase profilePhoto, FormCollection form)
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






                var x = User.Identity.GetUserId();
                int id = db.photographers.FirstOrDefault(a => a.user_id == x).id;
    

                PhotographerType Type = new PhotographerType();

                foreach (var item2 in db.PhotographyTypes.ToList())
                { bool f = true;
                    string starValuea = form[item2.TypeID.ToString()];
                    if (starValuea == item2.TypeID.ToString())
                    {
                        int sstarValuea = Convert.ToInt32(starValuea);

                        if (Type.TypeID == sstarValuea)
                        {
                            f = false;
                        }
                        //Session["vv6"] = "Checked6" + item2.TypeID.ToString();
                        if (f == true)
                        {
                            Type.TypeID = sstarValuea;
                            Type.PhotographerID = id;
                        }


                    }
                    else
                    {
                        continue;
                        //Session["vv6"] = "UNChecked6"+ item2.OrderID.ToString();
                    }
                }


                db.PhotographerTypes.Add(Type);

                db.SaveChanges();
                return RedirectToAction("Edit");


            }
            ViewBag.user_id = new SelectList(db.AspNetUsers, "Id", "Email", photographer.user_id);
            return View(photographer);
        }

        // GET: photographers1/Delete/5
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
