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
        public ActionResult GetRemainingBags(int Billname)
        {
            var numberofBags = db.AspNetBills.Where(x => x.Id == Billname).Select(x => x.Bags_Qty).First();
            return Json(numberofBags, JsonRequestBehavior.AllowGet);
        }

        // GET: AspNetSales/Create
        public ActionResult Create()
        {
            ViewBag.BillID = new SelectList(db.AspNetBills.Where(x => x.Bill_Status == "Not Generated"), "Id", "FarmerName");
            ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name");
            ViewBag.Today = DateTime.Now;
            return View();
        }

        // POST: AspNetSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      
        public ActionResult AddSales(List<AspNetSale> aspNetSale)
        {
            try { 
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
            }
            catch(Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return Content("1", "application/json");
            // ViewBag.BillID = new SelectList(db.AspNetBills, "Id", "Truck_No", aspNetSale.BillID);
            //ViewBag.CustomerID = new SelectList(db.AspNetCustomers, "Id", "Name", aspNetSale.CustomerID);

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
        //Sheheryer 29/12/2017
        //View TodaysSale added
        public ActionResult TodaysSale()
        {
            List<AspNetSale> todaySaleList = new List<AspNetSale>();
            todaySaleList= db.AspNetSales.Where(x => x.Date == DateTime.Today).Select(x => x).ToList();
            return View(todaySaleList);
        }
        public JsonResult FilterByDate(DateTime startDate, DateTime EndDate)
        {
            var datetoday = DateTime.Today;

            if (datetoday< startDate)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
            if (datetoday < EndDate)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
            if(startDate>EndDate)
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }
            var abc = db.AspNetSales.Where(x => x.Date >= startDate && x.Date<=EndDate).GroupBy(x => x.AspNetBill.AspNetFarmer.Name).Select(x => x).ToList();
            FarmerSalesModel[] lists = new FarmerSalesModel[abc.Count];
            for (int i = 0; i < abc.Count; i++)
            {
                lists[i] = new FarmerSalesModel();
                lists[i].Name = abc[i].Key;
            }
            for (int i = 0; i < abc.Count; i++)
            {
                string name = lists[i].Name;
                var sales = db.AspNetSales.Where(sale => sale.Date >= startDate && sale.Date <= EndDate && sale.AspNetBill.AspNetFarmer.Name == name).Select(x => x).ToList();
                lists[i].saleslist = new List<Sale>();
                foreach (var item in sales)
                {
                    Sale mysale = new Sale();
                    mysale.CustomerName = item.AspNetCustomer.Name; ;
                    mysale.Particular = item.Particular;
                    mysale.Quantity = item.Quantity;
                    mysale.Rate = item.Rate;
                    mysale.Amount = item.Amount;
                    mysale.Received = item.Received;
                    mysale.Discount = item.Discount;
                    mysale.Remaining = item.Remaining;
                    mysale.truck = item.AspNetBill.Truck_No;
                    lists[i].saleslist.Add(mysale);
                }
                
            }
            return Json(lists, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FarmersSale()
        {
            var abc=db.AspNetSales.GroupBy(x => x.AspNetBill.AspNetFarmer.Name).Select(x => x).ToList();
            FarmerSalesModel[] lists = new FarmerSalesModel[abc.Count];
            for (int i = 0; i < abc.Count; i++)
            {
                lists[i] = new FarmerSalesModel();
                lists[i].Name = abc[i].Key; 
            }
            for (int i = 0; i < abc.Count; i++)
            {
                string name = lists[i].Name;
                var sales = db.AspNetSales.Where(sale => sale.AspNetBill.AspNetFarmer.Name == name).Select(x => x).ToList();
                lists[i].saleslist = new List<Sale>();
                foreach (var item in sales)
                {
                    Sale mysale = new Sale();
                    mysale.CustomerName = item.AspNetCustomer.Name;
                    mysale.Particular = item.Particular;
                    mysale.Quantity = item.Quantity;
                    mysale.Rate = item.Rate;
                    mysale.Amount = item.Amount;
                    mysale.Received = item.Received;
                    mysale.Discount = item.Discount;
                    mysale.Remaining = item.Remaining;
                    mysale.truck = item.AspNetBill.Truck_No;
                    lists[i].saleslist.Add(mysale);
                }

            }
            return View(lists);
        }

        //Ends here
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
