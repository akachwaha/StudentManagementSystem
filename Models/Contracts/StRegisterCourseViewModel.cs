using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS_3.Models.Contracts
{
    public class StRegisterCourseViewModel
    {

        public string CourseName { get; set; }

        public string StudentID { get; set; }

        public string TutorName { get; set; }

        public string Duration { get; set; }
        public string Fees { get; set; }

    }
}