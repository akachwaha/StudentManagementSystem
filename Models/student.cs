//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMS_3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class student
    {
        public System.Guid StudentID { get; set; }
        public string StudentRegistrationNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public System.DateTime DateofBirth { get; set; }
        public System.DateTime JoiningDate { get; set; }
        public string Fathername { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StudentAddress { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Qualification { get; set; }
    }
}
