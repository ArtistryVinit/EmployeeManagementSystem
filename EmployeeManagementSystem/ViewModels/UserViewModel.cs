﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.ViewModels
{
    public class UserViewModel
    {
   
        public string ?Id { get; set; }

        //[DisplayName("Email Address")]
        public string Email { get; set; }

      
        [DisplayName("First Name")]
        public string? FirstName { get; set; }

     
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

       
        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Address")]

        public string? Address { get; set; }

        //[DisplayName("User Name")]
        public string UserName { get; set; }

        //[DisplayName("National Id")]
        public string? NationalId { get; set; }

        public string? FullName { get; set; }

        [DisplayName("User Role")]
        public string? RoleId { get; set; }
        public string? Role { get; internal set; }
    }
}
