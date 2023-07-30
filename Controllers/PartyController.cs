using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CruiseshipApp.Controllers
{
    public class PartyController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ViewResult Index()
        {
            return View(db.Party_hall_Table.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Hall Id Required");
            }
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            if (party_Hall == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Movie Not Found");
            }
            Party_hall party = new Party_hall();

            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                party = db.Party_hall_Table.Where(x => x.Hall_id == id).FirstOrDefault();
            }
            return View(party_Hall);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Party_hall party_Hall)
        {
            string fileName = Path.GetFileNameWithoutExtension(party_Hall.ImageFile.FileName);
            string extension = Path.GetExtension(party_Hall.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            party_Hall.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            party_Hall.ImageFile.SaveAs(fileName);
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                db.Party_hall_Table.Add(party_Hall);
                db.SaveChanges();
                TempData["AlertMessage"] = "Party Hall Added Successfully...!";
            }
            ModelState.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Hall Id Required");
            }
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            if (party_Hall == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Hall not Found");
            }
            return View(party_Hall);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            /*Party_hall party = db.Party_hall_Table.Find(id);
            string fileName = Path.GetFileNameWithoutExtension(party.ImageFile?.FileName);
            string extension = Path.GetExtension(party.ImageFile?.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            party.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            party.ImageFile?.SaveAs(fileName);
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                db.Party_hall_Table.AddOrUpdate(party);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View();*/

            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            UpdateModel(party_Hall);
            db.SaveChanges();
            TempData["AlertMessage"] = "Party Hall Updated Successfully...!";
            return RedirectToAction("Index");

        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Hall Id Required");
            }
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            if (party_Hall == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Hall not Found");
            }
            return View(party_Hall);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            db.Party_hall_Table.Remove(party_Hall);
            db.SaveChanges();
            TempData["AlertMessage"] = "Party Hall Deleted Successfully...!";
            return RedirectToAction("Index");
        }



/*=============================================      VOYAGER BOOKING SECTION      =============================================*/




        public ViewResult PartyHallBooking()
        {
            return View(db.Party_hall_Table.ToList());
        }

        public ActionResult Book(int? id)
        {
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
            Session["iid"] = id;
            return View(party_Hall);
        }
        [HttpPost]
        public ActionResult Confirm(string dt, string tm)
        {
            var iids = Session["iid"];
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");
            Party_hall party_Hall = db.Party_hall_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Date == dt && x.Time == tm).ToList();
            var newss2 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Voyager_id == newss.Voyager_id && x.Date == dt && x.Time == tm).FirstOrDefault();
            int count1 = newss1.Count;

            if (newss2 != null)
            {
                TempData["AlertMessage"] = "Party Hall is already booked...!";
            }
            else
            {
                if (count1 >= 1)
                {
                    TempData["AlertMessage"] = "Party Hall is not Available. select a different Date...!";
                }
                else
                {
                    Booking_details details = new Booking_details();
                    details.Booking_type = "Party hall";
                    details.Booking_for_id = party_Hall.Hall_id;
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
                    payment.Booking_for = "Party Hall";
                    payment.Amount = party_Hall.Price;
                    payment.Date = currentDate1;
                    db.Payments.Add(payment);
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Party Hall Booked Successfully...!";
                }
            }
            return RedirectToAction("PartyHallBooking");
        }

        public ActionResult ViewBookings()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from bd in dd.Booking_details_Table
                              join pt in dd.Party_hall_Table
                              on bd.Booking_for_id equals pt.Hall_id
                              where bd.Voyager_id == newss.Voyager_id
                              select new PartyBookingDetails
                              {
                                  Booking_details_id = bd.Booking_details_id,
                                  Name = bd.Voyager.First_name + " " + bd.Voyager.Last_name,
                                  Hallname = pt.Hall_name,
                                  Occasion = pt.Occasion,
                                  Date = bd.Date,
                                  Time = bd.Time,
                                  Amount = pt.Price,
                                  Status = bd.Status,
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult PartyPayment(int? id)
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
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();*/
            /*var iids = Session["iid"];*/
            
            
            int biid = Convert.ToInt32(Session["bid"]);

            var ids = db.Booking_details_Table.Where(y => y.Booking_details_id == biid).FirstOrDefault();
            /*var pricefind = ids.Booking_for_id;
            Party_hall party_Hall = db.Party_hall_Table.Find(pricefind);*/

            /*var amtid = Session["amts"];
            Payment payment = new Payment();
            payment.Booking_details_id = biid;
            payment.Booking_for = "Party Hall";
            payment.Amount = party_Hall.Price;
            payment.Date = currentDate1;
            db.Payments.Add(payment);
            db.SaveChanges();*/

            TempData["AlertMessage"] = "Payment Succesfully...!";

            ids.Status = "Paid";
            db.Entry(ids).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewBookings");

        }
    }
}
