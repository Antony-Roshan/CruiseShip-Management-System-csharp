using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
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
            /*Party_hall party_Hall = db.Party_hall_Table.Find(id);
            string fileName = Path.GetFileNameWithoutExtension(party_Hall.ImageFile.FileName);
            string extension = Path.GetExtension(party_Hall.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            party_Hall.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            party_Hall.ImageFile.SaveAs(fileName);
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                db.Party_hall_Table.AddOrUpdate(party_Hall);
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
            Party_hall party_Hall = db.Party_hall_Table.Find(iids);
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            var sid = Convert.ToInt32(iids);

            var newss1 = db.Booking_details_Table.Where(x => x.Booking_for_id == sid && x.Time == dt && x.Status == "Booked").ToList();
            var newss2 = db.Booking_details_Table.Where(x => x.Voyager_id == newss.Voyager_id && x.Time == dt && x.Status == "Booked").FirstOrDefault();
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
                    details.Booking_type = "Party_hall";
                    details.Booking_for_id = party_Hall.Hall_id;
                    details.Voyager_id = newss.Voyager_id;
                    details.Date = dt;
                    details.Time = tm;
                    details.Status = "Booked";
                    db.Booking_details_Table.Add(details);
                    db.SaveChanges();

                    TempData["AlertMessage"] = "Party Hall Booked Successfully...!";
                }
            }
            return RedirectToAction("PartyHallBooking");
        }
    }
}
