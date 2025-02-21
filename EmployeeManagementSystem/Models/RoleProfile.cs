using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class RoleProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }  // ✅ Ensure TaskId is an int to match SystemProfile.Id

        [ForeignKey("TaskId")]  // ✅ Explicitly define the foreign key
        public SystemProfile Task { get; set; }

        [Required]
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]  // ✅ Explicitly define the foreign key
        public IdentityRole Role { get; set; }
    }
}
