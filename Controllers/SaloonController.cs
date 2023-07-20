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
        public ActionResult Details(int? id)
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
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Beauty_Saloon beauty_Saloon) 
        {
            db.Beauty_Saloon_Table.Add(beauty_Saloon);
            db.SaveChanges();
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
            return RedirectToAction("Index");
        }
        public ViewResult BeautySaloonBooking()
        {
            return View(db.Beauty_Saloon_Table.ToList());
        }
    }
}