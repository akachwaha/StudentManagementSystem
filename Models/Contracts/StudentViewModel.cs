using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMS_3.Models.Contracts
{
    public class StudentViewModel
    {
        [Required]
        [MaxLength(30, ErrorMessage = "FirstName Exceeded the given Limit of 30")]
        [Display(Name ="First Name")]
        public string Firstname { get; set; }

        [Required]
        [MaxLength(30,ErrorMessage ="LastName Exceeded the given Limit of 30")]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoiningDate { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "FatherName Exceeded the given Limit of 30")]
        [Display(Name = "Father Name")]
        public string Fathername { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="InValid Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [MaxLength(200, ErrorMessage ="Invalid Address")]
        public string StudentAddress { get; set; }
        public bool? Status { get; set; }

        [Required]
        [MaxLength(20)]
        public string Qualification { get; set; }

    }
}