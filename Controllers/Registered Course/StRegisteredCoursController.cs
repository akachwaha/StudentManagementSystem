using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using SMS_3.Models;
using SMS_3.Models.Contracts;

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
      
        public ActionResult Create(Guid id)
        {
            StRegisterCourseViewModel viewModelData = (from tc in db.TutorCourseMappings
                                 join str in db.Courses on tc.CourseId equals str.CourseId
                                 join t in db.Tutors on tc.TutorId equals t.TutorId
                                 where str.CourseId == id
                                 select new StRegisterCourseViewModel
                                 {
                                     CourseName = str.CourseName,
                                     Duration = str.Duration,
                                     TutorName = t.Tutorname,
                                     Fees = str.fees.ToString()

                                 }).FirstOrDefault();

            return View(viewModelData);

            //ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName");
            //return View();
        }

        // POST: StRegisteredCours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CourseRegistrationId,CourseId,StudentID,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy")] StRegisteredCours stRegisteredCours)
        public ActionResult Create([Bind]StRegisterCourseViewModel stRegisterCourseViewModel)
        {
            if (ModelState.IsValid)
            {
                var courseId = db.Courses.FirstOrDefault(a => a.CourseName == stRegisterCourseViewModel.CourseName).CourseId;
                StRegisteredCours stRegisteredCours = new StRegisteredCours
                {
                    CourseId = courseId,
                    CourseRegistrationId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    StudentRegistrationNumber = stRegisterCourseViewModel.StudentID
                };

                //stRegisteredCours.CreatedDate = DateTime.Now;
                //stRegisteredCours.CourseRegistrationId = Guid.NewGuid();
                var data = db.StRegisteredCourses.FirstOrDefault(a => a.Course == stRegisteredCours.Course && a.StudentRegistrationNumber == stRegisteredCours.StudentRegistrationNumber);
                if (data != null)
                {
                    db.StRegisteredCourses.Add(stRegisteredCours);
                    db.SaveChanges();

                    student studentdetails = db.students.FirstOrDefault(a => a.StudentRegistrationNumber == stRegisteredCours.StudentRegistrationNumber);
                    Course coursedetails = db.Courses.FirstOrDefault(a => a.CourseId == stRegisteredCours.CourseId);                    
                    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", stRegisteredCours.CourseId);
                    var fromAddress = new MailAddress("gv9914667@gmail.com", "gv9914667@gmail.com");
                    var toAddress = new MailAddress(studentdetails.Email, studentdetails.Email);
                    const string fromPassword = "20.July.93";//"k_4A2aecT27WYXGCztsQGJi8BYuBr0R136ltw44";
                    const string subject = "Student Management COurse registration email";
                    string body = $"{studentdetails.Firstname} {studentdetails.Lastname} is registered to the course {coursedetails.CourseName}. Fee amount - { coursedetails.fees}";



                    var smtp = new SmtpClient()
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                        //Timeout = 20000
                    };

                    try
                    {
                        using (var message = new MailMessage(fromAddress, toAddress)
                        {
                            Subject = subject,
                            Body = body,
                        })
                        {
                            smtp.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                else
                {
                    ModelState.AddModelError("Duplicate entry", "student is already registered to this course. please select proper course for registration");
                    return View();
                }
                return RedirectToAction("Index");
            }

            return View();
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
