using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.ViewModels
{
    public class UserViewModel
    {
        //[Required]
        public int ?Id { get; set; }

        [Required]
        public string Email { get; set; }

        //[Required]
        public string? FirstName { get; set; }

        //[Required]
        public string? MiddleName { get; set; }

        //[Required]
        public string? LastName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        //[Required]

        public string? Address { get; set; }

        [Required]
        public string UserName { get; set; }

        
    }
}
