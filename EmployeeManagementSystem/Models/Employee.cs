using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee: UserActivity
    {
      
        public int Id { get; set; }

     
        public string EmpNo { get; set; }
        
       
        public string FirstName { get; set; }

      
        public string MiddleName { get; set; }

       
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

      
        public string EmailAddress { get; set; }

      
        public string Country { get; set; }

     
        public DateTime DateOfBirth { get; set; }

       
        public string Address { get; set; }

       
        public string Department { get; set; }

    
        public string Designation { get; set; }


    }
}
