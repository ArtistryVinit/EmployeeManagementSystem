using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class LeaveApplication : ApprovalActivity
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Display(Name = "Number of Leave Days")]
        [Range(1, int.MaxValue, ErrorMessage = "Number of Leave Days must be greater than zero.")]
        public int NumberOfDays { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public int DurationId { get; set; }
        public SystemCodeDetail Duration { get; set; }

        [Display(Name = "Leave Type")]
        [Required]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

        public string? Attachment { get; set; }

        [Display(Name = "Notes")]
        public string? Description { get; set; }


        [Required]
        public int StatusId { get; set; }
        public SystemCodeDetail Status { get; set; }

    }
}
