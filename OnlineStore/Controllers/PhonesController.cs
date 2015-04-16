using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class PhonesController : Controller
    {
        private storeDBEntities db = new storeDBEntities();

        // GET: Phones
        public ActionResult Index()
        {
            var phones = db.Phones.Include(p => p.Vendors);
            return View(phones.ToList());
        }

        // GET: Phones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phones phones = db.Phones.Find(id);
            if (phones == null)
            {
                return HttpNotFound();
            }
            return View(phones);
        }

        // GET: Phones/Create
        public ActionResult Create()
        {
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Title");
            return View();
        }

        // POST: Phones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,VendorId")] Phones phones)
        {
            if (ModelState.IsValid)
            {
                db.Phones.Add(phones);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Title", phones.VendorId);
            return View(phones);
        }

        // GET: Phones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phones phones = db.Phones.Find(id);
            if (phones == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Title", phones.VendorId);
            return View(phones);
        }

        // POST: Phones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,VendorId")] Phones phones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Title", phones.VendorId);
            return View(phones);
        }

        // GET: Phones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phones phones = db.Phones.Find(id);
            if (phones == null)
            {
                return HttpNotFound();
            }
            return View(phones);
        }

        // POST: Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Phones phones = db.Phones.Find(id);
            db.Phones.Remove(phones);
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
