using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS_3.Models.Contracts
{
    public class StudentReports : IEnumerable<StudentReports>
    {
        public string StudentName { get; set; }

        public string StudentRegistrationNumber { get; set; }
       
        public string CourseName { get; set; }

        public string CourseCode { get; set; }

        public IEnumerator<StudentReports> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}