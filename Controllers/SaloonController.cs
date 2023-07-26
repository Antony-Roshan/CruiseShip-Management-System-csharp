using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class SaloonController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ViewResult Index()
        {
            return View(db.Beauty_Saloon_Table.ToList());
        }
        /*public ActionResult Details(int? id)
        {
            if(id== null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Saloon Id Required");
            }
            Beauty_Saloon beauty_Saloon = db.Beauty_Saloon_Table.Find(id);
            if(beauty_Saloon == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Saloon not found");
            }
            return View(beauty_Saloon);
        }*/
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Beauty_Saloon beauty_Saloon) 
        {
            db.Beauty_Saloon_Table.Add(beauty_Saloon);
            db.SaveChanges();
            TempData["AlertMessage"] = "Beauty Saloon Added Successfully...!";
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Saloon Id Required ");
            }
            Beauty_Saloon beauty_Saloon = db.Beauty_Saloon_Table.Find(id);
            if( beauty_Saloon == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Saloon not found");
            }
            return View(beauty_Saloon);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            Beauty_Saloon beauty_saloon = db.Beauty_Saloon_Table.Find(id);
            UpdateModel(beauty_saloon);
            db.SaveChanges();
            TempData["AlertMessage"] = "Beauty Saloon Updated Successfully...!";
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Saloon ID Required");
            }
            Beauty_Saloon beauty_Saloon= db.Beauty_Saloon_Table.Find(id);
            if (beauty_Saloon == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Saloon not Found");
            }
            return View(beauty_Saloon);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Beauty_Saloon beauty_Saloon = db.Beauty_Saloon_Table.Find(id);
            db.Beauty_Saloon_Table.Remove(beauty_Saloon);
            db.SaveChanges();
            TempData["AlertMessage"] = "Beauty Saloon Deleted Successfully...!";
            return RedirectToAction("Index");
        }
        public ViewResult BeautySaloonBooking()
        {
            return View(db.Beauty_Saloon_Table.ToList());
        }

        public ActionResult Book(int? id)
        {
            Beauty_Saloon saloon  = db.Beauty_Saloon_Table.Find(id);
            Session["iid"] = id;
            return View(saloon);
        }

        [HttpPost]
        public ActionResult Confirm(string dt, string tm)
        {
            var iids = Session["iid"];
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            Beauty_Saloon saloon = db.Beauty_Saloon_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Saloon_bookings.Where(x => x.Saloon_id == sid && x.Date == dt && x.Time == tm && x.Status == "Booked").ToList();
            var newss2 = db.Saloon_bookings.Where(x => x.Voyager_id == newss.Voyager_id && x.Date == dt && x.Time == tm && x.Status == "Booked").FirstOrDefault();
            int count1 = newss1.Count;
            
            if(count1 >= 3)
            {
                TempData["AlertMessage"] = "Saloon slot not available select a different timing...!";
            }
            else
            {
                if(newss2 != null)
                {
                    TempData["AlertMessage"] = "Saloon is already booked...!";
                }
                else
                {
                    Saloon_booking saloon_Booking = new Saloon_booking();
                    saloon_Booking.Voyager_id = newss.Voyager_id;
                    saloon_Booking.Saloon_id = saloon.Saloon_id;
                    saloon_Booking.Date = dt;
                    saloon_Booking.Time = tm;
                    saloon_Booking.Status = "Booked";
                    db.Saloon_bookings.Add(saloon_Booking);
                    db.SaveChanges();
                    TempData["AlertMessage"] = "Saloon Appointment Booked...!";
                }
            }

            return RedirectToAction("BeautySaloonBooking");
        }

        public ActionResult SaloonPayment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmPayment()
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var iids = Session["iid"];
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");
            Movie_ticket movie = db.Movie_ticket_Table.Find(iids);

            int biid = Convert.ToInt32(Session["bid"]);
            var amtid = Session["amts"];
            Payment payment = new Payment();
            payment.Booking_details_id = biid;
            payment.Booking_for = "Movie Ticket";
            payment.Amount = amtid.ToString();
            payment.Date = currentDate1;
            db.Payments.Add(payment);
            db.SaveChanges();

            TempData["AlertMessage"] = "Payment Succesfully...!";

            return RedirectToAction("MovieTicketBooking");
        }
    }
}