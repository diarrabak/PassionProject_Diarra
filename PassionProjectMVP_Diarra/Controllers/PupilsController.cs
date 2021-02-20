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


        /// <summary>
        /// This method displays the pupil list to the view
        /// <example>Pupils/PupilList</example>
        /// </summary>
        /// <returns></returns>
        // GET: Pupils
        public ActionResult PupilList()
        {
            var pupils = db.Pupils.Include(p => p.Classe).Include(p => p.Location);
            return View(pupils.ToList());
        }

        /// <summary>
        /// <example>GET: Pupils/Details/5</example>
        /// <example>GET: Pupils/Details/1</example>
        /// </summary>
        /// <param name="id">Id of the selected pupil</param>
        /// <returns>Shows information about the pupil</returns>
        // 
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

        /// <summary>
        /// This method creates and adds a new pupil to the database
        /// <example>POST: Pupils/Create</example>
        /// </summary>
        /// <param name="pupil">Selected pupil</param>
        /// <returns></returns>
        // 
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

        /// <summary>
        /// This method shows the pupil which ID is provided
        /// <example>GET: Pupils/Edit/5</example>
        /// <example>GET: Pupils/Edit/1</example>
        /// </summary>
        /// <param name="id">ID of the selected pupil</param>
        /// <returns>Shows the selected information</returns>
        // 
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

        /// <summary>
        /// This method permits to edit and save the selected pupil
        /// <example>POST: Pupils/Edit/5</example>
        /// <example>POST: Pupils/Edit/4</example>
        /// </summary>
        /// <param name="pupil"></param>
        /// <returns></returns>
        // 
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

        /// <summary>
        /// This method shows the pupil to delete
        /// <example>GET: Pupils/Delete/5</example>
        /// <example>GET: Pupils/Delete/1</example>
        /// </summary>
        /// <param name="id">Id of the selected pupil</param>
        /// <returns></returns>
        // 
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

        /// <summary>
        /// This method permits to remove the selected pupil from the database
        /// <example>POST: Pupils/Delete/5</example>
        /// <example>POST: Pupils/Delete/1</example>
        /// </summary>
        /// <param name="id">Id of the selected pupil</param>
        /// <returns></returns>
        // 
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
