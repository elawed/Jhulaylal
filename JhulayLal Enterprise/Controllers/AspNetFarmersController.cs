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
    public class AspNetFarmersController : Controller
    {
        private JhulayLalEntities db = new JhulayLalEntities();

        // GET: AspNetFarmers
        public ActionResult Index()
        {
            return View(db.AspNetFarmers.ToList());
        }

        // GET: AspNetFarmers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFarmer aspNetFarmer = db.AspNetFarmers.Find(id);
            if (aspNetFarmer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Image = aspNetFarmer.Imagepath;
            return View(aspNetFarmer);
        }

        // GET: AspNetFarmers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetFarmers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,City,Address,Contact_No,image")] Farmer etFarmer)
        {
            if (ModelState.IsValid)
            {
                AspNetFarmer obj = new AspNetFarmer();

                obj.Name = etFarmer.Name;
                obj.City = etFarmer.City;
                obj.Address = etFarmer.Address;
                obj.Contact_No = etFarmer.Contact_No;
                db.AspNetFarmers.Add(obj);
                db.SaveChanges();
                string kk = "";
                if(etFarmer.image != null)
                {
                    MemoryStream target = new MemoryStream();
                    etFarmer.image.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();
                    string ImageName = System.IO.Path.GetFileName(etFarmer.image.FileName); //file2 to store path and url  
                    string physicalPath = Server.MapPath("~/Content/img/farmer/" + obj.Id);
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
                        if (ex.InnerException.Message == null)
                            ViewBag.error = ex.Message;
                        else
                            ViewBag.error = ex.InnerException.Message;
                    }
                    //aspomer.image.SaveAs(physicalPath);
                    kk = "/Content" + obj.Imagepath.Split(new string[] { "\\Content" }, StringSplitOptions.None)[1];
                    //obj.Imagepath = kk;
                    //etFarmer.image.SaveAs(physicalPath);
                    //obj.Imagepath = "img/" + ImageName;

                    
                }
                else
                {
                    kk = "/Content/img/user.png";
                }
                db.AspNetFarmers.Where(p => p.Id == obj.Id).FirstOrDefault().Imagepath = kk;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(etFarmer);
        }

        // GET: AspNetFarmers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFarmer aspNetFarmer = db.AspNetFarmers.Find(id);
            if (aspNetFarmer == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFarmer);
        }

        // POST: AspNetFarmers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,City,Address,Contact_No")] AspNetFarmer aspNetFarmer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetFarmer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetFarmer);
        }

        // GET: AspNetFarmers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetFarmer aspNetFarmer = db.AspNetFarmers.Find(id);
            if (aspNetFarmer == null)
            {
                return HttpNotFound();
            }
            return View(aspNetFarmer);
        }

        // POST: AspNetFarmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetFarmer aspNetFarmer = db.AspNetFarmers.Find(id);
            db.AspNetFarmers.Remove(aspNetFarmer);
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
