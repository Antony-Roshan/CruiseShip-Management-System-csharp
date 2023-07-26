﻿using CruiseshipApp.Models;
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
            return View(db.Employees.ToList());
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

        public ActionResult ViewPayments()
        {
            return View(db.Payments.ToList());
        }

    }
}