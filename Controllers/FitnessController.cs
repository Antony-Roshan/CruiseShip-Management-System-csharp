using CruiseshipApp.Models;
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class FitnessController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ViewResult Index()
        {
            return View(db.Fitness_centre_Table.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Fitness Centre Id Required");
            }
            Fitness_centre fitness_Centre  = db.Fitness_centre_Table.Find(id);
            if (fitness_Centre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Fitness Centre Not Found");
            }
            return View(fitness_Centre);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Fitness_centre fitness_Centre)
        {
            db.Fitness_centre_Table.Add(fitness_Centre);
            db.SaveChanges();
            TempData["AlertMessage"] = "Fitness Centre Added Successfully...!";
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Fitness Centre ID Required");
            }
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(id);
            if (fitness_Centre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Fitness Centre not Found");
            }
            return View(fitness_Centre);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(id);
            UpdateModel(fitness_Centre);
            db.SaveChanges();
            TempData["AlertMessage"] = "Fitness Centre Updated Successfully...!";
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Fitness Centre ID Required");
            }
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(id);
            if (fitness_Centre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Fitness Centre not Found");
            }
            return View(fitness_Centre);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(id);
            db.Fitness_centre_Table.Remove(fitness_Centre);
            db.SaveChanges();
            TempData["AlertMessage"] = "Fitness Centre Deleted Successfully...!";
            return RedirectToAction("Index");
        }

        public ViewResult FitnessCentreBooking()
        {
            return View(db.Fitness_centre_Table.ToList());
        }

        public ActionResult Book(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Fitness Centre Id Required");
            }
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(id);
            if (fitness_Centre == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Fitness Centre Not Found");
            }
            Session["iid"] = id;
            return View(fitness_Centre);
        }

        [HttpPost]
        public ActionResult Confirm(string booktime, string dt)
        {
            var iids = Session["iid"];
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Time == booktime && x.Status == "Booked").ToList();
            var newss2 = db.Booking_details_Table.Where(x => x.Voyager_id == newss.Voyager_id && x.Time == booktime && x.Status == "Booked").FirstOrDefault();
            int count1 = newss1.Count;

            if (newss2 != null)
            {
                TempData["AlertMessage"] = "Fitness centre is already booked...!"; 
            }
            else
            {
                if (count1 >= 50)
                {
                    TempData["AlertMessage"] = "Fitness centre is full. select a different timing...!";
                }
                else
                {
                    Booking_details details = new Booking_details();
                    details.Booking_type = "Fitness_Centre";
                    details.Booking_for_id = fitness_Centre.Fitness_id;
                    details.Voyager_id = newss.Voyager_id;
                    details.Date = dt;
                    details.Time = booktime;
                    details.Status = "Booked";
                    /*fitness_Centre.Status = "Booked";*/
                    db.Booking_details_Table.Add(details);
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Fitness Centre Booked Successfully...!";
                }
            }
            return RedirectToAction("FitnessCentreBooking");
        }

    }
}