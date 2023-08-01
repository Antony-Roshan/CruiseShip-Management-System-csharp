using CruiseshipApp.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace CruiseshipApp.Controllers
{
    public class EmployeeController : Controller
    {
        public ActionResult EMovie()
        {
            return View();
        }
        public ActionResult EFitness()
        {
            return View();
        }
        public ActionResult EParty()
        {
            return View();
        }
        public ActionResult ESaloon()
        {
            return View();
        }
        public ActionResult  EItems()
        {
            return View();
        }

        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ActionResult ViewOrders()
        {
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from od in dd.Orders_Table
                              join vv in dd.Voyagers
                              on od.Voyager_id equals vv.Voyager_id
                              select new OrderStackBookingDetails
                              {
                                  Order_id = od.Order_id,
                                  Name = vv.First_name + " " + vv.Last_name,
                                  Date = od.Date,
                                  Total = od.Total,
                                  Status = od.Status,
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult ItemOrderDetails(int? id)
        {

            /*int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
*/
            using (CruiseshipDbEntities dd = new CruiseshipDbEntities())
            {
                var result = (from od in dd.Order_details
                              join it in dd.Items_Table
                              on od.Item_id equals it.Item_id
                              where od.Order_id == id
                              select new OrderBookingDetails
                              {
                                  Name = it.Item_name,
                                  Price = it.Price,
                                  Quantity = od.Quantity,
                                  Amount = od.Amount,
                              }).ToList();
                return View(result);
            }
        }

        public ActionResult Deliver(int? id)
        {
            var ids = db.Orders_Table.Where(y => y.Order_id == id).FirstOrDefault();

            TempData["AlertMessage"] = "Delivered Succesfully...!";

            ids.Status = "Delivered";
            db.Entry(ids).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewOrders");
        }
    }
}