using SMS_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS_3.Controllers.Courses
{
    public class CourseControllerStatic : Controller
    {

        static IList<Course> coursesList = new List<Course>
        {
            new Course{CourseId=Guid.NewGuid(),CourseName="Java",fees=500,Duration="6" },
        };

        // GET: Course
        public ActionResult Index()
        {
            return View(coursesList);
        }

        // GET: Course/Details/5
        public ActionResult Details(Guid id)
        {
            Course courseData = coursesList.FirstOrDefault(a => a.CourseId == id);
            return View(courseData);
        }


        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(Course courseData)
        {
            try
            {
                // TODO: Add insert logic here
                coursesList.Add(courseData);


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(Guid id)
        {
            Course courseData = coursesList.FirstOrDefault(a => a.CourseId == id);

            return View(courseData);
        }

        // POST: Course/Edit/5
        [HttpPost]
        public ActionResult Edit(Course CourseData)
        {
            try
            {
                // TODO: Add update logic here
                //var edt = coursesList.FirstOrDefault();
                //Course courseData = coursesList.FirstOrDefault();//


                var edt = coursesList.FirstOrDefault(a => a.CourseId == CourseData.CourseId);
                coursesList.Remove(edt);
                coursesList.Add(CourseData);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(Guid id)
        {
            Course courseData = coursesList.FirstOrDefault(a => a.CourseId == id);


            return View(courseData);
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, Course course)
        {
            try
            {
                // TODO: Add delete logic here
                var edt = coursesList.FirstOrDefault(a => a.CourseId == id);
                coursesList.Remove(edt);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
