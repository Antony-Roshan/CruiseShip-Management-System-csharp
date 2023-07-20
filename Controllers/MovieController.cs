using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult Details(int? id)
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
        }

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
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

        public ViewResult MovieTicketBooking()
        {
            return View(db.Movie_ticket_Table.ToList());
        }
    }
}