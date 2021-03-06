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
        public ActionResult Index2()
        {
            var StRegisteredCours = db.StRegisteredCourses.Include(s => s.Course);
            return View(StRegisteredCours.ToList());
        }

        public ActionResult Index(string userEmail)
        {
            var StRegisteredCours = (from str in db.StRegisteredCourses
                                     join s in db.students on str.StudentRegistrationNumber equals s.StudentRegistrationNumber
                                     join c in db.Courses on str.CourseId equals c.CourseId
                                     join tc in db.TutorCourseMappings on str.CourseId equals tc.CourseId
                                     join t in db.Tutors on tc.TutorId equals t.TutorId
                                     where s.Email == userEmail
                                     select new StRegisterCourseViewModel
                                     {
                                         CourseName = c.CourseName,
                                         Duration = c.Duration,
                                         TutorName = t.Tutorname,
                                         Fees = c.fees.ToString(),
                                         StudentID = s.StudentRegistrationNumber
                                     }).ToList();

            return View(StRegisteredCours.ToList());
        }

        // GET: StRegisteredCours/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stRegisteredCours = db.StRegisteredCourses.Find(id);
            if (stRegisteredCours == null)
            {
                return HttpNotFound();
            }
            return View(stRegisteredCours);
        }

        // GET: StRegisteredCours/Create
        [HttpGet]
        public ActionResult Create(Guid id, string userEmail)
        {
            ViewBag.CourseId = id;
            var studentId = db.students.FirstOrDefault(a => a.Email == userEmail).StudentRegistrationNumber;
            ViewBag.StudentId = studentId;
            StRegisterCourseViewModel viewModelData = (from tc in db.TutorCourseMappings
                                                       join str in db.Courses on tc.CourseId equals str.CourseId
                                                       join t in db.Tutors on tc.TutorId equals t.TutorId
                                                       where str.CourseId == id
                                                       select new StRegisterCourseViewModel
                                                       {
                                                           CourseName = str.CourseName,
                                                           Duration = str.Duration,
                                                           TutorName = t.Tutorname,
                                                           Fees = str.fees.ToString(),
                                                           StudentID = studentId

                                                       }).FirstOrDefault();

            return View(viewModelData);
        }

        // POST: StRegisteredCours/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Guid courseId, string studentRegistrationNumber)
        {
            if (ModelState.IsValid)
            {
                var stRegisteredCours = new StRegisteredCours
                {
                    CourseId = courseId,
                    CourseRegistrationId = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    StudentRegistrationNumber = studentRegistrationNumber
                };

                //stRegisteredCours.CreatedDate = DateTime.Now;
                //stRegisteredCours.CourseRegistrationId = Guid.NewGuid();
                var data = db.StRegisteredCourses.FirstOrDefault(a => a.CourseId == courseId && a.StudentRegistrationNumber == stRegisteredCours.StudentRegistrationNumber);
                if (data == null)
                {
                    db.StRegisteredCourses.Add(stRegisteredCours);
                    db.SaveChanges();

                    Models.student studentdetails = db.students.FirstOrDefault(a => a.StudentRegistrationNumber == stRegisteredCours.StudentRegistrationNumber);
                    Course coursedetails = db.Courses.FirstOrDefault(a => a.CourseId == stRegisteredCours.CourseId);
                    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseName", stRegisteredCours.CourseId);
                    var fromAddress = new MailAddress("gv9914667@gmail.com", "gv9914667@gmail.com");
                    var toAddress = new MailAddress(studentdetails.Email, studentdetails.Email);
                    const string fromPassword = "20.July.93";
                    const string subject = "Student Management COurse registration email";
                    string body = $"{studentdetails.Firstname} {studentdetails.Lastname} is registered to the course {coursedetails.CourseName}. Fee amount - { coursedetails.fees}";

                    ViewBag.message = $"Registered to the Course {coursedetails.CourseName}";

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
                    return RedirectToAction("Index", "Courses");

                }
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Courses");

        }





        // GET: StRegisteredCours/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stRegisteredCours = db.StRegisteredCourses.Find(id);
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
            var stRegisteredCours = db.StRegisteredCourses.Find(id);
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
            var stRegisteredCours = db.StRegisteredCourses.Find(id);
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
