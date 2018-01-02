using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JhulayLal_Enterprise.Models;
using System.IO;

namespace JhulayLal_Enterprise.Controllers
{
    [Authorize]
    public class AspNetCustomersController : Controller
    {
        private JhulayLalEntities db = new JhulayLalEntities();

        // GET: AspNetCustomers
        public ActionResult Index()
        {
            return View(db.AspNetCustomers.ToList());
        }

        // GET: AspNetCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCustomer aspNetCustomer = db.AspNetCustomers.Find(id);
            List<AspNetSale> aspNetSales = db.AspNetSales.Where(x => x.CustomerID == id).ToList();
            ViewBag.Name = aspNetCustomer.Name;
            ViewBag.ContactNo = aspNetCustomer.Contact_No;
            ViewBag.AccountNo = aspNetCustomer.AccountNo;
            ViewBag.Image = aspNetCustomer.Imagepath;
            ViewBag.CNIC = aspNetCustomer.CNIC;
            if (aspNetCustomer == null)
            {
                return HttpNotFound();
            }
            return View(aspNetSales);
        }

        // GET: AspNetCustomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City,Address,Contact_No,CNIC,AccountNo,Picture")] AspNetCustomer aspNetCustomer)
        {
            if (ModelState.IsValid)
            {

                db.AspNetCustomers.Add(aspNetCustomer);

                db.SaveChanges();
                int cutomerID = db.AspNetCustomers.Max(x => x.Id);
                AspNetSale aspNetSale = new AspNetSale();
                aspNetSale.Particular = "Opening Balance";
                aspNetSale.Date = DateTime.Now;
                aspNetSale.Remaining = Convert.ToInt32(Request.Form["OpeningBalance"]);
                aspNetSale.CustomerID = cutomerID;
                db.AspNetSales.Add(aspNetSale);
                return RedirectToAction("Index");
            }

            return View(aspNetCustomer);
        }


        public ActionResult Creaoote([Bind(Include = "Id,Name,City,Address,Contact_No,CNIC,AccountNo,Picture,image")] Customer aspomer)
        {
            if (ModelState.IsValid)
            {
                AspNetCustomer obj = new AspNetCustomer();

                obj.AccountNo = aspomer.AccountNo;
                obj.Address = aspomer.Address;
                obj.City = aspomer.City;
                obj.CNIC = aspomer.CNIC;
                obj.Contact_No = aspomer.Contact_No;
                obj.Name = aspomer.Name;

                db.AspNetCustomers.Add(obj);
                db.SaveChanges();
                string kk = "";
                if(aspomer.image != null)
                {
                    MemoryStream target = new MemoryStream();
                    aspomer.image.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    string ImageName = System.IO.Path.GetFileName(aspomer.image.FileName); //file2 to store path and url  
                    string physicalPath = Server.MapPath("~/Content/img/customer/" + obj.Id);
                    if (!Directory.Exists(physicalPath))
                    {
                        Directory.CreateDirectory(physicalPath);
                    }
                    try
                    {
                        obj.Imagepath = physicalPath + "\\Pic.jpg";
                        System.IO.File.WriteAllBytes(obj.Imagepath, data);
                    }
                    catch (Exception ex)
                    {

                    }
                    //aspomer.image.SaveAs(physicalPath);
                    kk = "/Content" + obj.Imagepath.Split(new string[] { "\\Content" }, StringSplitOptions.None)[1];
                    

                }
                else
                {
                    kk = "/Content/img/user.png";
                }
                db.AspNetCustomers.Where(p => p.Id == obj.Id).FirstOrDefault().Imagepath = kk;
                db.SaveChanges();



                //obj.Picture = BinaryReader.ReadBytes(dd);
                //byte[] image = BinaryReader.ReadBytes(108732);

                //db.AspNetCustomers.Add(aspNetCustomer);

                //db.SaveChanges();
                int cutomerID = db.AspNetCustomers.Max(x => x.Id);
                AspNetSale aspNetSale = new AspNetSale();
                aspNetSale.Particular = "Opening Balance";
                aspNetSale.Date = DateTime.Now;
                aspNetSale.Remaining = Convert.ToInt32(Request.Form["OpeningBalance"]);
                aspNetSale.CustomerID = cutomerID;
                db.AspNetSales.Add(aspNetSale);
                return RedirectToAction("Index");
            }

            return View(aspomer);
        }

        // GET: AspNetCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCustomer aspNetCustomer = db.AspNetCustomers.Find(id);
            if (aspNetCustomer == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCustomer);
        }

        // POST: AspNetCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,City,Address,Contact_No,CNIC,AccountNo,Picture")] AspNetCustomer aspNetCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetCustomer);
        }

        // GET: AspNetCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetCustomer aspNetCustomer = db.AspNetCustomers.Find(id);
            if (aspNetCustomer == null)
            {
                return HttpNotFound();
            }
            return View(aspNetCustomer);
        }

        // POST: AspNetCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetCustomer aspNetCustomer = db.AspNetCustomers.Find(id);
            db.AspNetCustomers.Remove(aspNetCustomer);
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

        public class CustomerInformation
        {
            public AspNetCustomer aspNetCustomer { get; set; }
            public int? Remaining { get; set; }
        }
        public JsonResult GetCustomerInfomration(int Id)
        {
            AspNetCustomer aspNetCustomer = db.AspNetCustomers.Find(Id);
            int? Remaining = db.AspNetSales.Where(x => x.CustomerID == Id).OrderByDescending(x => x.Id).Take(1).Select(x => x.Remaining).FirstOrDefault() ;
            CustomerInformation customerInformation = new CustomerInformation();
            customerInformation.aspNetCustomer = new AspNetCustomer();
            customerInformation.aspNetCustomer.Name = aspNetCustomer.Name;
            customerInformation.aspNetCustomer.Contact_No = aspNetCustomer.Contact_No;
            customerInformation.aspNetCustomer.CNIC = aspNetCustomer.CNIC;
            customerInformation.aspNetCustomer.AccountNo = aspNetCustomer.AccountNo;

            customerInformation.Remaining = Remaining;
            return Json(customerInformation, JsonRequestBehavior.AllowGet);
        }

        
    }
}
