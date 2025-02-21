using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class SystemProfile: UserActivity
    {
        
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ProfileId { get; set; }

        public virtual SystemProfile Profile { get; set; }

        //public virtual ICollection<SystemProfile> Children { get; set; }

        public virtual ICollection<SystemProfile> Children { get; set; } = new List<SystemProfile>();


        public int? Order {  get; set; }
    }
}
