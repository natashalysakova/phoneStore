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
    public class ScoresController : Controller
    {
        private storeDBEntities db = new storeDBEntities();

        // GET: Scores
        public ActionResult Index()
        {
            var scores = db.Scores.Include(s => s.Phones).Include(s => s.Users);
            return View(scores.ToList());
        }

        // GET: Scores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scores scores = db.Scores.Find(id);
            if (scores == null)
            {
                return HttpNotFound();
            }
            return View(scores);
        }

        // GET: Scores/Create
        public ActionResult Create()
        {
            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Scores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,PhoneId,Result")] Scores scores)
        {
            if (ModelState.IsValid)
            {
                db.Scores.Add(scores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "Title", scores.PhoneId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", scores.UserId);
            return View(scores);
        }

        // GET: Scores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scores scores = db.Scores.Find(id);
            if (scores == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "Title", scores.PhoneId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", scores.UserId);
            return View(scores);
        }

        // POST: Scores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,PhoneId,Result")] Scores scores)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhoneId = new SelectList(db.Phones, "Id", "Title", scores.PhoneId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Name", scores.UserId);
            return View(scores);
        }

        // GET: Scores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scores scores = db.Scores.Find(id);
            if (scores == null)
            {
                return HttpNotFound();
            }
            return View(scores);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Scores scores = db.Scores.Find(id);
            db.Scores.Remove(scores);
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
