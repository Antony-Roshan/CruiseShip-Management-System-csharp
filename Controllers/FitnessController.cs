using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
            return RedirectToAction("Index");
        }

        public ViewResult FitnessCentreBooking()
        {
            return View(db.Fitness_centre_Table.ToList());
        }
    }
}