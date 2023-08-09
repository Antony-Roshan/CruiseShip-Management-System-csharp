using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class FeedbackController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ActionResult Index()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            Voyager voyager = new Voyager();
            return View(db.Feedbacks.Where(x => x.Voyager_id == newss.Voyager_id).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string Feedback1)
        {
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy");
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

            Feedback feedback1 = new Feedback();
            feedback1.Voyager_id = newss.Voyager_id;
            feedback1.Date = currentDate1;
            feedback1.Feedback1 = Feedback1;
            db.Feedbacks.Add(feedback1);
            db.SaveChanges();
            TempData["AlertMessage"] = "Feedbackk Added Successfully...!";
            return RedirectToAction("/Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Feedback ID Required");
            }
            Feedback feedback= db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Feedback not Found");
            }
            return View(feedback);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy");
            Feedback feedback = db.Feedbacks.Find(id);
            feedback.Date = currentDate1;
            UpdateModel(feedback);
            db.SaveChanges();
            TempData["AlertMessage"] = "Feedback Updated Successfully...!";
            return RedirectToAction("/Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Feedback ID Required");
            }
            Feedback feedback= db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Feedback not Found");
            }
            return View(feedback);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Feedback feedback= db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
            db.SaveChanges();
            TempData["AlertMessage"] = "Feedback Deleted Successfully...!";
            return RedirectToAction("/Index");
        }

        public ViewResult ViewFeedback()
        {  
            return View(db.Feedbacks.ToList());
        }
    }
}