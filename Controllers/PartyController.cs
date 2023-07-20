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
            }
            ModelState.Clear();
            return View();
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
            Party_hall party_Hall = db.Party_hall_Table.Find(id);
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
            return View();
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
            return RedirectToAction("Index");
        }

        public ViewResult PartyHallBooking()
        {
            return View(db.Party_hall_Table.ToList());
        }
    }
}