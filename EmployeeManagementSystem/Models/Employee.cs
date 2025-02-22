using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Employee: UserActivity
    {
      
        public int Id { get; set; }

  
        public string EmpNo { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";

        //public string FullName { get; set; } // Ensure this exists

        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Country Name")]
        public int? CountryId { get; set; }

       
        public Country Country {  get; set; }

     
        public DateTime DateOfBirth { get; set; }

       
        public string Address { get; set; }

        [DisplayName("Department Name")]
        public int? DepartmentId { get; set; }

     
        public Department Department { get; set; }

        [DisplayName("Designation Name")]
        public int? DesignationId { get; set; }

        public Designation Designation { get; set; }

        [DisplayName("Gender Name")]
        public int? GenderId { get; set; }

        public SystemCodeDetail Gender { get; set; }

        [DisplayName("Employee Photo")]
        public string? Photo { get; set; }

        public DateTime? EmploymentDate { get; set; }

        public int? StatusId { get; set; }

        public SystemCodeDetail Status { get; set; }

        public DateTime? InactiveDate { get; set; }

        public int? CauseofInactivityId { get; set; }

        public SystemCodeDetail CauseofInactivity { get; set; }

        public DateTime? TerminationDate { get; set; }

        public int? ReasonforterminationId { get; set; }

        public SystemCodeDetail Reasonfortermination { get; set; }

        public int? BankId { get; set; }

        public Bank Bank { get; set; }

        public string? BankAccountNo { get; set; }

        public string? IBAN {  get; set; }

        public string? SwiftCode { get; set; }

        public string? NSSFNO { get; set; }

        public string? NHIF {  get; set; }

        public string? CompanyEmail { get; set; }

        public string? KARPIN { get; set; }

        public string? PassportNo { get; set; }

        public int? EmploymentTermsId { get; set; }

        public SystemCodeDetail EmploymentTerms { get; set; }



    }
}
