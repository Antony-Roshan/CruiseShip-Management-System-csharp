using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
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
            return RedirectToAction("/Index");
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
            return RedirectToAction("/Index");
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
            return RedirectToAction("/Index");
        }


/*=============================================      VOYAGER BOOKING SECTION      =============================================*/



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
        public ActionResult Confirm(string tm, string dt)
        {
            var iids = Session["iid"];
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            Fitness_centre fitness_Centre = db.Fitness_centre_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Time == tm && x.Date == dt).ToList();
            var newss2 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Voyager_id == newss.Voyager_id && x.Time == tm && x.Date == dt).FirstOrDefault();
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
                    details.Booking_type = "Fitness Centre";
                    details.Booking_for_id = fitness_Centre.Fitness_id;
                    details.Voyager_id = newss.Voyager_id;
                    details.Date = dt;
                    details.Time = tm;

                    var check1 = db.Logins.Where(y => y.Login_id == ssid && y.Usertype == "Premium").FirstOrDefault();
                    if (check1 != null)
                    {
                        details.Status = "Premium Paid";
                        db.Booking_details_Table.Add(details);
                        db.SaveChanges();

                    }
                    else
                    {
                        details.Status = "Payment Pending";

                        db.Booking_details_Table.Add(details);
                        db.SaveChanges();
                    }

                    Payment payment = new Payment();
                    payment.Booking_details_id = details.Booking_details_id;
                    payment.Booking_for = "Fitness Centre";
                    payment.Amount = "300";
                    payment.Date = currentDate1;
                    db.Payments.Add(payment);
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Fitness Centre Booked Successfully...!";
                }
            }
            return RedirectToAction("FitnessCentreBooking");
        }


        public ActionResult ViewBookings()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from bd in dd.Booking_details_Table
                              join ft in dd.Fitness_centre_Table
                              on bd.Booking_for_id equals ft.Fitness_id
                              where bd.Voyager_id == newss.Voyager_id
                              select new FitnessBookingDetails
                              {
                                  Booking_details_id = bd.Booking_details_id,
                                  Name = bd.Voyager.First_name + " " + bd.Voyager.Last_name,
                                  Fitname = ft.Fitness_name,
                                  Place = ft.Place,
                                  Date = bd.Date,
                                  Time = bd.Time,
                                  Status = bd.Status,
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult FitnessPayment(int? id)
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
            /*int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var iids = Session["iid"];
            
            Fitness_centre fitness= db.Fitness_centre_Table.Find(iids);*/
            int biid = Convert.ToInt32(Session["bid"]);

            var ids = db.Booking_details_Table.Where(y => y.Booking_details_id == biid).FirstOrDefault();

            /*var amtid = Session["amts"];
            Payment payment = new Payment();
            payment.Booking_details_id = biid;
            payment.Booking_for = "Fitness Centre";
            payment.Amount = "300";
            payment.Date = currentDate1;
            db.Payments.Add(payment);*/

            TempData["AlertMessage"] = "Payment Succesfully...!";

            ids.Status = "Paid";
            db.Entry(ids).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewBookings");

        }

    }
}