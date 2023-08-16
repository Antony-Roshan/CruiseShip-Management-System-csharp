using CruiseshipApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;



namespace CruiseshipApp.Controllers
{
    public class MovieController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ViewResult Index()
        {
            return View(db.Movie_ticket_Table.ToList());
        }
        /*public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Movie Id Required");
            }
            Movie_ticket movie_Ticket= db.Movie_ticket_Table.Find(id);
            if (movie_Ticket == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Movie Not Found");
            }
            return View(movie_Ticket);
        }*/

        public ActionResult Create()
        {
            var list = new List<int>() { 1,2,3,4};
            ViewBag.list = list;
            return View();
        }
        [HttpPost]
        public ActionResult Create(Movie_ticket movie_Ticket)
        {
            db.Movie_ticket_Table.Add(movie_Ticket);
            db.SaveChanges();
            TempData["AlertMessage"] = "Movie Added Successfully...!";
            return RedirectToAction("/Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Movie ID Required");
            }
            Movie_ticket movie_Ticket = db.Movie_ticket_Table.Find(id);
            if (movie_Ticket == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Movie not Found");
            }
            var list = new List<int>() { 1, 2, 3, 4 };
            ViewBag.list = list;
            return View(movie_Ticket);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            Movie_ticket movie_Ticket = db.Movie_ticket_Table.Find(id);
            UpdateModel(movie_Ticket);
            db.SaveChanges();
            TempData["AlertMessage"] = "Movie Updated Successfully...!";
            return RedirectToAction("/Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Movie ID Required");
            }
            Movie_ticket movie_Ticket = db.Movie_ticket_Table.Find(id);
            if (movie_Ticket == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Movie not Found");
            }
            return View(movie_Ticket);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Movie_ticket movie_Ticket = db.Movie_ticket_Table.Find(id);
            db.Movie_ticket_Table.Remove(movie_Ticket);
            db.SaveChanges();
            TempData["AlertMessage"] = "Movie Deleted Successfully...!";
            return RedirectToAction("/Index");
        }


/*=============================================      VOYAGER BOOKING SECTION      =============================================*/



        public ViewResult MovieTicketBooking()
        {
            return View(db.Movie_ticket_Table.ToList());
        }
        public ActionResult book(int? id)
        {
            Movie_ticket movie = db.Movie_ticket_Table.Find(id);
            Session["iid"] = id;
            return View(movie);
        }

        [HttpPost]
        public ActionResult Confirm(string dt, int st)
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var iids = Session["iid"];
            var iidds = Convert.ToInt32(iids);
            Movie_ticket movie_Ticket = db.Movie_ticket_Table.Find(iids);
            var amt = st * Convert.ToInt32(movie_Ticket.Price);


            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");

            int? sums = db.Movie_bookings.Where(x => x.Movie_id == iidds && x.Date == dt).Sum(x =>x.seat );

            var tot = sums + st;
            if ((tot) > 100)
            {
                TempData["AlertMessage"] = "Movie seat slot full...!";
                return RedirectToAction("MovieTicketBooking");
            }
            else
            {
                Movie_bookings movie_Bookings = new Movie_bookings();
                movie_Bookings.Voyager_id = newss.Voyager_id;
                movie_Bookings.Movie_id = movie_Ticket.Movie_id;
                movie_Bookings.seat = st;
                movie_Bookings.Total = movie_Bookings.Total + amt.ToString();
                movie_Bookings.Date = dt;

                var check1 = db.Logins.Where(y => y.Login_id == ssid && y.Usertype == "Premium").FirstOrDefault();
                if (check1 != null)
                {
                    movie_Bookings.Status = "Premium Paid";
                    db.Movie_bookings.Add(movie_Bookings);
                    db.SaveChanges();

                }
                else
                {
                    movie_Bookings.Status = "Payment Pending";
                    db.Movie_bookings.Add(movie_Bookings);
                    db.SaveChanges();

                    
                }

                Payment payment = new Payment();
                payment.Booking_details_id = movie_Bookings.Movie_bookings_id;
                payment.Booking_for = "Movie Ticket";
                payment.Amount = movie_Bookings.Total;
                payment.Date = currentDate1;
                db.Payments.Add(payment);
                db.SaveChanges();

                TempData["AlertMessage"] = "Movie booked successfull...!";

                return RedirectToAction("MovieTicketBooking");
            }
        }

        public ActionResult ViewBookings()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            using (CruiseshipDbEntities dd= new CruiseshipDbEntities())
            {
                List<Movie_bookings> slist = dd.Movie_bookings.ToList();
                List<MovieBookingDetails> addsend = slist.Select(x => new MovieBookingDetails
                {
                    Movie_bookings_id = Convert.ToInt32(x.Movie_bookings_id),
                    Voyager_id = Convert.ToInt32(x.Voyager_id),
                    name = x.Voyager.First_name + ' ' + x.Voyager.Last_name,
                    movie = x.Movie_ticket.Movie_name,
                    Date = x.Date,
                    Total = x.Total,
                    Status = x.Status
                }).Where(x => x.Voyager_id == newss.Voyager_id).ToList();
                return View(addsend);
            }
        }

        public ActionResult MoviePayment(int? id)
        {
            Session["bid"] = id;

            /*int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

            var check1 = db.Logins.Where(y => y.Login_id == ssid && y.Usertype == "Premium").FirstOrDefault();
            if (check1 != null)
            {
                TempData["AlertMessage"] = "Premium users need not make payment...!";
                return RedirectToAction("ViewBookings");
            }

            var check = db.Payments.Where(y => y.Booking_details_id == id).FirstOrDefault();
            if (check != null)
            {
                *//*return RedirectToAction("ConfirmPayment");*//*
                TempData["AlertMessage"] = "Payment already done...!";
                return RedirectToAction("ViewBookings");
            }
            else
            {
                
            }*/
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmPayment()
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
           /* int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var iids = Session["iid"];
            
            Movie_ticket movie = db.Movie_ticket_Table.Find(iids);*/
            int biid = Convert.ToInt32(Session["bid"]);
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");

            var ids = db.Movie_bookings.Where(y => y.Movie_bookings_id == biid).FirstOrDefault();
           

            ids.Status = "Paid";
            db.Entry(ids).State = EntityState.Modified;

            var pid = db.Payments.Where(y => y.Booking_details_id == biid).FirstOrDefault();

            pid.Date = currentDate1;
            db.Entry(ids).State = EntityState.Modified;

            db.SaveChanges();

            TempData["AlertMessage"] = "Payment Succesfully...!";

            return RedirectToAction("ViewBookings");
        }
    }
}