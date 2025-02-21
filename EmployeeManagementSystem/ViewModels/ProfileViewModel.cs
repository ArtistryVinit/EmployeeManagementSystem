using EmployeeManagementSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.ViewModels
{
    public class ProfileViewModel
    {
        public List<SystemProfile>? Profiles { get; set; }

        public ICollection<int> RolesRightsIds { get; set; }

        public int[] Ids { get; set; }

        [Required]
        public string? RoleId { get; set; }

        [Required]
        public int TaskId { get; set; }
    }
}
