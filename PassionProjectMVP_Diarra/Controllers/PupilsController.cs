using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PassionProjectMVP_Diarra.Models;

namespace PassionProjectMVP_Diarra.Controllers
{
    public class PupilsController : Controller
    {
        private PassionDataContext db = new PassionDataContext();

        // GET: Pupils
        public ActionResult PupilList()
        {
            var pupils = db.Pupils.Include(p => p.Classe).Include(p => p.Location);
            return View(pupils.ToList());
        }

        // GET: Pupils/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pupil pupil = db.Pupils.Find(id);
            if (pupil == null)
            {
                return HttpNotFound();
            }
            return View(pupil);
        }

        // GET: Pupils/Create
        public ActionResult Create()
        {
            ViewBag.classId = new SelectList(db.Classes, "classId", "className");
            ViewBag.locId = new SelectList(db.Locations, "locId", "city");
            return View();
        }

        // POST: Pupils/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pId,firstName,lastName,age,classId,locId")] Pupil pupil)
        {
            if (ModelState.IsValid)
            {
                db.Pupils.Add(pupil);
                db.SaveChanges();
                return RedirectToAction("PupilList");
            }

            ViewBag.classId = new SelectList(db.Classes, "classId", "className", pupil.classId);
            ViewBag.locId = new SelectList(db.Locations, "locId", "city", pupil.locId);
            return View(pupil);
        }

        // GET: Pupils/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pupil pupil = db.Pupils.Find(id);
            if (pupil == null)
            {
                return HttpNotFound();
            }
            ViewBag.classId = new SelectList(db.Classes, "classId", "className", pupil.classId);
            ViewBag.locId = new SelectList(db.Locations, "locId", "city", pupil.locId);
            return View(pupil);
        }

        // POST: Pupils/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pId,firstName,lastName,age,classId,locId")] Pupil pupil)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pupil).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("PupilList");
                return RedirectToAction("Details", new { id = pupil.pId });
            }
            ViewBag.classId = new SelectList(db.Classes, "classId", "className", pupil.classId);
            ViewBag.locId = new SelectList(db.Locations, "locId", "city", pupil.locId);
            return View(pupil);
        }

        // GET: Pupils/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pupil pupil = db.Pupils.Find(id);
            if (pupil == null)
            {
                return HttpNotFound();
            }
            return View(pupil);
        }

        // POST: Pupils/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pupil pupil = db.Pupils.Find(id);
            db.Pupils.Remove(pupil);
            db.SaveChanges();
            return RedirectToAction("PupilList");
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
