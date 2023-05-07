using sight.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace sight.Controllers
{
    public class HomeController : Controller
    {

        private sightEntities db = new sightEntities();
        public ActionResult Index()
        {
            return View(db.PhotographyTypes.ToList());

        }

        public ActionResult ChoosePhotographer()
        {

            var city = db.cities;
            return View(city.ToList());

        }

        public ActionResult gallery()
        {

            var photos = db.photosAdmins.OrderByDescending(x => x.id).Take(15);
            return View(photos.ToList());
        }

        public ActionResult commentsHome()
        {
            var comments = db.commentsHomes;
            return View(comments.ToList());
        }

      


        public PartialViewResult search(int city)
        {
            return PartialView();

        }

        public ActionResult BlockProfile()
        {

            return View();
        }

        public ActionResult Client()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Photographer()
        {
            ViewBag.city = new SelectList(db.cities.ToList(), "id", "cityName");
            ViewBag.type = new SelectList(db.PhotographyTypes.ToList(), "TypeID", "TypeName");

            return View();
        }



        public ActionResult About()
        {
            var ServicesCount = db.PhotographerTypes.Count();
            ViewBag.ServicesCount = ServicesCount;

            ViewBag.Message = "Your application description page.";

            return View(db.FAQs.ToList());

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SendEmail(string Name, string Email, string Subject, string Message)
        {
            try
            {
                // Create a MailMessage object with the sender, recipient, subject, and body
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Email);
                mail.To.Add("sight@photographer.net");
                mail.Subject = Subject;
                mail.Body = string.Format("Name: {0}\n\nEmail: {1}\n\nMessage: {2}", Name, Email, Message);

                // Create a SmtpClient object to send the message
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587; // Use the appropriate port number for your SMTP server
                smtpClient.Credentials = new System.Net.NetworkCredential("sight.teams@gmail.com", "pykvbopfepgzgvvy");
                smtpClient.EnableSsl = true;

                // Send the message
                smtpClient.Send(mail);

                TempData["Message"] = "Email sent successfully.";

                // Redirect the user to a "Thank you" page
                return View("Contact");
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                ViewBag.Error = "There was an error sending your message: " + ex.Message;
                return View("Contact");
            }
        }
    }
}