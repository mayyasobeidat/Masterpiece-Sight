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
using System.Net.Mail;
using System.Web.Helpers;
using System.Xml.Linq;


namespace sight.Controllers
{
    public class photographersController : Controller
    {
        private sightEntities db = new sightEntities();

        // GET: photographers
        public ActionResult Index()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser).Include(p => p.Subscriptions);
            return View(photographers.ToList());
        }
        public ActionResult Indexx()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }

        public ActionResult Indexxx()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }
        public ActionResult Photos()
        {
            var photos = db.photos.Include(p => p.photographer).Include(p => p.PhotographyType1);
            return View(photos.ToList());
        }

        public ActionResult state()
        {
            var photographers = db.photographers.Include(p => p.AspNetUser);
            return View(photographers.ToList());
        }



        public ActionResult Search(string search, string searchBut)
        {
            if (searchBut == "FullName")
            {
                return View("Indexx", db.photographers.Where(p => p.FullName.Contains(search)).ToList());
            }
            else if (searchBut == "PhoneNumber")
            {
                return View("Indexx", db.photographers.Where(p => p.PhoneNumber.Contains(search)).ToList());
            }
            else if (searchBut == "All")
            {
                return View("Indexx", db.photographers
                    .Where(p => p.FullName.ToLower().Contains(search.ToLower())
                            || p.PhoneNumber.ToLower().Contains(search.ToLower())
                            || p.AspNetUser.Email.ToLower().Contains(search.ToLower()))
                    .Include(p => p.AspNetUser)
                    .ToList());
            }
            else if (searchBut == "Email")
            {
                return View("Indexx", db.photographers.Include(p => p.AspNetUser).Where(p => p.AspNetUser.Email.Contains(search)).ToList());
            }
            else
            {
                return View("Indexx");
            }
        }

        public ActionResult SearchUser(string search, string searchBut)
        {
            var currentDate = DateTime.Now;

            if (searchBut == "FullName")
            {
                return View("photographers", db.photographers.Where(p => p.FullName.Contains(search)).Where(p => p.is_hidden == false).ToList());
            }
            else if (searchBut == "PhoneNumber")
            {
                return View("photographers", db.photographers.Where(p => p.PhoneNumber.Contains(search)).Where(p => p.is_hidden == false).ToList());
            }
            else if (searchBut == "All")
            {
                return View("photographers", db.photographers
                    .Where(p => p.FullName.ToLower().Contains(search.ToLower())
                            || p.PhoneNumber.ToLower().Contains(search.ToLower())
                            || p.AspNetUser.Email.ToLower().Contains(search.ToLower()))
                    .Where(p => p.is_hidden == false)
                    .Include(p => p.AspNetUser)
                    .ToList());
               
            }
            else if (searchBut == "Email")
            {
                return View("photographers", db.photographers.Include(p => p.AspNetUser).Where(p => p.AspNetUser.Email.Contains(search)).Where(p => p.is_hidden == false).ToList());
            }
         
            else
            {
                return View("photographers");
            }
        }



        public ActionResult Pays()
        {
            return View();
        }

        public ActionResult Accept(int? id)
        {
            var photographer = db.photographers.Find(id);

            var subsID = db.Subscriptions.FirstOrDefault(a => a.PhotographerId == id).ID;
            var subscription = db.Subscriptions.Find(subsID);

            subscription.PhotographerId = (int)id;
            subscription.startDate = DateTime.Now;
            subscription.endDate = DateTime.Now.AddDays(7);
            subscription.Price = 0;

            if (photographer == null)
            {
                return HttpNotFound();
            }
            photographer.accept = true;
            photographer.is_hidden = false;

            //db.Entry(subscription).State = EntityState.Added;

            db.Entry(subscription).State = EntityState.Modified;
            db.Entry(photographer).State = EntityState.Modified;
            db.SaveChanges();



            try
            {

                // Create a MailMessage object with the sender, recipient, subject, and body
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("sight.teams@gmail.com");
                mail.To.Add(photographer.AspNetUser.Email);
                mail.Subject = "Your request has been accepted ✅";
                mail.IsBodyHtml = true; //وبما أن mail.IsBodyHtml يستخدم القيمة الافتراضية false، فيجب تعيينه إلى true ليعرض الجدول بشكل صحيح. يمكنك استخدام الكود التالي:


                string message = @" <body> <center> <table class='body-wrap' style='text-align: center; width: 86%; font-family: arial,sans-serif; border: 12px solid #f2c511; border-spacing: 4px 20px; background-color: white; '> <tr> <img src='https://i.ibb.co/WVW3KT2/logo.png' style='width:36%;'/> </tr> <tr> <td> <center> <table bgcolor='#FFFFFF' width='80%' ' border='0'> <tbody> <tr> <td style='font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:13px;color:#202020;line-height:1.5'> <h1 style='color:#575252;text-align:center;'>📸 " + photographer.FullName + " 📸</h1> </td> </tr> <tr style='text-align:center;color:#575252;font-size:14px;'> <td> <span><h3>Congratulations<h3></span> </td> </tr> <tr style='text-align:center;color:#a2a2a2;font-size:14px;'> <td> <span>You have been accepted as a photographer in the Sight family</span> </td> </tr> <tr style='text-align:center;color:#a2a2a2;font-size:14px;height:45px;'> <td> <span> You will have 7 days free trial period from the moment your account is approved. After that, you need to choose the paid subscription that suits you to continue appearing on the site. </span> </td> </tr> <tr style='text-align:center;color:#a2a2a2;font-size:14px;height:45px;'> <td> <span> We thank you for the decision to join us, and we hope that we will live up to your expectations 💛 </span> </td> </tr> <tr style='text-align:center;color:#a2a2a2;font-size:14px;height:45px;'> <td> <span><b style='color:#575252;'>Go:</b> <a href='//localhost:44339/Home/Index'>Sight</a></span> </td> </tr> </tbody> </table> </center> </td> </tr> </table> </center> </body>"; 
                mail.Body = message;
                //mail.Body = "Dear " + photographer.FullName + ",\n\nYour request has been accepted. Thank you for using our services.";

                // Create a SmtpClient object to send the message
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587; // Use the appropriate port number for your SMTP server
                smtpClient.Credentials = new System.Net.NetworkCredential("sight.teams@gmail.com", "pykvbopfepgzgvvy");
                smtpClient.EnableSsl = true;

                // Send the message
                smtpClient.Send(mail);

            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                ViewBag.Error = "There was an error sending your message: " + ex.Message;
                return View("Contact");
            }


            return RedirectToAction("Indexxx", "photographers"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }

        //public ActionResult Reject(int? id)
        //{
        //    var photographer = db.photographers.Find(id);
        //    if (photographer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    photographer.accept = false;

        //    db.Entry(photographer).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToAction("Indexxx", "photographers"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        //}

        public ActionResult Deactivate(int? id)
        {
            var photographer = db.photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            photographer.is_hidden = true;
            db.Entry(photographer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "photographers"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }

        public ActionResult Activate(int? id)
        {
            var photographer = db.photographers.Find(id);
            if (photographer == null)
            {
                return HttpNotFound();
            }
            photographer.is_hidden = false;
            db.Entry(photographer).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "photographers"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
        }



        public ActionResult ActivateFree(int? id)
        {
            var photographer = db.photographers.Find(id);

            var subsID = db.Subscriptions.FirstOrDefault(a => a.PhotographerId == id).ID;
            var subscription = db.Subscriptions.Find(subsID);

            subscription.PhotographerId = (int)id;
            subscription.startDate = DateTime.Now;
            subscription.endDate = DateTime.Now.AddDays(7);
            subscription.Price = 0;

            if (photographer == null)
            {
                return HttpNotFound();
            }
            photographer.is_hidden = false;

            //db.Entry(subscription).State = EntityState.Added;

            db.Entry(subscription).State = EntityState.Added;
            db.Entry(photographer).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "photographers"); // أعد توجيه المستخدم إلى صفحة العرض الرئيسية للتعليقات
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


        public ActionResult photographers()
        {

            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.is_hidden == false).ToList();

            return View(photographers);

        }


        public ActionResult CityFiltter(int id)
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }
            db.SaveChanges();

             

            var photographers = db.photographers
                          .Include(p => p.AspNetUser)
                          .Where(p => p.photographer_cities.Any(c => c.city_id == id)).ToList();

            return View(photographers);
        }

        public ActionResult TypeFiltter(int id)
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }
            db.SaveChanges();


            var photographers = db.photographers
                          .Include(p => p.AspNetUser)
                          .Where(p => p.PhotographerTypes.Any(c => c.TypeID == id)).ToList();

            return View(photographers);
        }


        public ActionResult photographersInCity()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid"))
                               .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersInIrbid()
        {

            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Irbid"))
                             .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersInAmman()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Amman"))
                           .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersInJarash()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.photographer_cities)
                                .Where(p => p.photographer_cities.Any(c => c.city.cityName == "Jarash"))
                        .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }


        public ActionResult photographersPortrait()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Portrait"))
                             .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersProduct()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Product"))
                            .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersNewborn()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Newborn"))
                          .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersFood()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Food"))
                          .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersOccasions()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Occasions"))
                            .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }

        public ActionResult photographersLandscape()
        {
            var currentDate = DateTime.Now;
            var photographersCheck = db.photographers
                .Include(p => p.AspNetUser)
                .Where(p => p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now || p.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().startDate > DateTime.Now).ToList();

            foreach (var photographer in photographersCheck)
            {
                if (photographer.Subscriptions.OrderByDescending(s => s.ID).FirstOrDefault().endDate < DateTime.Now)
                {
                    photographer.is_hidden = true;
                }
            }

            db.SaveChanges();

            var photographers = db.photographers
                                .Include(p => p.PhotographerTypes)
                                .Where(p => p.PhotographerTypes.Any(c => c.PhotographyType.TypeName == "Landscape"))
                           .Where(p => p.is_hidden == false).ToList();
            return View("photographersInCity", photographers);
        }





        public ActionResult PhotographerProfile(int? id)
        {
            if(User.IsInRole("Client"))
            {
                var x = User.Identity.GetUserId();
                int iduser = db.clients.FirstOrDefault(a => a.user_id == x).id;
                ViewBag.clientid = iduser;
                ViewBag.photographerId = id;
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var photographer = db.photographers.Find(id);

            ViewBag.Name = photographer.FullName;


            if (photographer == null)
            {
                return HttpNotFound();
            }

            if (!photographer.accept || photographer.is_hidden)
            {
                return View("Error");
            }

            var sessionsCount = db.photo_sessions
                .Include(p => p.photographer)
                .Where(p => p.photographer_id == id && p.status == true)
                .Count();
            ViewBag.sessionsCount = sessionsCount;

            var clientsCount = db.photo_sessions
                .Where(p => p.photographer_id == id && p.status == true)
                .Select(p => p.client_id)
                .Distinct()
                .Count();
            ViewBag.clientsCount = clientsCount;

            var ServicesCount = db.PhotographerTypes
                    .Count(p => p.PhotographerID == id);

            ViewBag.ServicesCount = ServicesCount;

            return View(photographer);
        }

        //public ActionResult sessionsCount(int? id)
        //{
        //    var count = db.photo_sessions
        //               .Include(p => p.photographer)
        //              .Where(p => p.photographer_id == id && p.status)
        //              .Count();

        //    return Content(count.ToString());
        //}


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
