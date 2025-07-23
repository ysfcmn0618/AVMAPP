using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMAPP.Data.Entities
{
    internal class UserEntity : IdentityUser, IGenericField
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ResetPasswordToken { get; set; }
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = null!;
        public string? FullName { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public virtual ICollection<ProductEntity> Products { get; set; } 
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;       
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

    }

}
