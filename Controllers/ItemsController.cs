using CruiseshipApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.EnterpriseServices;

namespace CruiseshipApp.Controllers
{
    public class ItemsController : Controller
    {
        CruiseshipDbEntities db = new CruiseshipDbEntities();
        public ActionResult Index()
        {
            return View(db.Items_Table.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Item item)
        {
            string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
            string extension = Path.GetExtension(item.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            item .Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            item.ImageFile.SaveAs(fileName);
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                db.Items_Table.Add(item);
                db.SaveChanges();
                TempData["AlertMessage"] = "Items Added Successfully...!";
            }
            ModelState.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Item ID Required");
            }
            Item item = db.Items_Table.Find(id);
            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Item not Found");
            }
            return View(item);
        }
        [HttpPost]
        public ActionResult Edit(int id)
        {
            /*Item item = db.Items_Table.Find(id);
            string fileName = Path.GetFileNameWithoutExtension(item.ImageFile.FileName);
            string extension = Path.GetExtension(item.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            item.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            item.ImageFile.SaveAs(fileName);
            using (CruiseshipDbEntities db = new CruiseshipDbEntities())
            {
                db.Items_Table.Add(item);
                db.SaveChanges();
                TempData["AlertMessage"] = "Items Updated Successfully...!";
            }
            ModelState.Clear();
            return RedirectToAction("Index");*/
            Item item = db.Items_Table.Find(id);
            UpdateModel(item);
            db.SaveChanges();
            TempData["AlertMessage"] = "Items Updated Successfully...!";
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Item ID Required");
            }
            Item item = db.Items_Table.Find(id);
            if (item == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Item not Found");
            }
            return View(item);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Item item = db.Items_Table.Find(id);
            db.Items_Table.Remove(item);
            db.SaveChanges();
            TempData["AlertMessage"] = "Items Deleted Successfully...!";
            return RedirectToAction("Index");
        }



/*=============================================      VOYAGER BOOKING SECTION      =============================================*/



        public ViewResult ItemOrdereing()
        {
            return View(db.Items_Table.ToList());
        }

        public ActionResult Cart(int? id)
        {
            Item item = db.Items_Table.Find(id);
            Session["iid"] = id;
            return View(item);
        }

        [HttpPost]
        public ActionResult Done(string qn)
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();
            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            var iids = Session["iid"];
            Item item = db.Items_Table.Find(iids);
            var amt = Convert.ToInt32(qn) * Convert.ToInt32(item.Price);


            var newss1 = db.Orders_Table.Where(x => x.Voyager_id == newss.Voyager_id && x.Status=="Pending").FirstOrDefault();
            var idss= "";
            if (newss1 != null)
            {
                idss = newss1.Order_id.ToString();
                int ot= Convert.ToInt32(newss1.Total) + amt;
                newss1.Total = ot.ToString();
                db.Entry(newss1).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Order order = new Order();
                order.Voyager_id = newss.Voyager_id;
                order.Total = order.Total + amt;

                order.Date = currentDate1;
                order.Status = "Pending";
                db.Orders_Table.Add(order);
                db.SaveChanges();
                var ss=db.Orders_Table.OrderByDescending(y => y.Order_id).FirstOrDefault();
                idss = ss.Order_id.ToString();
            }


            Order_details order_Details = new Order_details();
            

            order_Details.Order_id = Convert.ToInt32(idss);
            order_Details.Item_id = item.Item_id;
            order_Details.Quantity = qn;
            order_Details.Amount = Convert.ToString(amt);
            db.Order_details.Add(order_Details);
            db.SaveChanges();

            TempData["AlertMessage"] = "Item Added to cart...!";

            return RedirectToAction("ItemOrdereing");
        }

        public ActionResult ViewOrders()
        {
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

            return View(db.Orders_Table.Where(x => x.Voyager_id == newss.Voyager_id && x.Status == "Pending").ToList());
            
        }

        public ActionResult SingleItemsDetails(int? id)
        {

            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

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

        public ActionResult OrderPayment(int? id)
        {

            Session["bid"] = id;

            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();

            var check1 = db.Logins.Where(y => y.Login_id == ssid && y.Usertype == "Premium" ).FirstOrDefault();
            if(check1 != null)
            {
                int biid = Convert.ToInt32(Session["bid"]);
                var ids = db.Orders_Table.Where(y => y.Order_id == biid).FirstOrDefault();
                ids.Status = "Paid";
                db.Entry(ids).State = EntityState.Modified;
                db.SaveChanges();

                TempData["AlertMessage"] = "Premium users need not make payment...!";
                return RedirectToAction("ViewOrders");
            }

            var check = db.Payments.Where(y => y.Booking_details_id == id).FirstOrDefault();
            if (check != null)
            {
                /*return RedirectToAction("ConfirmPayment");*/
                TempData["AlertMessage"] = "Payment already done...!";
                return RedirectToAction("ViewOrders");
            }
            else
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmPayment()
        {
            CruiseshipDbEntities db = new CruiseshipDbEntities();
            int ssid = Convert.ToInt32(Session["login_id"]);
            var newss = db.Voyagers.Where(x => x.Login_id == ssid).FirstOrDefault();


            /*var iids = Session["iid"];
            Item item = db.Items_Table.Find(iids);*/

            string currentDate1 = DateTime.Now.ToString("MM/dd/yyyy hh mm tt");
            int biid = Convert.ToInt32(Session["bid"]);

            /*int biid = Convert.ToInt32(Session["bid"]);*/

            var ids = db.Orders_Table.Where(y => y.Order_id == biid).FirstOrDefault();
            

            /*var amtid = Session["amts"];*/
            Payment payment = new Payment();
            payment.Booking_details_id = biid;
            payment.Booking_for = "Items";
            payment.Amount = ids.Total;
            payment.Date = currentDate1;
            db.Payments.Add(payment);
            db.SaveChanges();

            TempData["AlertMessage"] = "Payment Succesfully...!";

            ids.Status = "Paid";
            db.Entry(ids).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("ViewOrders");

        }
    }
}