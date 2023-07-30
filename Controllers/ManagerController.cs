using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Manager()
        {
            return View();
        }
        CruiseshipDbEntities db = new CruiseshipDbEntities();

        public ViewResult ViewEmployees()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Employees
                              join yy in dd.Logins
                              on xx.Login_id equals yy.Login_id

                              select new ViewEmployeeDetails
                              {
                                  Name = xx.Employee_name,
                                  Phone = xx.Phone,
                                  Email = xx.Email,
                                  UserType = yy.Usertype
                              }).ToList();
                return View(result);
            }
        }
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(string en, string em, string ph, string un, string pwd, string ut)
        {
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                Login l = new Login();
                l.Username = un;
                l.Password = pwd;
                l.Usertype = ut;
                db.Logins.Add(l);
                db.SaveChanges();

                Employee e = new Employee();
                var ids = db.Logins.OrderByDescending(y => y.Login_id).FirstOrDefault();
                e.Login_id = ids.Login_id;
                e.Employee_name = en;
                e.Phone = ph;
                e.Email = em;
                db.Employees.Add(e);
                db.SaveChanges();
            }
            return RedirectToAction("ViewEmployees", "Manager");
        }
        /*public ActionResult DeleteEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Employee ID Required");
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Employee not Found");
            }
            return View(employee);
        }*/
        public ActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.Find(id);
            int Lid = employee.Login_id;
            db.Employees.Remove(employee);
            db.SaveChanges();
            Login login = db.Logins.Find(Lid);
            db.Logins.Remove(login);
            db.SaveChanges();
            return RedirectToAction("ViewEmployees");
        }

        public ActionResult AcceptPremiumVoyager()
        {
            return View(db.Voyagers.Where(x => x.Status == "PremiumPending").ToList());
        }

        public ActionResult Accept(int? id)
        {
            Voyager voyager = db.Voyagers.Find(id);
            int Lid = voyager.Login_id;
            voyager.Status = "Premium";
            UpdateModel(voyager);
            Login login = db.Logins.Find(Lid);
            login.Usertype = "Premium";
            UpdateModel(login);
            db.SaveChanges();
            TempData["AlertMessage"] = "Premium User accepted Successfully...!";
            return RedirectToAction("AcceptPremiumVoyager");  
        }

        public ActionResult Reject(int? id)
        {
            Voyager voyager = db.Voyagers.Find(id);
            int Lid = voyager.Login_id;
            db.Voyagers.Remove(voyager);
            db.SaveChanges();
            Login login = db.Logins.Find(Lid);
            db.Logins.Remove(login);
            db.SaveChanges();
            TempData["AlertMessage"] = "Premium User rejected Successfully...!";
            return RedirectToAction("AcceptPremiumVoyager");
        }

        public ActionResult ViewVoyagers()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Voyagers
                              join yy in dd.Logins
                              on xx.Login_id equals yy.Login_id

                              select new ViewVoyagers
                              {
                                  Name = xx.First_name + " " + xx.Last_name,
                                  Place = xx.Place,
                                  Gender = xx.Gender,
                                  Phone = xx.Phone,
                                  Email = xx.Email,
                                  Status = yy.Usertype
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult ViewBookings()
        {

            return View();
        }

        public ActionResult ViewMovieBookingDetails()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Movie_bookings
                              join yy in dd.Payments
                              on xx.Movie_bookings_id equals yy.Booking_details_id
                              join zz in dd.Movie_ticket_Table
                              on xx.Movie_id equals zz.Movie_id
                              join uu in dd.Voyagers
                              on xx.Voyager_id equals uu.Voyager_id
                              join vv in dd.Logins
                              on uu.Login_id equals vv.Login_id

                              select new ViewMovieBookingDetails
                              {
                                  Name = uu.First_name + " " +uu.Last_name,
                                  Usertype = vv.Usertype,
                                  MovieName = zz.Movie_name,
                                  Screen = zz.Screen,
                                  seat = xx.seat,
                                  Date = xx.Date,
                                  Time = zz.Time,
                                  Amount = yy.Amount,
                                  Status = xx.Status
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult ViewPartyBookingDetails()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Booking_details_Table
                              join yy in dd.Payments
                              on xx.Booking_details_id equals yy.Booking_details_id
                              join zz in dd.Party_hall_Table
                              on xx.Booking_for_id equals zz.Hall_id
                              join uu in dd.Voyagers
                              on xx.Voyager_id equals uu.Voyager_id
                              join vv in dd.Logins
                              on uu.Login_id equals vv.Login_id

                              select new ViewPartyBookingDetails
                              {
                                  Name = uu.First_name + " " + uu.Last_name,
                                  Usertype = vv.Usertype,
                                  HallName = zz.Hall_name,
                                  Occasion = zz.Occasion,
                                  Date = xx.Date,
                                  Time = xx.Time,
                                  Amount = yy.Amount,
                                  Status = xx.Status

                              }).ToList();
                return View(result);
            }
        }

        public ActionResult ViewFitnessBookingDetails()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Booking_details_Table
                              join yy in dd.Payments
                              on xx.Booking_details_id equals yy.Booking_details_id
                              join zz in dd.Fitness_centre_Table
                              on xx.Booking_for_id equals zz.Fitness_id
                              join uu in dd.Voyagers
                              on xx.Voyager_id equals uu.Voyager_id
                              join vv in dd.Logins
                              on uu.Login_id equals vv.Login_id

                              select new ViewFitnessBookingDetails
                              {
                                  Name = uu.First_name + " " + uu.Last_name,
                                  Usertype = vv.Usertype,
                                  FitName = zz.Fitness_name,
                                  Place = zz.Place,
                                  Date = xx.Date,
                                  Time = xx.Time,
                                  Amount = yy.Amount,
                                  Status = xx.Status
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult ViewSaloonBookingDetails()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from xx in dd.Saloon_bookings
                              join yy in dd.Payments
                              on xx.Saloon_booking_id equals yy.Booking_details_id
                              join zz in dd.Beauty_Saloon_Table
                              on xx.Saloon_id equals zz.Saloon_id
                              join uu in dd.Voyagers
                              on xx.Voyager_id equals uu.Voyager_id
                              join vv in dd.Logins
                              on uu.Login_id equals vv.Login_id

                              select new ViewSaloonBookingDetails
                              {
                                  Name = uu.First_name + " " + uu.Last_name,
                                  Usertype = vv.Usertype,
                                  SaloonName = zz.Saloon_name,
                                  Service = zz.saloon_service,
                                  Date = xx.Date,
                                  Time = xx.Time,
                                  Amount = yy.Amount,
                                  Status = xx.Status
                              }).ToList();
                return View(result);
            }
        }

    }
}
