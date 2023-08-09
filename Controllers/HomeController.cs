using CruiseshipApp.Models;
using System.Linq;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        { 
            return View(); 
        }
        [HttpPost]
        public ActionResult Login(string un, string pwd)
        {
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                var Ldetails = db.Logins.Where(x => x.Username == un && x.Password == pwd).FirstOrDefault();

                if (Ldetails == null)
                {
                    ViewBag.Message = "Invalid Username and Password";
                }
                else
                {
                    Session["login_id"] = Ldetails.Login_id;

                    if (Ldetails.Usertype == "Manager")
                    {
                        return RedirectToAction("Manager", "Manager");
                    }
                    else
                    {
                        if (Ldetails.Usertype == "Voyager")
                        {
                            return RedirectToAction("VoyagerHome", "Home");
                        }
                        else
                        {
                            if (Ldetails.Usertype == "Premium")
                            {
                                return RedirectToAction("VoyagerHome", "Home");
                            }
                            else
                            {
                                if (Ldetails.Usertype == "EMovie")
                                {
                                    return RedirectToAction("EMovie", "Employee");
                                }
                                else
                                {
                                    if (Ldetails.Usertype == "EFitness")
                                    {
                                        return RedirectToAction("EFitness", "Employee");
                                    }
                                    else
                                    {
                                        if (Ldetails.Usertype == "EParty")
                                        {
                                            return RedirectToAction("EParty", "Employee");
                                        }
                                        else
                                        {
                                            if (Ldetails.Usertype == "ESaloon")
                                            {
                                                return RedirectToAction("ESaloon", "Employee");
                                            }
                                            else
                                            {
                                                if (Ldetails.Usertype == "EItems")
                                                {
                                                    return RedirectToAction("EItems", "Employee");
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            return View();
        }

        public ActionResult Voyager()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Voyager(string fn, string ln, string pl, string gn, string ph, string em, string un, string pwd)
        {
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                Login l = new Login();
                l.Username = un;
                l.Password = pwd;
                l.Usertype = "Voyager";
                db.Logins.Add(l);
                db.SaveChanges();

                Voyager v = new Voyager();
                var ids = db.Logins.OrderByDescending(y => y.Login_id).FirstOrDefault();
                v.Login_id = ids.Login_id;
                v.First_name = fn;
                v.Last_name = ln;
                v.Place = pl;
                v.Gender = gn;
                v.Phone = ph;
                v.Email = em;
                db.Voyagers.Add(v);
                db.SaveChanges();
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Premium()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Premium(string fn, string ln, string pl, string gn, string ph, string em, string un, string pwd)
        {
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                Login l = new Login();
                l.Username = un;
                l.Password = pwd;
                l.Usertype = "Pending";
                db.Logins.Add(l);
                db.SaveChanges();

                Voyager v = new Voyager();
                var ids = db.Logins.OrderByDescending(y => y.Login_id).FirstOrDefault();
                v.Login_id = ids.Login_id;
                v.First_name = fn;
                v.Last_name = ln;
                v.Place = pl;
                v.Gender = gn;
                v.Phone = ph;
                v.Email = em;
                v.Status = "PremiumPending";
                db.Voyagers.Add(v);
                db.SaveChanges();
                TempData["AlertMessage"] = "Please wait 2 hours for premium verification...!";

            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult VoyagerHome()
        {
            return View();
        }

        public ActionResult Sample()
        {

            return View();
        }
    }
}