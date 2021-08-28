using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS_3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() //default action method//action method should be public and cannot perform overloading methods and cannot be static//
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]

        public ActionResult GetData(string input)
        {
            return View(input);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}