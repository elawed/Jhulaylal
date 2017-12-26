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
    public class AspNetBillsController : Controller
    {
        private JhulayLalEntities db = new JhulayLalEntities();

        // GET: AspNetBills
        public ActionResult Index()
        {
            var aspNetBills = db.AspNetBills.Include(a => a.AspNetFarmer);
            return View(aspNetBills.ToList());
        }

    

        public ActionResult NotGeneratedBills()
        {
            var aspNetBills = db.AspNetBills.Include(a => a.AspNetFarmer).Where(x=>x.Bill_Status=="Not Generated");
            return View("Index",aspNetBills.ToList());
        }

        public class Bill
        {
            public int FreightCharges { get; set; }
            public int Commission { get; set; }
            public int Labour { get; set; }
            public int MarketFee { get; set; }
            public int TelephoneCharges { get; set; }
            public int Accountant { get; set; }

        }

        public ActionResult GenerateBill(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var BagsDetail= db.AspNetBillDetails.Where(x => x.BillID == id).ToList();
            ViewBag.BagsDetail = BagsDetail;
            AspNetBill aspNetBill = db.AspNetBills.Find(id);
            aspNetBill.Amount = db.AspNetBillDetails.Where(x => x.BillID == id).Sum(x => x.Amount);
            aspNetBill.Freight_Charges = aspNetBill.Freight_Charges;
            aspNetBill.Commission = (aspNetBill.Amount * 5) / 100;
            aspNetBill.Labour = (aspNetBill.Amount * 10) / 100;
            aspNetBill.Market_Fees = BagsDetail.Sum(x => x.Bags) * 2;
            aspNetBill.Telephone_Charges = 5;
            aspNetBill.Accountant = BagsDetail.Sum(x => x.Bags);
            aspNetBill.Total_Expenses = aspNetBill.Freight_Charges + aspNetBill.Commission + aspNetBill.Labour + aspNetBill.Market_Fees + aspNetBill.Telephone_Charges + aspNetBill.Accountant;
            aspNetBill.Expenses = aspNetBill.Total_Expenses;
            aspNetBill.Total_Amount = aspNetBill.Amount - aspNetBill.Expenses;
            if (aspNetBill == null)
            {
                return HttpNotFound();
            }
            return View(aspNetBill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateBill(AspNetBill aspNetBill)
        {
           
            if (ModelState.IsValid)
            {
                AspNetBill aspNetBills = db.AspNetBills.Find(aspNetBill.Id);
                aspNetBill.FarmerID = aspNetBill.FarmerID;
                aspNetBills.Accountant = aspNetBill.Accountant;
                aspNetBills.Amount = aspNetBill.Amount;
                aspNetBills.Commission = aspNetBill.Commission;
                aspNetBills.Expenses = aspNetBill.Expenses;
                aspNetBills.Freight_Charges = aspNetBill.Freight_Charges;
                aspNetBills.Labour = aspNetBill.Labour;
                aspNetBills.Market_Fees = aspNetBill.Market_Fees;
                aspNetBills.Telephone_Charges = aspNetBill.Telephone_Charges;
                aspNetBills.Total_Amount = aspNetBill.Total_Amount;
                aspNetBills.Total_Expenses = aspNetBill.Total_Expenses;
                aspNetBills.Bill_Status = "Generated";
                db.SaveChanges();
                var BagsDetails = db.AspNetBillDetails.Where(x => x.BillID == aspNetBill.Id).ToList();
                ViewBag.BagsDetail = BagsDetails;
                return View(aspNetBills);
            }
            var BagsDetail = db.AspNetBillDetails.Where(x => x.BillID == aspNetBill.Id).ToList();
            ViewBag.BagsDetail = BagsDetail;
            aspNetBill.Bill_Status = "Not Generated";
            aspNetBill.Amount = db.AspNetBillDetails.Where(x => x.BillID == aspNetBill.Id).Sum(x => x.Amount);
            aspNetBill.Freight_Charges = aspNetBill.Freight_Charges;
            aspNetBill.Commission = (aspNetBill.Amount * 5) / 100;
            aspNetBill.Labour = (aspNetBill.Amount * 10) / 100;
            aspNetBill.Market_Fees = BagsDetail.Sum(x => x.Bags) * 2;
            aspNetBill.Telephone_Charges = 5;
            aspNetBill.Accountant = BagsDetail.Sum(x => x.Bags);
            aspNetBill.Total_Expenses = aspNetBill.Freight_Charges + aspNetBill.Commission + aspNetBill.Labour + aspNetBill.Market_Fees + aspNetBill.Telephone_Charges + aspNetBill.Accountant;
            aspNetBill.Expenses = aspNetBill.Total_Expenses;
            aspNetBill.Total_Amount = aspNetBill.Amount - aspNetBill.Expenses;
            return View(aspNetBill);
        }

        // GET: AspNetBills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            AspNetBill aspNetBill = db.AspNetBills.Find(id);
            if (aspNetBill == null)
            {
                return HttpNotFound();
            }
            return View(aspNetBill);
        }

        // GET: AspNetBills/Create
        public ActionResult Create()
        {

            ViewBag.FarmerID = new SelectList(db.AspNetFarmers, "Id", "Name");
            return View();
        }

        // POST: AspNetBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FarmerID,Date,Truck_No,Bags_Qty,Freight_Charges,Commission,Labour,Market_Fees,Telephone_Charges,Accountant,Total_Expenses,Amount,Expenses,Total_Amount")] AspNetBill aspNetBill)
        {
            if (ModelState.IsValid)
            {
                aspNetBill.Bill_Status = "Not Generated";
                aspNetBill.Freight_Charges = 0;
                db.AspNetBills.Add(aspNetBill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FarmerID = new SelectList(db.AspNetFarmers, "Id", "Name", aspNetBill.FarmerID);
            return View(aspNetBill);
        }

        // GET: AspNetBills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetBill aspNetBill = db.AspNetBills.Find(id);
            if (aspNetBill == null)
            {
                return HttpNotFound();
            }
            ViewBag.FarmerID = new SelectList(db.AspNetFarmers, "Id", "Name", aspNetBill.FarmerID);
            return View(aspNetBill);
        }

        // POST: AspNetBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FarmerID,Date,Truck_No,Bags_Qty,Freight_Charges,Commission,Labour,Market_Fees,Telephone_Charges,Accountant,Total_Expenses,Amount,Expenses,Total_Amount")] AspNetBill aspNetBill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetBill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FarmerID = new SelectList(db.AspNetFarmers, "Id", "Name", aspNetBill.FarmerID);
            return View(aspNetBill);
        }

        // GET: AspNetBills/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetBill aspNetBill = db.AspNetBills.Find(id);
            if (aspNetBill == null)
            {
                return HttpNotFound();
            }
            return View(aspNetBill);
        }

        // POST: AspNetBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetBill aspNetBill = db.AspNetBills.Find(id);
            db.AspNetBills.Remove(aspNetBill);
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
