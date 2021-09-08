using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SMS_3.Models;
using SMS_3.Models.Contracts;

namespace SMS_3.Controllers.Student
{
    [Authorize(Roles = "Admin.Student")]
    public class studentsController : Controller
    {
        private StudentManagementSystemEntities db = new StudentManagementSystemEntities();

        // GET: students
        public ActionResult Index()
        {
            return View(db.students.ToList());
        }

        // GET: students/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentViewModel student)
        {

            if (ModelState.IsValid)
            {
                student studentDBModel = new student
                {
                    DateofBirth = student.DateofBirth,
                    Email = student.Email,
                    Fathername = student.Fathername,
                    Firstname = student.Firstname,
                    JoiningDate = DateTime.Now,
                    Lastname = student.Lastname,
                    Phone = student.Phone,
                    Qualification = student.Phone,
                    Status = true,
                    StudentAddress = student.StudentAddress,
                    StudentRegistrationNumber = "ST" + GenerateNewRandom(),
                    StudentID = Guid.NewGuid()
                };


                db.students.Add(studentDBModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }
        private static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }

        // GET: students/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentID,StudentRegistrationNumber,Firstname,Lastname,DateofBirth,Fathername,Email,Phone,StudentAddress,Status,Qualification")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: students/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = db.students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            student student = db.students.Find(id);
            db.students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult StudentReports(string coursename, string studentname)
        {
            using (StudentManagementSystemEntities managementSystemEntities = new StudentManagementSystemEntities())
            {
                List<string> coursesList = new List<string> { "All" };
                List<string> studentsList = new List<string> { "All" };
                var courseNames = managementSystemEntities.Courses.Select(a => a.CourseName).ToList();
                coursesList.AddRange(courseNames);
                ViewBag.Courses = coursesList;

                var studentsRecords = managementSystemEntities.students.Select(a => a.Firstname + " " + a.Lastname).Distinct().ToList();
                studentsList.AddRange(studentsRecords);

                ViewBag.Students = studentsList;

                if (coursename == "All" && studentname == "All")
                {
                    var studentRegisteredCourses = (from str in managementSystemEntities.StRegisteredCourses
                                                   join students in managementSystemEntities.students on str.StudentID equals students.StudentID
                                                   join course in managementSystemEntities.Courses on str.CourseId equals course.CourseId
                                                   select new StudentReports
                                                   {
                                                       StudentRegistrationNumber = students.StudentRegistrationNumber,
                                                       StudentName = students.Firstname + " " + students.Lastname,
                                                       CourseName = course.CourseName,
                                                       CourseCode = course.CourseCode
                                                   }).ToList();
                    return View(studentRegisteredCourses);
                }
            }



            return View();
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
