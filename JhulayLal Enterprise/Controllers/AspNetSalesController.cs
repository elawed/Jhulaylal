using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JhulayLal_Enterprise.Models;

namespace JhulayLal_Enterprise.Controllers
{
    [Authorize]
    public class AspNetSalesController : Controller
    {
        private JhulayLalEntities db = new JhulayLalEntities();

        // GET: AspNetSales
        public ActionResult Index()
        {
            var aspNetSales = db.AspNetSales.Include(a => a.AspNetBill).Include(a => a.AspNetCustomer);
            return View(aspNetSales.ToList());
        }

        // GET: AspNetSales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSale aspNetSale = db.AspNetSales.Find(id);
            if (aspNetSale == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSale);
        }
        [HttpGet]
        public ActionResult GetRemainingBags(int FarmerID)
        {
            var numberofBags=db.AspNetBills.Where(x => x.FarmerID == FarmerID).Select(x => x.Bags_Qty).First();
            return Json(numberofBags,JsonRequestBehavior.AllowGet);
        }

        // GET: AspNetSales/Create
        public ActionResult Create()
        {
            var ty = (from name in db.AspNetFarmers
                      join e in db.AspNetBills on name.Id equals e.FarmerID select new
                      {
                          name.Name
                      }).ToList();
             
          
            ViewBag.BillID = new SelectList(db.AspNetBills.Where(x => x.Bill_Status == "Not Generated"), "Id", ty);
            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name");
            return View();
        }

        // POST: AspNetSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      
        public ActionResult AddSales(List<AspNetSale> aspNetSale)
        {
            if (ModelState.IsValid)
            {
                foreach(var sale in aspNetSale)
                {
                    db.AspNetSales.Add(sale);
                    AspNetBillDetail aspNetBillDetail = new AspNetBillDetail();
                    aspNetBillDetail.BillID = sale.BillID;
                    aspNetBillDetail.Bags = sale.Quantity;
                    aspNetBillDetail.Rate = sale.Rate;
                    aspNetBillDetail.Amount = sale.Amount;
                    db.AspNetBillDetails.Add(aspNetBillDetail);

                }
              
               db.SaveChanges();
               return RedirectToAction("Index");
            }

           // ViewBag.BillID = new SelectList(db.AspNetBills, "Id", "Truck_No", aspNetSale.BillID);
            //ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name", aspNetSale.CustomerID);
            return View(aspNetSale);
        }

        public ActionResult CashReceived()
        {
            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CashReceived(AspNetSale aspNetSale)
        {
            if (ModelState.IsValid)
            {
                db.AspNetSales.Add(aspNetSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name");
            return View(aspNetSale);
        }

        public JsonResult GetCustomerRemainings(int Id)
        {
            int? Remaining = db.AspNetSales.Where(x => x.CustomerID == Id).OrderByDescending(x => x.Id).Take(1).Select(x => x.Remaining).FirstOrDefault();
            return Json(Remaining, JsonRequestBehavior.AllowGet);
        }
        // GET: AspNetSales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSale aspNetSale = db.AspNetSales.Find(id);
            if (aspNetSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.BillID = new SelectList(db.AspNetBills, "Id", "Truck_No", aspNetSale.BillID);
            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name", aspNetSale.CustomerID);
            return View(aspNetSale);
        }

        // POST: AspNetSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerID,Date,Particular,BillID,Quantity,Rate,Amount,Received,Discount,Remaining")] AspNetSale aspNetSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BillID = new SelectList(db.AspNetBills, "Id", "Truck_No", aspNetSale.BillID);
            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name", aspNetSale.CustomerID);
            return View(aspNetSale);
        }

        // GET: AspNetSales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetSale aspNetSale = db.AspNetSales.Find(id);
            if (aspNetSale == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSale);
        }

        // POST: AspNetSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetSale aspNetSale = db.AspNetSales.Find(id);
            db.AspNetSales.Remove(aspNetSale);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
