using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices;
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
            return RedirectToAction("/Index");
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
            return RedirectToAction("/Index");
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
            return RedirectToAction("/Index");
        }



/*=============================================      VOYAGER BOOKING SECTION      =============================================*/



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
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");
            Beauty_Saloon saloon = db.Beauty_Saloon_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Saloon_bookings.Where(x => x.Saloon_id == sid && x.Date == dt && x.Time == tm).ToList();
            var newss2 = db.Saloon_bookings.Where(x => x.Voyager_id == newss.Voyager_id && x.Saloon_id == sid && x.Date == dt && x.Time == tm).FirstOrDefault();
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
                  /*  saloon_Booking.Status = "Booked"*/;

                    var check1 = db.Logins.Where(y => y.Login_id == ssid && y.Usertype == "Premium").FirstOrDefault();
                    if (check1 != null)
                    {
                        saloon_Booking.Status = "Premium Paid";
                        db.Saloon_bookings.Add(saloon_Booking);
                        db.SaveChanges();

                    }
                    else
                    {
                        saloon_Booking.Status = "Payment Pending";
                        db.Saloon_bookings.Add(saloon_Booking);
                        db.SaveChanges();
                    }

                    var ids = db.Beauty_Saloon_Table.Where(y => y.Saloon_id == saloon_Booking.Saloon_id).FirstOrDefault();

                    Payment payment = new Payment();
                    payment.Booking_details_id = saloon_Booking.Saloon_booking_id;
                    payment.Booking_for = "Beauty Saloon";
                    payment.Amount = ids.Price;
                    payment.Date = currentDate1;
                    db.Payments.Add(payment);
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Saloon Appointment Booked...!";
                }
            }
            return RedirectToAction("BeautySaloonBooking");
        }

        public ActionResult ViewBookings()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                List<Saloon_booking> slist = dd.Saloon_bookings.ToList();
                List<SaloonBookingDetails> addsend = slist.Select(x => new SaloonBookingDetails
                {
                    Saloon_booking_id = Convert.ToInt32(x.Saloon_booking_id),
                    Voyager_id = Convert.ToInt32(x.Voyager_id),
                    name = x.Voyager.First_name + ' ' + x.Voyager.Last_name,
                    saloon = x.Beauty_Saloon.Saloon_name,
                    Date = x.Date,
                    Time = x.Time,
                    amount = x.Beauty_Saloon.Price,
                    Status = x.Status
                }).Where(x => x.Voyager_id == newss.Voyager_id).ToList();
                return View(addsend);
            }
        }

        public ActionResult SaloonPayment(int? id)
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
                return RedirectToAction("ConfirmPayment");
                
            }
            else
            {
                TempData["AlertMessage"] = "Payment already done...!";
                return RedirectToAction("ViewBookings");
            }*/
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmPayment()
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            /*int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var iids = Session["iid"];*/

            /*Beauty_Saloon beauty_Saloon = db.Beauty_Saloon_Table.Find(iids);*/


            /*var slb = db.Saloon_bookings.Find(biid);
            var ids = db.Beauty_Saloon_Table.Where(y => y.Saloon_id == slb.Saloon_id).FirstOrDefault();*/
            int biid = Convert.ToInt32(Session["bid"]);
            var sbids = db.Saloon_bookings.Where(y => y.Saloon_booking_id == biid).FirstOrDefault();

            /*var amtid = Session["amts"];
            Payment payment = new Payment();
            payment.Booking_details_id = biid;
            payment.Booking_for = "Beauty Saloon";
            payment.Amount = ids.Price;
            payment.Date = currentDate1;
            db.Payments.Add(payment);
            db.SaveChanges();*/

            TempData["AlertMessage"] = "Payment Succesfully...!";

            sbids.Status = "Paid";
            db.Entry(sbids).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewBookings");

        }
    }
}