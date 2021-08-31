using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMS_3.Models;

namespace SMS_3.Controllers.Registered_Course
{
    public class StRegisteredCoursController : Controller
    {
        private StudentManagementSystemEntities db = new StudentManagementSystemEntities();

        // GET: StRegisteredCours
        public ActionResult Index()
        {
            var stRegisteredCourses = db.StRegisteredCourses.Include(s => s.Course);
            return View(stRegisteredCourses.ToList());
        }

        // GET: StRegisteredCours/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StRegisteredCours stRegisteredCours = db.StRegisteredCourses.Find(id);
            if (stRegisteredCours == null)
            {
                return HttpNotFound();
            }
            return View(stRegisteredCours);
        }

        // GET: StRegisteredCours/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            return View();
        }

        // POST: StRegisteredCours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseRegistrationId,CourseId,StudentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] StRegisteredCours stRegisteredCours)
        {
            if (ModelState.IsValid)
            {
                stRegisteredCours.CourseRegistrationId = Guid.NewGuid();
                db.StRegisteredCourses.Add(stRegisteredCours);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", stRegisteredCours.CourseId);
            return View(stRegisteredCours);
        }

        // GET: StRegisteredCours/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StRegisteredCours stRegisteredCours = db.StRegisteredCourses.Find(id);
            if (stRegisteredCours == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", stRegisteredCours.CourseId);
            return View(stRegisteredCours);
        }

        // POST: StRegisteredCours/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseRegistrationId,CourseId,StudentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] StRegisteredCours stRegisteredCours)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stRegisteredCours).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", stRegisteredCours.CourseId);
            return View(stRegisteredCours);
        }

        // GET: StRegisteredCours/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StRegisteredCours stRegisteredCours = db.StRegisteredCourses.Find(id);
            if (stRegisteredCours == null)
            {
                return HttpNotFound();
            }
            return View(stRegisteredCours);
        }

        // POST: StRegisteredCours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            StRegisteredCours stRegisteredCours = db.StRegisteredCourses.Find(id);
            db.StRegisteredCourses.Remove(stRegisteredCours);
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
